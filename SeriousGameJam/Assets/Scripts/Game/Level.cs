using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

public class Level : MonoBehaviour {
	[Header("Data"), Space]
	string operatorName = "Operator";
	[SerializeField] PatientData[] patients;

	[Header("Refs"), Space]
	[SerializeField] DialogLogUI dialogLog;
	[SerializeField] DialogSelectUI dialogSelect;
	[SerializeField] ManualUI manual;

	int currPatientId = 0;
	PatientData currPatient;

	void Start() {
		StartNewPatient();
	}

	void StartNewPatient() {
		dialogLog.ClearLog();
		dialogSelect.Clear();

		currPatient = Instantiate(patients[currPatientId]);

		dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Вам звонить {currPatient.name}", onShowLog: ()=> {
			dialogSelect.AddButton("Пiдняти трубку", () => {
				NodeLinkData narrativeData = currPatient.dialogue.NodeLinks.First(); //Entrypoint node
				ProceedToNarrative(narrativeData.TargetNodeGUID);
			});
		});
	}

	void ProceedToNarrative(string narrativeDataGUID) {
		DialogueNodeData nodeData = currPatient.dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID);
		string text = nodeData.DialogueText;
		List<NodeLinkData> choices = currPatient.dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID).ToList();

		PatientData.PatientMoodData mood = currPatient.GetMoodData(nodeData.mood);

		dialogLog.AddToLog(DialogLogUI.LogEntryType.Patient, ProcessProperties(text), currPatient.name, mood.backColor, mood.avatar, () => {
			dialogSelect.Clear();
			foreach (var choice in choices) {
				string choiceText = ProcessProperties(choice.PortName);

				dialogSelect.AddButton(choiceText, () => {
					dialogLog.AddToLog(DialogLogUI.LogEntryType.Operator, choiceText, operatorName, onShowLog: () => {
						switch (mood.mood) {
							case PatientMood.Exit:
								EndPatient();
								break;

							case PatientMood.Normal:
							case PatientMood.Angry:
							default:
								ProceedToNarrative(choice.TargetNodeGUID);
								break;
						}
					});
				});
			}

			if (mood.mood == PatientMood.Exit) {
				EndPatient();
			}
		});
	}

	void EndPatient() {
		dialogSelect.Clear();

		++currPatientId;
		if (currPatientId == patients.Length) {
			dialogLog.ClearLog();
			dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Ви пройшли гру! Конгарц", onShowLog: () => {
				dialogSelect.AddButton("Start again", () => {
					currPatientId = 0;
					StartNewPatient();
				});
			});
		}
		else {
			dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Розмова закiнчена", onShowLog: ()=> {
				dialogSelect.AddButton("Покласти трубку", () => {
					StartNewPatient();
				});
			});
		}
	}

	string ProcessProperties(string processedText) {
		foreach (var exposedProperty in currPatient.dialogue.ExposedProperties)
			processedText = processedText.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
		return processedText;
	}
}
