using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the background of the environment
/// </summary>
public class BackgroundController : MonoBehaviour {
    //DragonClass reference
    [SerializeField]
    private GameObject _backgroundObject; //object to move
    private Vector3 _homePosition; //fake const value for the home position
    private Vector3 _currentVelocity = Vector3.zero; //the current movement

    private Vector3 _currentRotational; //the current rotational movement being added
    private bool canMove = false;

    public static BackgroundController BGInstance;

    [Header("TEST PROPERTIES")]
    public Vector3 velocity;
    public float rotation;

	// Use this for initialization
	void Start () {
        _homePosition = _backgroundObject.transform.position;
	}

    /// <summary>
    /// Tell the background to move in a di
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    public void ModifyScrollVelocty(Vector3 direction, float speed) {
        _currentVelocity += direction * speed;
    }

    /// <summary>
    /// Tell the background to rotate
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="speed"> how many degrees per frame? </param>
    public void AddRotation(float rotation, float speed){
        _currentRotational += new Vector3(0, 0, rotation);
    }

    /// <summary>
    /// reset the scrolling speed to default
    /// </summary>
    public void ResetScrollVelocity() {
        _currentVelocity = Vector3.zero;
    }

    /// <summary>
    /// reset the rotation speed to default
    /// </summary>
    public void ResetRotationVelocity() {
        _currentRotational = Vector3.zero;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            ModifyScrollVelocty(velocity, 1);
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            AddRotation(rotation, 1);
        }
        //lerp the movement
        _backgroundObject.transform.position = Vector3.Lerp(_backgroundObject.transform.position, _backgroundObject.transform.position + _currentVelocity, Time.deltaTime);
        //lerp the rotation and convert back to quaternion
        _backgroundObject.transform.rotation = Quaternion.Euler(Vector3.Lerp(_backgroundObject.transform.rotation.eulerAngles, _backgroundObject.transform.rotation.eulerAngles + _currentRotational, Time.deltaTime));
    }

    
}
