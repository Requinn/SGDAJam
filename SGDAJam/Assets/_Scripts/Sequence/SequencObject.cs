using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequencObject : MonoBehaviour {
    public float sequenceDuration;
    public abstract void PlaySequenceObject();
}
