using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSelectUI : MonoBehaviour {
	[Header("Prefabs"), Space]
	[SerializeField] GameObject buttonPrefab;
	[SerializeField] GameObject buttonStartPrefab;
	[SerializeField] GameObject buttonEndPrefab;
	[SerializeField] GameObject buttonGameOverPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;
	[SerializeField] ContentSizeFitter contentSizeFitter;

	List<DialogSelectButton> buttons = new List<DialogSelectButton>();

	public void AddButton(string text, Action onClick) {
		GameObject buttonGO;

		switch (text.Trim().ToLower()) {
			case "start":
				buttonGO = Instantiate(buttonStartPrefab, layoutGroup.transform);
				break;

			case "end":
				buttonGO = Instantiate(buttonEndPrefab, layoutGroup.transform);
				break;

			case "gameover":
				buttonGO = Instantiate(buttonGameOverPrefab, layoutGroup.transform);
				break;

			default:
				buttonGO = Instantiate(buttonPrefab, layoutGroup.transform);
				break;
		}
		DialogSelectButton button = buttonGO.GetComponent<DialogSelectButton>();

		button.Init(buttons.Count + 1, text, onClick);
		button.Show();

		buttons.Add(button);

		contentSizeFitter.enabled = false;
		contentSizeFitter.SetLayoutVertical();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)button.transform);
		contentSizeFitter.enabled = true;
	}

	public void Clear() {
		foreach (var button in buttons) {
			button.transform.SetParent(transform.parent, true);
			button.Hide();
		}

		buttons.Clear();
	}

	public void ClearForce() {
		foreach (Transform child in layoutGroup.transform) 
			Destroy(child.gameObject);
		buttons.Clear();
	}
}
