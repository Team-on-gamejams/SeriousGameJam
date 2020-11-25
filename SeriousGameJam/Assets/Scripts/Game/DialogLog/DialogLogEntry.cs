using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class DialogLogEntry : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI nameTextField;
	[SerializeField] TextMeshProUGUI textTextField;
	[SerializeField] Image back;
	[Space]
	[SerializeField] Image avatar;
	[Space]
	[SerializeField] RectTransform spineParent;
	[SerializeField] SkeletonGraphic spineSkeleton;

	public void Init(string name, string text, Sprite _avatar, Color backColor) {
		if(!string.IsNullOrEmpty(name) && nameTextField)
			nameTextField.text = name;

		textTextField.text = text;

		if (avatar) {
			avatar.sprite = _avatar;
			avatar.gameObject.SetActive(true);
		}

		back.color = backColor;

		if (spineParent) 
			spineParent.gameObject.SetActive(false);
	}

	public void Init(string name, string text, SkeletonDataAsset skeletonDataAsset, AnimationReferenceAsset animation, Color backColor) {
		if (!string.IsNullOrEmpty(name) && nameTextField)
			nameTextField.text = name;

		textTextField.text = text;

		if (avatar) 
			avatar.gameObject?.SetActive(false);

		back.color = backColor;

		spineParent.gameObject.SetActive(true);
		spineSkeleton.Clear();
		spineSkeleton.skeletonDataAsset = skeletonDataAsset;
		spineSkeleton.Initialize(true);
		spineSkeleton.AnimationState.SetAnimation(0, animation, true);
	}

	public void OnBecomeOld() {
		if(spineSkeleton)
			spineSkeleton.AnimationState.TimeScale = 0;
	}
}
