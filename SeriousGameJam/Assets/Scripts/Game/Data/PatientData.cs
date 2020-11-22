using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Subtegral.DialogueSystem.DataContainers;

[CreateAssetMenu(fileName = "Patient", menuName = "Data/Patient Data")]
public class PatientData : ScriptableObject {
	[SerializeField] PatientMoodData[] moodData;
	[Space]
	public string name;
	public DialogueContainer dialogue;
	public SkeletonDataAsset skeletonDataAsset;
	PatientMood mood = PatientMood.Normal;

	public PatientMoodData GetMoodData(string str) {
		switch (str.ToLower().Trim()) {
			case "normal":
				mood = PatientMood.Normal;
				break;
			case "angry":
				mood = PatientMood.Angry;
				break;
			case "doubts":
			case "doubt":
				mood = PatientMood.Doubt;
				break;
			case "no":
				mood = PatientMood.No;
				break;
			case "exit":
			case "exitok":
				mood = PatientMood.ExitOk;
				break;
			case "exitno":
			case "exitnot":
			case "exitnook":
			case "exitnotok":
				mood = PatientMood.ExitNotOK;
				break;
		}

		for(int i = 0; i < moodData.Length; ++i) {
			if (moodData[i].mood == mood) {
				if(moodData[i].avatar == null) {
					moodData[i].avatar = moodData[0].avatar;
				}
				if (moodData[i].backColor == Color.clear) {
					moodData[i].backColor = moodData[0].backColor;
				}
				return moodData[i];
			}
		}
			
		return moodData[0];
	}

	[Serializable]
	public struct PatientMoodData{
		public PatientMood mood;
		public Color backColor;
		[Space]
		public Sprite avatar;
		[Space]
		public AnimationReferenceAsset animationSpine;
	}
}
