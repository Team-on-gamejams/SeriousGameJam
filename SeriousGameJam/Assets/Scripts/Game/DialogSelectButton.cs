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

	public void Init(int id, string text, params Action[] onClicks) {
		textField.text = $"{id}) {text}";

		foreach (var click in onClicks) {
			btn.onClick.AddListener(() => click?.Invoke()) ;
		}
	}

	private void OnDestroy() {
		btn.onClick.RemoveAllListeners();
	}
}
