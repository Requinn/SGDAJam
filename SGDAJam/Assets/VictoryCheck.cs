using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryCheck : MonoBehaviour {

	public TMP_Text gameOver, con;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetFloat("victory") == 1) {
			gameOver.text = "You Slayed The Dragon!";
			con.text = "Go Again?";
		} 
	}
}
