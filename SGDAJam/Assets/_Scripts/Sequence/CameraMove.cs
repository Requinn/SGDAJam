using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : SequencObject {
    public Cinemachine.CinemachineVirtualCamera vCam;
    public Cinemachine.CinemachineVirtualCamera vCam2;

    public override void PlaySequenceObject() {
        vCam2.Priority = 0;
        vCam.Priority = 11;
    }
}
