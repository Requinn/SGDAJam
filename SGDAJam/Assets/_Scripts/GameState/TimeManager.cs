using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    public RectTransform pauseScreen;

    private enum TimeState
    {
        Normal,
        Paused
    }

    private TimeState currentState;
    private float previousTimescale;

    public void Start()
    {
        currentState = TimeState.Normal;
        if(pauseScreen == null)
        {
            Debug.LogError("TimeManager requires a reference to the Pause Screen!");
        }
    }

    public void Update()
    {
        switch (currentState)
        {
            case (TimeState.Normal):
                if(GameSceneManager.Instance.currentState == GameSceneManager.ViewState.InGame &&
                    Input.GetButtonDown("Escape"))
                {
                    Pause();
                }
                break;
            case (TimeState.Paused):
                if(Input.GetButtonDown("Escape"))
                {
                    Resume();
                }
                break;
        }
    }

    public void Pause()
    {
        if (currentState != TimeState.Paused)
        {
            //Freeze Timescale and make note of previous scale
            //Assumes nothing else has control over time
            previousTimescale = Time.timeScale;
            Time.timeScale = 0.0f;

            if(pauseScreen) pauseScreen.gameObject.SetActive(true);
            this.currentState = TimeState.Paused;
        }
    }

    //Close
    public void Resume()
    {
        if (currentState == TimeState.Paused)
        {
            Time.timeScale = previousTimescale;
            if(pauseScreen) pauseScreen.gameObject.SetActive(false);
            this.currentState = TimeState.Normal;
        }
    }

}
