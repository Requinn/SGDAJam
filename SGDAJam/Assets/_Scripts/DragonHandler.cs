using MichaelWolfGames.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragonHandler : MonoBehaviour {
    [SerializeField]
    HealthManagerBase[] _damageableNode;
    private int _nodesToHit;
    private int _nodesKilled;
	// Use this for initialization

	public DumbSceneWorkaround dsw;
	public GameObject dragon;
	private bool gameOver, done;
	
    void Start() {
        _nodesToHit = _damageableNode.Length;
        //subscribe to all the damageablenodes
        for (int i = 0; i < _nodesToHit; i++) {
            _damageableNode[i].OnTakeDamage += ReactToDamage;
            _damageableNode[i].OnDeath += ReactToKill;
        }

		gameOver = done = false;
	}

	private void Update() {
		if(gameOver) {
			if (dragon.transform.position.y > -100f) {
				dragon.transform.position += Vector3.down * 15f * Time.deltaTime;
				done = false;
			} else {
				done = true;
			}

			if (done) {
				PlayerPrefs.SetFloat("victory", 1f);
				PlayerPrefs.Save();
				dsw.GameOver();
			}
		}
		
	}

	/// <summary>
	/// To handle the critical node dying
	/// </summary>
	private void ReactToKill() {
        Debug.Log("Node died");
        _nodesKilled++;

        CheckForGameOver();
    }

    /// <summary>
    /// How will we handle the damage being dealt to us?
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ReactToDamage(object sender, Damage.DamageEventArgs args) {
        Debug.Log("Damaged with severity: " + args.DamageValue);
        //when struck, speed up the animation of the dragon by X amount for some seconds
    }

    /// <summary>
    /// Check if the game is finished every time we kill a node
    /// </summary>
    private void CheckForGameOver() {
        if (_nodesKilled == _nodesToHit) {
			//End the game

			gameOver = true;
			Debug.Log("GAME ENDED");
        } 
    }
}
