using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingDragon : SequencObject {
    public GameObject dragonObject;
    public Vector3 startPos;
    public Vector3 endPos;

    public override void PlaySequenceObject() {
        StartCoroutine(RaiseTheDragon());
    }

    /// <summary>
    /// Now I gotta train it....
    /// </summary>
    /// <returns></returns>
    private IEnumerator RaiseTheDragon() {
        while(dragonObject.transform.position.y < endPos.y) {
            dragonObject.transform.position = Vector3.Lerp(dragonObject.transform.position, dragonObject.transform.position -= dragonObject.transform.position * .30f, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
