using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Menu : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] DialogLogUI dialogLog;
	[SerializeField] DialogSelectUI dialogSelect;
	[SerializeField] ManualUI manual;
	[Space]
	[SerializeField] Level level;


	void Start() {
		level.OnLevelEndEvent += OnGameEnd;

		StartGame();
	}

	private void OnDestroy() {
		level.OnLevelEndEvent -= OnGameEnd;
	}

	void StartGame() {
		level.StartGame();
	}

	void OnGameEnd() {

	}
}
