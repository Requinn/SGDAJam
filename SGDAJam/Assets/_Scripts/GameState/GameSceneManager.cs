using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour {

    public enum GameState
    {
        Menu,
        InGame,
        GameFailure,
        GameVictory,
        VictoryScreen
    }

    public string menuSceneName;
    public string gameSceneName;
    public string failureSceneName;
    public string victorySceneName;

    public GameState currentState;

    private static GameSceneManager _instance;

    public float fadeTime = 2f; //in seconds
    private float startFadeTime = 0;
    public Color victoryFadeColor = Color.white;
    public Color failureFadeColor = Color.black;

    public static GameSceneManager GetInstance()
    {
        return _instance;
    }

    public void Awake()
    {
        if(_instance == null  )
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //The previous scene already had a scene manager, we need to dissapear
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        switch (currentState)
        {
            case (GameState.GameFailure):
                //The player has failed, fade to black
                {
                    float endFadeTime = startFadeTime + fadeTime;
                    if (Time.fixedTime >= endFadeTime)
                    {

                    }
                    else
                    {
                        //Lerp color
                        FadeWithLerp(Mathf.InverseLerp(startFadeTime, endFadeTime, Time.fixedTime));
                    }

                    break;
                }
            case (GameState.GameVictory):
                //The player is victorious, fade to white, then swap to victory scene
                {
                    float endFadeTime = startFadeTime + fadeTime;
                    if (Time.fixedTime >= endFadeTime)
                    {
                        
                    }
                    else
                    {
                        //Lerp color
                        FadeWithLerp(Mathf.InverseLerp(startFadeTime, endFadeTime, Time.fixedTime));
                    }
                    break;
                }
        }

    }

    public bool FailGame()
    {
        //User has failed, fade to black and reload main scene
        if (currentState == GameState.InGame)
        {
            currentState = GameState.GameFailure;
            startFadeTime = Time.fixedTime;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool WinGame()
    {
        //End the game after 2 seconds, performing a fade to color
        if (currentState == GameState.InGame)
        {
            currentState = GameState.GameVictory;
            startFadeTime = Time.fixedTime;
            return true;
        }
        else
        {
            return false;
        }

    }

#region Fade Controls
    private void ResetFade()
    {
        
    }

    private void FadeWithLerp(float lerp)
    {
        
    }

    private void SetFadeColor(Color fadeColor)
    {

    }
#endregion

}
