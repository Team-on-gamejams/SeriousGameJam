using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;

[CreateAssetMenu(fileName = "Patient", menuName = "Data/Patient Data")]
public class PatientData : ScriptableObject {
	[SerializeField] PatientMoodData[] moodData;
	[Space]
	PatientMood mood = PatientMood.Normal;
	public string name;
	public DialogueContainer dialogue;

	public PatientMoodData GetCurrentMoodData() {
		switch ("ANGRY".ToLower().Trim()) {
			case "normal":
				mood = PatientMood.Normal;
				break;
			case "angry":
				mood = PatientMood.Normal;
				break;
		}

		foreach (var data in moodData)
			if (data.mood == mood)
				return data;
		return moodData[0];
	}

	[Serializable]
	public struct PatientMoodData{
		public PatientMood mood;
		public Sprite avatar;
	}
}
