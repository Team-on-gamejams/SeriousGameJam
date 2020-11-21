using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patient", menuName = "Data/Patient Data")]
public class PatientData : ScriptableObject {
	[SerializeField] PatientMoodData[] moodData;
	[Space]
	public PatientMood mood = PatientMood.Normal;
	public string name;

	public PatientMoodData GetCurrentMoodData() {
		foreach (var data in moodData)
			if (data.mood == mood)
				return data;
		return moodData[0];
	}

	public enum PatientMood : byte { 
		Normal = 0,
		Angry = 1,
	}

	[Serializable]
	public struct PatientMoodData{
		public PatientMood mood;
		public Sprite avatar;
	}
}
