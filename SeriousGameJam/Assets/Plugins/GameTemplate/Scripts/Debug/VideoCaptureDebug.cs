using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class VideoCaptureDebug : MonoBehaviour {
	string savePath => "";

	[Header("Data")]
	[Space]
	[SerializeField] bool isOfflineRenderer = true;

	[Header("Video keys")]
	[Space]
	[SerializeField] KeyCode startVideoKey = KeyCode.F1;
	[SerializeField] KeyCode pauseVideoKey = KeyCode.F2;
	[SerializeField] KeyCode stopVideoKey = KeyCode.F3;
	[SerializeField] KeyCode openVideoFolderKey = KeyCode.F4;

	[Header("UI")]
	[Space]
	[SerializeField] Image recordingImg;
	[SerializeField] TextMeshProUGUI recordingText;

	[Header("Refs")]
	[Space]
	

	bool isProcessFinish;

#if UNITY_EDITOR
	private void OnValidate() {
	}
#endif

	void Update() {
		
	}

	void ShowExplorer(string itemPath) {
		itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
		System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
	}
}
