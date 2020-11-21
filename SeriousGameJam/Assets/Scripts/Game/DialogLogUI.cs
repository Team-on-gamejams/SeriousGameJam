using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogUI : MonoBehaviour {
	public enum LogEntryType : byte { Patient, Operator, Servise }

	[Header("Style"), Space]
	[SerializeField] Color operatorColor;
	[SerializeField] Color serviceColor;

	[Header("Prefabs"), Space]
	[SerializeField] GameObject operatorLogPrefab;
	[SerializeField] GameObject patientLogPrefab;
	[SerializeField] GameObject serviceLogPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;
	[SerializeField] ScrollRect scroll;

	List<DialogLogEntry> entries = new List<DialogLogEntry>();

	public void AddToLog(LogEntryType type, string text, string name = "", Color backColor = default, Sprite avatar = null) {
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

		entry.Init(name, text, avatar,c);

		entries.Add(entry);

		StartCoroutine(ScrollToBottom());
	}

	public void ClearLog() {
		foreach (Transform child in layoutGroup.transform)
			Destroy(child.gameObject);
		entries.Clear();
	}

	public void OnLogScroll() {
		//LeanTween.cancel(scroll.gameObject, false);
	}

	IEnumerator ScrollToBottom() {
		yield return null;
		yield return null;
		LeanTween.value(scroll.gameObject, scroll.normalizedPosition, Vector2.zero, 0.5f)
		.setEase(LeanTweenType.easeOutCirc)
		.setOnUpdate((Vector2 pos) => {
			scroll.normalizedPosition = pos;
		});
	}
}
