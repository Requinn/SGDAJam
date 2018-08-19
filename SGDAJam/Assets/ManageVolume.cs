using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageVolume : MonoBehaviour {

	public AudioSource audio;
	public ChangeImage ci;
	public Slider volumeSlider;

	// Use this for initialization
	void Start () {
		volumeSlider = GetComponent<Slider>();
		volumeSlider.minValue = 0f;
		volumeSlider.maxValue = 1f;
		volumeSlider.value = 1f;

		PlayerPrefs.SetFloat("volume", volumeSlider.value);
	}

	public void VolumeChange() {
		audio.volume = volumeSlider.value;
		PlayerPrefs.SetFloat("volume", volumeSlider.value);
	}
}
