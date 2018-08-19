using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbSceneWorkaround : MonoBehaviour {
    //This is bad and I should feel bad
    public void StartGame()
    {
        GameSceneManager.Instance.StartGame();
    }

	public void GameOver() {
		GameSceneManager.Instance.FailGame();
	}

	public void MainMenu() {
		GameSceneManager.Instance.ReturnToMenu();
	}
}
