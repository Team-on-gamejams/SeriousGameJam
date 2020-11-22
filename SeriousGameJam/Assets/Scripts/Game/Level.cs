using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

public class Level : MonoBehaviour {
	[Header("Music"), Space]
	[SerializeField] AudioClip defaultAmbient;
	[SerializeField] AudioClip winAmbient;
	[SerializeField] AudioClip loseAmbient;


	[Header("Data"), Space]
	string operatorName = "Operator";
	[SerializeField] PatientData[] patients;

	[Header("Refs"), Space]
	[SerializeField] DialogLogUI dialogLog;
	[SerializeField] DialogSelectUI dialogSelect;
	[SerializeField] ManualUI manual;

	int currPatientId = 0;
	PatientData currPatient;
	AudioSource winAmbientAS;

	void Start() {
		dialogSelect.ClearForce();

		StartNewPatient();
	}

	void StartNewPatient() {
		dialogLog.ClearLog();
		dialogSelect.Clear();

		if (winAmbientAS) {
			AudioManager.Instance.ChangeASVolume(winAmbientAS, 0.0f, 1.0f);
		}
		AudioManager.Instance.PlayMusic(defaultAmbient);
		currPatient = Instantiate(patients[currPatientId]);

		dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Вам звонить {currPatient.name}", onShowLog: ()=> {
			dialogSelect.AddButton("start", () => {
				dialogSelect.Clear();
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

		if (mood.mood == PatientMood.ExitNotOK) {
			AudioManager.Instance.PlayMusic(loseAmbient, time: 0.5f);
		}
		else if (mood.mood == PatientMood.ExitOk) {
			AudioManager.Instance.MuteMusic(AudioManager.Instance.crossfadeTime);
			winAmbientAS = AudioManager.Instance.Play(winAmbient, 0.5f, 1.0f, AudioManager.Instance.crossfadeTime);
			LeanTween.delayedCall(winAmbient.length - AudioManager.Instance.crossfadeTime, AudioManager.Instance.ContinueMusicAfterMute);
		}

		dialogLog.AddToLog(DialogLogUI.LogEntryType.Patient, ProcessProperties(text), currPatient.name, mood.backColor, currPatient, mood, () => {
			foreach (var choice in choices) {
				string choiceText = ProcessProperties(choice.PortName);

				dialogSelect.AddButton(choiceText, () => {
					dialogSelect.Clear();
					dialogLog.AddToLog(DialogLogUI.LogEntryType.Operator, choiceText, operatorName, minTimeToCallShow: 0.5f, onShowLog: () => {
						switch (mood.mood) {
							case PatientMood.ExitOk:
							case PatientMood.ExitNotOK:
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

			if (mood.mood == PatientMood.ExitOk || mood.mood == PatientMood.ExitNotOK) {
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
				dialogSelect.AddButton("gameover", () => {
					currPatientId = 0;
					StartNewPatient();
				});
			});
		}
		else {
			dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Розмова закiнчена", onShowLog: ()=> {
				dialogSelect.AddButton("end", () => {
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
