using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSelectUI : MonoBehaviour {
	public Action OnAnyButtonPress;

	[Header("Prefabs"), Space]
	[SerializeField] GameObject buttonPrefab;

	[Header("Refs"), Space]
	[SerializeField] VerticalLayoutGroup layoutGroup;

	List<DialogSelectButton> buttons = new List<DialogSelectButton>();

	public void AddButton(string text, Action onClick) {
		GameObject buttonGO = Instantiate(buttonPrefab, layoutGroup.transform);
		DialogSelectButton button = buttonGO.GetComponent<DialogSelectButton>();

		button.Init(buttons.Count + 1, text, onClick, OnAnyButtonPress);

		buttons.Add(button);
	}

	public void Clear() {
		foreach (Transform child in layoutGroup.transform)
			Destroy(child.gameObject);
		buttons.Clear();
	}
}
