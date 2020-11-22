using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Subtegral.DialogueSystem.DataContainers;

public class Level : MonoBehaviour {
	[Header("Music"), Space]
	[SerializeField] AudioClip defaultAmbient;
	[SerializeField] AudioClip winAmbient;
	[SerializeField] AudioClip loseAmbient;

	[Header("Sounds"), Space]
	[SerializeField] AudioClip patientWriteSound;
	[SerializeField] AudioClip systemMessageSound;
	[SerializeField] AudioClip callSound;
	[SerializeField] AudioClip buttonAppear;
	[SerializeField] float buttonAppearVolume = 0.5f;

	[Header("Data"), Space]
	[ReorderableList] [SerializeField] PatientData[] patients;
	[SerializeField] string operatorName = "Operator";

	[Header("Refs"), Space]
	[SerializeField] DialogLogUI dialogLog;
	[SerializeField] DialogSelectUI dialogSelect;
	[SerializeField] ManualUI manual;

	int currPatientId = 0;
	PatientData currPatient;
	AudioSource winAmbientAS;

	void Start() {
		dialogSelect.ClearForce();
		dialogLog.ClearLog();

		AudioManager.Instance.PlayMusic(defaultAmbient);
		LeanTween.delayedCall(gameObject, 1.5f, StartNewPatient);
	}

	void StartNewPatient() {
		//dialogLog.ClearLog();
		dialogSelect.Clear();

		currPatient = Instantiate(patients[currPatientId]);

		AudioSource callAS = AudioManager.Instance.PlayLoop(callSound, 0.15f);
		AudioManager.Instance.Play(systemMessageSound);
		dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Call with <i>{currPatient.name}</i> started", onShowLog: ()=> {
			
		});

		dialogSelect.AddButton("start", () => {
			AudioManager.Instance.ChangeASVolume(callAS, 0.0f, 0.25f);
			Destroy(callAS.gameObject, 1.0f);

			dialogSelect.Clear();
			NodeLinkData narrativeData = currPatient.dialogue.NodeLinks.First(); //Entrypoint node
			ProceedToNarrative(narrativeData.TargetNodeGUID);
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
			winAmbientAS = AudioManager.Instance.Play(winAmbient, 0.4f, 1.0f, AudioManager.Instance.crossfadeTime);
			LeanTween.delayedCall(winAmbient.length - AudioManager.Instance.crossfadeTime, AudioManager.Instance.ContinueMusicAfterMute);
		}

		dialogLog.AddToLog(DialogLogUI.LogEntryType.Patient, ProcessProperties(text), currPatient.name, mood.backColor, currPatient, mood, ()=> { 
			AudioManager.Instance.Play(patientWriteSound, 0.5f);
		}, () => {
			AudioManager.Instance.Play(buttonAppear, buttonAppearVolume);

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

		AudioManager.Instance.Play(systemMessageSound);
		dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Call ended", onShowLog: () => {
			AudioManager.Instance.Play(buttonAppear, buttonAppearVolume);
			dialogSelect.AddButton("end", () => {

				++currPatientId;
				if (currPatientId == patients.Length) {
					dialogSelect.Clear();
					AudioManager.Instance.Play(systemMessageSound);
					dialogLog.AddToLog(DialogLogUI.LogEntryType.Servise, $"Thank you for playing the demo!", onShowLog: () => {
						AudioManager.Instance.Play(buttonAppear, buttonAppearVolume);
						dialogSelect.AddButton("gameover", () => {
							currPatientId = 0;
							dialogLog.ClearLog();
							dialogSelect.Clear();
							if (winAmbientAS) 
								AudioManager.Instance.ChangeASVolume(winAmbientAS, 0.0f, 1.0f);
							AudioManager.Instance.PlayMusic(defaultAmbient);
							LeanTween.delayedCall(UnityEngine.Random.Range(1.0f, 2.5f), StartNewPatient);
						});
					});
				}
				else {
					dialogSelect.Clear();
					if (winAmbientAS) 
						AudioManager.Instance.ChangeASVolume(winAmbientAS, 0.0f, 1.0f);
					AudioManager.Instance.PlayMusic(defaultAmbient);
					LeanTween.delayedCall(UnityEngine.Random.Range(1.0f, 2.5f), StartNewPatient);
				}

			});
		});
	}

	string ProcessProperties(string processedText) {
		foreach (var exposedProperty in currPatient.dialogue.ExposedProperties)
			processedText = processedText.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
		processedText = processedText.Replace("\\n", "\n");
		return processedText;
	}
}
