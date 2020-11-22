using UnityEngine;
using UnityEngine.UI;

public class PlayOnClick : MonoBehaviour {
	[SerializeField] AudioClip clip; 
	[SerializeField] Button button;
	[SerializeField] float volume = 1.0f;

#if UNITY_EDITOR
	private void OnValidate() {
		if (button == null)
			button = GetComponent<Button>();
	}
#endif

	private void Awake() {
		if(button != null) {
			button.onClick.AddListener(OnClick);
		}
	}

	private void OnDestroy() {
		if (button != null) {
			button.onClick.RemoveListener(OnClick);
		}
	}

	void OnClick() {
		AudioManager.Instance.Play(clip, volume, channel: AudioManager.AudioChannel.Sound);
	}
}
