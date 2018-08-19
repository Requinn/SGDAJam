using MichaelWolfGames;
using MichaelWolfGames.CC2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePlayer : MonoBehaviour {
    public SequencObject[] steps;
    private CharacterController2D _playerRef;
    int _playedIndex = 0; 

	void Start () {
        _playerRef = PlayerInstance.Instance.GetComponent<CharacterController2D>();
        StartCoroutine(PlaySequence());
	}

    private IEnumerator PlaySequence() {
        //suspend all input
        _playerRef.Suspended = true;
        //play every sequence object
        while (_playedIndex < steps.Length) {
            steps[_playedIndex].PlaySequenceObject();
            yield return new WaitForSeconds(steps[_playedIndex].sequenceDuration);
            _playedIndex++;
        }
        //return control to the players
        _playerRef.Suspended = false;
    }
}
