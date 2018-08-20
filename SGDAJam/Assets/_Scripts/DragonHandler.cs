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
    public AudioClip deathRoar;
    public AudioClip hurtRoar;
    public AudioSource Audio;
	public DumbSceneWorkaround dsw;
	public GameObject dragon;
	private bool gameOver, done;
    public float fallSpeed = 20f;
    public Transform dragonRoot;
    public int nodesToStartRot = 1;
    public float rotTime = 5f;
    public float rotGoal = 60f;
    public GameObject airShip;
    public float shipDissapearTime = 5f;
    private bool startedRot = false;
	
    void Start() {
        _nodesToHit = _damageableNode.Length;
        //subscribe to all the damageablenodes
        for (int i = 0; i < _nodesToHit; i++) {
            _damageableNode[i].OnTakeDamage += ReactToDamage;
            _damageableNode[i].OnDeath += ReactToKill;
        }
        Audio = GetComponent<AudioSource>();
		gameOver = done = false;
	}

	private void Update() {
		if(gameOver) {
			PlayerPrefs.SetFloat("victory", 1f);
			PlayerPrefs.Save();
			if (dragon.transform.position.y > -100f) {
				dragon.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
				done = false;
			} else {
				done = true;
			}

			if (done) {
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

	    if (_nodesKilled == nodesToStartRot && !startedRot)
	    {
	        startedRot = true;
	        StartCoroutine(CoRotateUp(rotTime));
	    }

        CheckForGameOver();
    }

    /// <summary>
    /// How will we handle the damage being dealt to us?
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ReactToDamage(object sender, Damage.DamageEventArgs args) {
        //Debug.Log("Damaged with severity: " + args.DamageValue);
        //when struck, speed up the animation of the dragon by X amount for some seconds
        Audio.PlayOneShot(hurtRoar);

        if (_nodesKilled == 1 && startedRot)
        {
            StartCoroutine(CoRotateUp(rotTime));
        }
    }

    /// <summary>
    /// Check if the game is finished every time we kill a node
    /// </summary>
    private void CheckForGameOver() {
        if (_nodesKilled == _nodesToHit) {
            //End the game
            Audio.PlayOneShot(deathRoar);
            gameOver = true;
			Debug.Log("GAME ENDED");
        } 
    }

    IEnumerator CoRotateUp(float duration)
    {
        if (dragonRoot == null)
        {
            dragonRoot = transform.parent;
        }

        Quaternion startRot = dragonRoot.localRotation;
        Quaternion endRot = Quaternion.Euler(0f, 0f, rotGoal);
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (timer > shipDissapearTime)
            {
                airShip.SetActive(false);
            }

            dragonRoot.localRotation = Quaternion.Slerp(startRot, endRot, timer / duration);
            yield return null;
        }
    }
}
