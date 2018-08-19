using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePlayer : MonoBehaviour {
    public SequencObject[] steps;
    int _playedIndex = 0; 
	void Start () {
        StartCoroutine(PlaySequence());
	}

    private IEnumerator PlaySequence() {
        while (_playedIndex < steps.Length) {
            steps[_playedIndex].PlaySequenceObject();
            yield return new WaitForSeconds(steps[_playedIndex].sequenceDuration);
            _playedIndex++;
        }

    }
}
