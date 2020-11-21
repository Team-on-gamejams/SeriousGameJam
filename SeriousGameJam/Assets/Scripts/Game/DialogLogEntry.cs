using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogLogEntry : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI nameTextField;
	[SerializeField] TextMeshProUGUI textTextField;
	[SerializeField] Image avatar;
	[SerializeField] Image back;

	public void Init(string name, string text, Sprite _avatar, Color backColor) {
		nameTextField.text = name;

		textTextField.text = text;

		if(avatar)
			avatar.sprite = _avatar;

		back.color = backColor;
	}
}
