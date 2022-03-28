using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class AllStaticVariables : MonoBehaviour
{
    public static AllStaticVariables instance;

    public int totalLevel;
    internal int clearedLevelNo;
    internal int highScore;
    internal int currentLevel;

    internal Vector2 ballStartPos;
    internal UnityEvent deviceBackBtnFunction;

    //creating instance of clas
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            deviceBackBtnFunction = null;
            deviceBackBtnFunction = new UnityEvent();

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //cache cleared level's
        clearedLevelNo = PlayerPrefs.GetInt("ClearedLevel", 1);

        //cache hight Score
        highScore = PlayerPrefs.GetInt("HighScore", 0);

    }

    // Update is called once per frame
    void Update()
    {
        //Check for device back button key
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (instance != null)
                deviceBackBtnFunction.Invoke();
        }
    }

    //Get the best score
    internal int GetBestScore(int currentScore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            return currentScore;
        }
            
        return highScore;
    }

    //Save Cleared level no
    internal void SaveClearedLevelNo()
    {
        PlayerPrefs.SetInt("ClearedLevel", clearedLevelNo);
        PlayerPrefs.Save();
    }

    //Save high score
    internal void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    //print debug statement
    internal void LogMessage(string message)
    {
        Debug.Log(message);
    }

    //Load scene by sceneName
    internal void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
