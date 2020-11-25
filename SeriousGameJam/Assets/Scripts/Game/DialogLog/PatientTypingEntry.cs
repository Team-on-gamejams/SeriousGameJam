using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientTypingEntry : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Animator anim;
	[SerializeField] Image back;

	public void Init(Color backColor) {
		back.color = backColor;
	}
}
