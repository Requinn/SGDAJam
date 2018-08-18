using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrigger : MonoBehaviour {

	private Transform cannonBall;

	// Use this for initialization
	void Start() {
		cannonBall = transform.GetChild(0);
		cannonBall.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.name == "Player") {
			cannonBall.gameObject.SetActive(true);
		}
	}
}
