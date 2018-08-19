using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour {

    public enum GameState
    {
        None,
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

    public GameState startingState = GameState.None;
    public GameState currentState { get { return _currentState; } }
    private GameState _currentState;

    private static GameSceneManager _instance;

    public float fadeTime = 2f; //in seconds
    private float startFadeTime = 0;
    public Color victoryFadeColor = Color.white;
    public Color failureFadeColor = Color.black;
    public Image fadeLayer;

    public static GameSceneManager Instance
    {
        get
        { 
            return _instance;
        }
    }

    public void Awake()
    {
        if(_instance == null)
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

    public void Start()
    {
        _currentState = startingState;
        DisableFade();
    }

    public void Update()
    {
        switch (_currentState)
        {
            case (GameState.GameFailure):
                //The player has failed, fade to black
                {
                    float endFadeTime = startFadeTime + fadeTime;
                    if (Time.fixedTime >= endFadeTime)
                    {
                        DisableFade();
                        if(!string.IsNullOrEmpty(failureSceneName))
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene(failureSceneName);
                        }
                        else
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
                        }
                        
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
                        DisableFade();
                        UnityEngine.SceneManagement.SceneManager.LoadScene(victorySceneName);
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

    public bool StartGame()
    {
        _currentState = GameState.InGame;
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
        return true;
    }

    public bool FailGame()
    {
        //User has failed, fade to black and reload main scene
        if (_currentState == GameState.InGame)
        {
            _currentState = GameState.GameFailure;
            EnableFade();
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
        if (_currentState == GameState.InGame)
        {
            _currentState = GameState.GameVictory;
            startFadeTime = Time.fixedTime;
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool ReturnToMenu()
    {
        _currentState = GameState.Menu;
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
        return true;
    }

#region Fade Controls
    //Shoulda been seperate
    private void DisableFade()
    {
        fadeLayer.color = new Color(1, 1, 1, 0);
        fadeLayer.gameObject.SetActive(false);
    }

    private void EnableFade()
    {
        startFadeTime = Time.fixedTime;
        fadeLayer.gameObject.SetActive(true);
    }

    private void FadeWithLerp(float lerp)
    {
        var newColor = GetCurrentFadeColor();
        fadeLayer.color = new Color(newColor.r, newColor.g, newColor.b, lerp);
    }

    private Color GetCurrentFadeColor()
    {
        switch (_currentState)
        {
            case (GameState.GameFailure):
                return failureFadeColor;
            case (GameState.GameVictory):
            default:
                return victoryFadeColor;
        }

    }
#endregion

}
