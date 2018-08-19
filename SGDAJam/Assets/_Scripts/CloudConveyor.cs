using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moves a cloud from one spot too the other
/// </summary>
public class CloudConveyor : MonoBehaviour {
    private float _farLeftX = -65f;
    private float _farRightX = 180;
    private float _movementSpeedMin = 0.001f;
    private float _movementSpeedMax = 0.05f;
    private float _calcedMoveSpeed; 

    private Transform _transform;

    void Start() {
        _transform = gameObject.transform;
        _calcedMoveSpeed = Random.Range(_movementSpeedMin, _movementSpeedMax);
    }


    void Update() {
        _transform.Translate(-_calcedMoveSpeed, 0, 0);
        if(_transform.position.x <= _farLeftX) {
            _transform.position = new Vector2(_farRightX, _transform.position.y);
        }
    }
}
