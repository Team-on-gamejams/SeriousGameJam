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
		
		NodeLinkData narrativeData = currPatient.dialogue.NodeLinks.First(); //Entrypoint node
		ProceedToNarrative(narrativeData.TargetNodeGUID);
	}

	private void ProceedToNarrative(string narrativeDataGUID) {
		DialogueNodeData nodeData = currPatient.dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID);
		string text = nodeData.DialogueText;
		List<NodeLinkData> choices = currPatient.dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID).ToList();

		PatientData.PatientMoodData mood = currPatient.GetMoodData(nodeData.mood);

		dialogLog.AddToLog(false, currPatient.name, ProcessProperties(text), mood.backColor, mood.avatar);

		dialogSelect.Clear();
		foreach (var choice in choices) {
			string choiceText = ProcessProperties(choice.PortName);

			dialogSelect.AddButton(choiceText, ()=> {
				dialogLog.AddToLog(true, "Operator", choiceText);

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
		}

		string ProcessProperties(string processedText) {
			foreach (var exposedProperty in currPatient.dialogue.ExposedProperties)
				processedText = processedText.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
			return processedText;
		}

		if(mood.mood == PatientMood.Exit) {
			EndPatient();
		}
	}

	void EndPatient() {
		dialogSelect.Clear();

		LeanTween.delayedCall(1.0f, () => {
			++currPatientId;
			if (currPatientId == patients.Length) {
				dialogLog.ClearLog();
				dialogLog.AddToLog(true, "Operator", "You win");
				dialogSelect.AddButton("Start again", () => {
					currPatientId = 0;
					StartNewPatient();
				});
			}
			else {
				StartNewPatient();
			}
		});
	}
}
