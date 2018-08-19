using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbSceneWorkaround : MonoBehaviour {
    //This is bad and I should feel bad
    public void StartGame(float delay)
    {
        StartCoroutine(Delay(delay));
    }

    private IEnumerator Delay(float delay) {
        yield return new WaitForSeconds(delay);
        GameSceneManager.Instance.StartGame();
    }

	public void GameOver() {
		GameSceneManager.Instance.FailGame();
	}

	public void MainMenu() {
		GameSceneManager.Instance.ReturnToMenu();
	}
}
