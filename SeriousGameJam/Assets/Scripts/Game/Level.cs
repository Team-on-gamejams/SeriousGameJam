using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] PatientData[] patients;

	[Header("Refs"), Space]
	[SerializeField] DialogLogUI dialogLog;
	[SerializeField] DialogSelectUI dialogSelect;
	[SerializeField] ManualUI manual;

	int currPatientId = 0;
	PatientData currPatient;

	private void Awake() {
		dialogSelect.OnAnyButtonPress += OnAnyButtonPress;
	}

	void Start() {
		StartNewPatient();
	}

	private void OnDestroy() {
		dialogSelect.OnAnyButtonPress -= OnAnyButtonPress;
	}

	void StartNewPatient() {
		dialogLog.ClearLog();
		dialogSelect.Clear();

		currPatient = Instantiate(patients[currPatientId]);

		PatientData.PatientMoodData mood = currPatient.GetCurrentMoodData();
		dialogLog.AddToLog(false, currPatient.name, "Hi!", mood.avatar);
		FillSelectDialog();
	}

	void MakeAngry() {
		dialogLog.AddToLog(true, "Operator", "Gonna make you <b>angry</b>");
		
		currPatient.mood = PatientData.PatientMood.Angry;
	}

	void MakeNormal() {
		dialogLog.AddToLog(true, "Operator", "Gonna make you <b>normal</b>");
		
		currPatient.mood = PatientData.PatientMood.Normal;
	}

	void EndPatient() {
		dialogLog.AddToLog(true, "Operator", "END");
		
		LeanTween.delayedCall(1.0f, () => {
			++currPatientId;
			if(currPatientId == patients.Length) {
				dialogLog.ClearLog();
				dialogSelect.Clear();
				dialogLog.AddToLog(true, "Operator", "You win");
			}
			else {
				StartNewPatient();
			}
		});
	}

	void OnAnyButtonPress() {
		dialogSelect.Clear();

		PatientData.PatientMoodData mood = currPatient.GetCurrentMoodData();

		dialogLog.AddToLog(false, currPatient.name, "Text", mood.avatar);

		FillSelectDialog();
	}

	void FillSelectDialog() {
		dialogSelect.AddButton("Make angry", MakeAngry);
		dialogSelect.AddButton("Make normal", MakeNormal);
		dialogSelect.AddButton("Start new patient", EndPatient);
	}
}
