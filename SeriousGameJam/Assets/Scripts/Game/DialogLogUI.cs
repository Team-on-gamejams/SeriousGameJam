using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class DialogLogUI : MonoBehaviour {
	public enum LogEntryType : byte { Patient, Operator, Servise }

	[Header("Show animation"), Space]
	[SerializeField] [MinMaxSlider(0, 10, false)] Vector2 dotsAnimLenghtRange = new Vector2(1, 2);

	[Header("Style"), Space]
	[SerializeField] Color operatorColor;
	[SerializeField] Color serviceColor;

	[Header("Prefabs"), Space]
	[SerializeField] GameObject operatorLogPrefab;
	[SerializeField] GameObject patientLogPrefab;
	[SerializeField] GameObject serviceLogPrefab;
	[SerializeField] GameObject patientTypingPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;
	[SerializeField] ScrollRect scroll;
	[SerializeField] ContentSizeFitter contentSizeFitter;

	List<DialogLogEntry> entries = new List<DialogLogEntry>();
	PatientTypingEntry patientTyping;

	public void AddToLog(LogEntryType type, string text, string name = "", Color backColor = default, PatientData patientData = null, PatientData.PatientMoodData moodData = default, Action onShowLog = null, float minTimeToCallShow = 0.0f) {
		if (type == LogEntryType.Patient) {
			GameObject typingGO = Instantiate(patientTypingPrefab, layoutGroup.transform);
			patientTyping = typingGO.GetComponent<PatientTypingEntry>();
			patientTyping.Init(patientData != null ? patientData.GetMoodData("normal").backColor : backColor);

			StartCoroutine(ScrollToBottom(null, 0.0f));

			float delay = dotsAnimLenghtRange.GetRandomValueFloat();
			LeanTween.delayedCall(delay, ()=> {
				Destroy(patientTyping.gameObject);
				AddToLogImmediatly();
			});
		}
		else {
			AddToLogImmediatly();
		}


		void AddToLogImmediatly() {
			GameObject entryGO;
			Color c;

			switch (type) {
				case LogEntryType.Operator:
					entryGO = Instantiate(operatorLogPrefab, layoutGroup.transform);
					c = operatorColor;
					break;
				case LogEntryType.Servise:
					entryGO = Instantiate(serviceLogPrefab, layoutGroup.transform);
					c = serviceColor;
					break;

				case LogEntryType.Patient:
				default:
					entryGO = Instantiate(patientLogPrefab, layoutGroup.transform);
					c = backColor;
					break;
			}

			DialogLogEntry entry = entryGO.GetComponent<DialogLogEntry>();

			if (patientData != null && patientData.skeletonDataAsset != null && moodData.animationSpine != null) {
				entry.Init(name, text, patientData.skeletonDataAsset, moodData.animationSpine, c);
			}
			else {
				entry.Init(name, text, moodData.avatar, c);
			}

			if(type != LogEntryType.Servise) {
				for(int i = 0; i < entries.Count; ++i)
					entries[i].OnBecomeOld();
			}
			entries.Add(entry);

			contentSizeFitter.enabled = false;
			contentSizeFitter.SetLayoutVertical();
			foreach (Transform child in entryGO.transform) {
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)child.transform);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)entryGO.transform);
			contentSizeFitter.enabled = true;

			StartCoroutine(ScrollToBottom(onShowLog, minTimeToCallShow));
		}
	}

	public void ClearLog() {
		foreach (Transform child in layoutGroup.transform)
			Destroy(child.gameObject);
		entries.Clear();
	}

	public void OnLogScroll() {
		//LeanTween.cancel(scroll.gameObject, false);
	}

	IEnumerator ScrollToBottom(Action onEnd, float minTime) {
		LeanTween.cancel(scroll.gameObject, true);
		
		yield return null;

		GameObject entryGO = entries[entries.Count - 1].gameObject;
		contentSizeFitter.enabled = false;
		contentSizeFitter.SetLayoutVertical();
		foreach (Transform child in entryGO.transform) {
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)child.transform);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)entryGO.transform);
		contentSizeFitter.enabled = true;

		yield return null;

		if (scroll.normalizedPosition == Vector2.zero) {
			LeanTween.delayedCall(minTime, () => {
				onEnd?.Invoke();
			});
		}
		else {
			LeanTween.value(scroll.gameObject, scroll.normalizedPosition, Vector2.zero, 0.5f)
			.setEase(LeanTweenType.easeOutCirc)
			.setOnUpdate((Vector2 pos) => {
				scroll.normalizedPosition = pos;
			})
			.setOnComplete(() => {
				onEnd?.Invoke();
			});
		}
	}
}
