using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSelectButton : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] Button btn;
	KeyCode key;
	KeyCode keyAlt;

	public void Init(int id, string text, params Action[] onClicks) {
		textField.text = $"{id}) {text}";

		foreach (var click in onClicks) {
			btn.onClick.AddListener(() => click?.Invoke()) ;
		}

		switch (id) {
			case 1:
				key = KeyCode.Alpha1;
				keyAlt = KeyCode.Keypad1;
				break;
			case 2:
				key = KeyCode.Alpha2;
				keyAlt = KeyCode.Keypad2;
				break;
			case 3:
				key = KeyCode.Alpha3;
				keyAlt = KeyCode.Keypad3;
				break;
			case 4:
				key = KeyCode.Alpha4;
				keyAlt = KeyCode.Keypad4;
				break;
			case 5:
				key = KeyCode.Alpha5;
				keyAlt = KeyCode.Keypad5;
				break;
			case 6:
				key = KeyCode.Alpha6;
				keyAlt = KeyCode.Keypad6;
				break;
			case 7:
				key = KeyCode.Alpha7;
				keyAlt = KeyCode.Keypad7;
				break;
			case 8:
				key = KeyCode.Alpha8;
				keyAlt = KeyCode.Keypad8;
				break;
			case 9:
				key = KeyCode.Alpha9;
				keyAlt = KeyCode.Keypad9;
				break;
			case 0:
				key = KeyCode.Alpha0;
				keyAlt = KeyCode.Keypad0;
				break;
		}
	}

	private void Update() {
		if (Input.GetKeyDown(key) || Input.GetKeyDown(keyAlt)) {
			btn.onClick?.Invoke();
			key = KeyCode.None;
		}
	}

	private void OnDestroy() {
		btn.onClick.RemoveAllListeners();
	}
}
