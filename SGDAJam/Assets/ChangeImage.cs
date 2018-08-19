using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {

	public AudioSource audio;

	Image sound;
	public Sprite full, mute;

	// Use this for initialization
	void Awake () {
		sound = GetComponent<Image>();
		sound.sprite = full;
		PlayerPrefs.SetFloat("mute", 0f);
	}

	public void SwapImage() {
		if(sound.sprite == mute) {
			sound.sprite = full;
			audio.mute = false;
			PlayerPrefs.SetFloat("mute", 0f);
		} else {
			sound.sprite = mute;
			audio.mute = true;
			PlayerPrefs.SetFloat("mute", 1f);
		}
	}
}
