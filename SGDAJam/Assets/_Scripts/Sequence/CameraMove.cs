using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : SequencObject {
    public Cinemachine.CinemachineVirtualCamera vCam;
    public Cinemachine.CinemachineVirtualCamera vCam2;
    public float orthographSizeStart; //starting value of camera zoom
    public float orthographSizeEnd; //ending value of camera zoom

    public override void PlaySequenceObject() {
        //StartCoroutine(MoveAndAdjustCamera());
        vCam2.Priority = 0;
        vCam.Priority = 11;
    }

    private IEnumerator MoveAndAdjustCamera() {
        while (vCam.m_Lens.OrthographicSize > orthographSizeEnd) {
            //vCam.m_Lens.OrthographicSize = Mathf.Lerp(vCam.m_Lens.OrthographicSize, vCam.m_Lens.OrthographicSize -= vCam.m_Lens.OrthographicSize * 0.45f, Time.deltaTime);
            yield return new WaitForFixedUpdate();
            Debug.Log(vCam.m_Lens.OrthographicSize);
        }
        vCam.m_Lens.OrthographicSize = orthographSizeEnd;
    }

    // Use this for initialization
    void Start () {
        //vCam.m_Lens.OrthographicSize = orthographSizeStart;
        //PlaySequenceObject();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
