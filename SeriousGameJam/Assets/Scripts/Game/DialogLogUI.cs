using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogUI : MonoBehaviour {
	[Header("Style"), Space]
	[SerializeField] Color operatorColor;

	[Header("Prefabs"), Space]
	[SerializeField] GameObject operatorLogPrefab;
	[SerializeField] GameObject patientLogPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;
	[SerializeField] ScrollRect scroll;

	List<DialogLogEntry> entries = new List<DialogLogEntry>();

	public void AddToLog(bool isOperator, string name, string text, Color backColor = default, Sprite avatar = null) {
		GameObject entryGO = Instantiate(isOperator ? operatorLogPrefab : patientLogPrefab, layoutGroup.transform);
		DialogLogEntry entry = entryGO.GetComponent<DialogLogEntry>();

		entry.Init(name, text, avatar, isOperator ? operatorColor : backColor);

		entries.Add(entry);

		StartCoroutine(ScrollToBottom());
	}

	public void ClearLog() {
		foreach (Transform child in layoutGroup.transform)
			Destroy(child.gameObject);
		entries.Clear();
	}

	IEnumerator ScrollToBottom() {
		yield return null;
		scroll.normalizedPosition = new Vector2(0, 0);
	}
}
