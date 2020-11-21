using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogUI : MonoBehaviour {
	[Header("Prefabs"), Space]
	[SerializeField] GameObject operatorLogPrefab;
	[SerializeField] GameObject patientLogPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;
	[SerializeField] ScrollRect scroll;

	List<DialogLogEntry> entries = new List<DialogLogEntry>();

	public void AddToLog(bool isOperator, string name, string text, Sprite avatar = null) {
		GameObject entryGO = Instantiate(isOperator ? operatorLogPrefab : patientLogPrefab, layoutGroup.transform);
		DialogLogEntry entry = entryGO.GetComponent<DialogLogEntry>();

		entry.Init(name, text, avatar);

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
