using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour {

	AudioSource audio;
	public AudioClip looper;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audio.PlayScheduled(AudioSettings.dspTime + audio.clip.length - audio.time);
	}
}
