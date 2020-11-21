using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSelectUI : MonoBehaviour {
	[Header("Prefabs"), Space]
	[SerializeField] GameObject buttonPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;
	[SerializeField] ContentSizeFitter contentSizeFitter;

	List<DialogSelectButton> buttons = new List<DialogSelectButton>();

	public void AddButton(string text, Action onClick) {
		GameObject buttonGO = Instantiate(buttonPrefab, layoutGroup.transform);
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
