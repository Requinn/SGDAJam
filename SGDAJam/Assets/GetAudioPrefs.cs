using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioPrefs : MonoBehaviour {

	AudioSource audio;
	public float volumeModifier = 1f;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audio.volume = PlayerPrefs.GetFloat("volume") * volumeModifier;

		if(PlayerPrefs.GetFloat("mute") == 0f) {
			audio.mute = false;
		} else {
			audio.mute = true;
		}
	}
}
