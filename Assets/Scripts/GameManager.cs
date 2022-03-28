using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Global Variables
    public static GameManager instance;

    [SerializeField]
    private GameObject exitPopUp;

    [Header("GameOver PopUp")]
    [SerializeField]
    private GameObject gameOverPopUp;
    [SerializeField]
    private TextMeshProUGUI currentScoreTxt;

    [Header("Level Cleared PopUp")]
    [SerializeField]
    private GameObject levelCleared;
    [SerializeField]
    private TextMeshProUGUI currentLvl;
    [SerializeField]
    private TextMeshProUGUI currentScrTxt;
    [SerializeField]
    private TextMeshProUGUI bestScrTxt;
    [SerializeField]
    private TextMeshProUGUI timeTaken;

    [Header("Common Game UI")]
    [SerializeField]
    private TextMeshProUGUI scoreTxt;
    [SerializeField]
    private TextMeshProUGUI bestScoreTxt;
    [SerializeField]
    private TextMeshProUGUI time;
    [SerializeField]
    private TextMeshProUGUI levelNo;
    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    internal GameObject blastEffect;

    [Header("Level's")]
    public List<GameObject> levels;

    internal GameState gameState;

    internal bool isBallServed;

    internal int currentScore;
    private int currentLevel;

    internal int totalBrick;
    internal int breakedBrick;

    private string timeTakenToComplete;

    Ball ball;
    Trajectory trajectory;

    [SerializeField] float ballSpeed;
    float zDept;

    private Vector3 mousePosition;
    
    private Camera cam;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        cam = Camera.main;
        ball = FindObjectOfType<Ball>();

        AllStaticVariables.instance.ballStartPos = ball.Position;

        zDept = cam.transform.position.z - transform.position.z;
        mousePosition = new Vector3(0f, 0f, zDept);

        trajectory = FindObjectOfType<Trajectory>();

        gameState = GameState.Start;

        currentLevel = AllStaticVariables.instance.currentLevel;

        currentScore = 0;
        scoreTxt.text = "Score : " + currentScore;
        bestScoreTxt.text = "Best : " + 0;
        bestScoreTxt.text = "Best : " + AllStaticVariables.instance.highScore;


        levelNo.text = "Level " + currentLevel;
        time.text = "Time : 00";


        blastEffect.SetActive(false);

        exitPopUp.SetActive(false);
        gameOverPopUp.SetActive(false);
        levelCleared.SetActive(false);

        isBallServed = false;

        for(int i = 0; i < levels.Count; i++)
        {
            if((i+1) == currentLevel)
            {
                levels[i].SetActive(true);
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
        instruction.SetActive(true);
        AllStaticVariables.instance.deviceBackBtnFunction.AddListener(CheckExitPopUp);
    }

    #region Time taken by Player
    //Set time taken by player
    void SetTime()
    {
        float seconds = (Time.timeSinceLevelLoad);        
        time.text = "Time : " + ConvertTime(seconds);
    }

    //convert time into minutes and second format
    internal string ConvertTime(float sec)
    {
        string second = sec.ToString().Split('.')[0];
        int minute = int.Parse(second) / 60;
        int seconds = int.Parse(second) - minute * 60;
        timeTakenToComplete = minute + "M : " + seconds+"S";
        return minute + "M : "+seconds+"S";
    }
    #endregion

    #region GamePlay Logic

    //Check ball is in moving position or not
    internal bool IsBallServed()
    {
        return isBallServed;
    }

    //Handle Ball movement
    void Update()
    {
        if (!isBallServed)
        {
            trajectory.Show();
            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;
            trajectory.UpdateDots(Camera.main.WorldToScreenPoint(ball.Position), mousePosition);
            if (Input.GetKeyDown(KeyCode.Mouse0))
                ServeBall();
        }
        else
        {
            if (gameState == GameState.Playing)
            {
                SetTime();
                ball.CheckVelocity();
            }                
        }
    }

    //Launch the ball first time on Input position
    void ServeBall()
    {
        isBallServed = true;

        instruction.SetActive(false);
        trajectory.Hide();

        gameState = GameState.Playing;

        // Update mouse position at the launch time
        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.y;

        Vector3 ballPosition = cam.WorldToScreenPoint(ball.Position); // ball position in Screenpoint

        // Caculate the direction
        Vector3 direction = (mousePosition - ballPosition);
        direction = direction.normalized;

        Vector2 force = new Vector2(direction.x * ballSpeed, direction.y * ballSpeed);

        ball.AddForce(force);
    }
    #endregion

    #region GameOver PopUp Setup
    internal void SetGameOver()
    {
        Time.timeScale = 0;
        gameState = GameState.GameOver;
        gameOverPopUp.SetActive(true);
        currentScoreTxt.text = "" + currentScore;
    }
    #endregion

    #region Level Cleared PopUp SetUp
    internal void SetLevelCleared()
    {
        Time.timeScale = 0;
        gameState = GameState.Cleared;
        gameOverPopUp.SetActive(false);
        levelCleared.SetActive(true);
        timeTaken.text = "Time taken : "+timeTakenToComplete;
        if (currentLevel == AllStaticVariables.instance.clearedLevelNo)
        {
            AllStaticVariables.instance.clearedLevelNo++;
            AllStaticVariables.instance.SaveClearedLevelNo();
        }
            
        currentLvl.text = "Level " + currentLevel + " cleared !";
        currentScrTxt.text = "Score : " + currentScore;
        bestScrTxt.text = "Score : " + AllStaticVariables.instance.GetBestScore(currentScore);
    }

    public void NextBtnClicked()
    {
        gameOverPopUp.SetActive(false);
        levelCleared.SetActive(false);

        AudioManager.instance.PlayClickSound();

        AllStaticVariables.instance.currentLevel++;
        if (AllStaticVariables.instance.currentLevel > AllStaticVariables.instance.totalLevel)
            AllStaticVariables.instance.currentLevel = 1;
        AllStaticVariables.instance.LoadScene("GamePlay");
    }

    public void ReplayClicked()
    {
        levelCleared.SetActive(false);
        gameOverPopUp.SetActive(false);

        AudioManager.instance.PlayClickSound();

        AllStaticVariables.instance.LoadScene("GamePlay");
    }
    #endregion

    #region Exit PopUp Methods
    public void CheckExitPopUp()
    {
        if (exitPopUp.activeSelf)
        {
            Time.timeScale = 1;
            exitPopUp.SetActive(false);
            if (!isBallServed)
                gameState = GameState.Start;
            else
                gameState = GameState.Playing;
        }
        else
        {
            gameState = GameState.Freeze;
            Time.timeScale = 0;
            exitPopUp.SetActive(true);
        }
    }

    public void NoBtnClicked()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayClickSound();
        exitPopUp.SetActive(false);
    }
    #endregion

    #region Load Scene by SceneName
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayClickSound();
        AllStaticVariables.instance.LoadScene(sceneName);
    }
    #endregion

    internal void SetScore()
    {
        scoreTxt.text = "Score : " + currentScore;
    }
}

public enum GameState { Start, Playing, Cleared,Freeze, GameOver }
