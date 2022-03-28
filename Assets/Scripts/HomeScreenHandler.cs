using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeScreenHandler : MonoBehaviour
{
    #region Global Variables

    [Header("ExitPopUp")]
    [SerializeField]
    private GameObject exitPopUp;

    [Header("Setting PopUp")]
    [SerializeField]
    private GameObject settingPopUp;
    [SerializeField]
    private Image sound;
    [SerializeField]
    private Image sfx;
    [SerializeField]
    private Sprite sfxOff;
    [SerializeField]
    private Sprite sfxOn;
    [SerializeField]
    private Sprite soundOff;
    [SerializeField]
    private Sprite soundOn;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        settingPopUp.SetActive(false);
        exitPopUp.SetActive(false);

        #region Sound Checking
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound").Equals("On"))
            {
                sound.sprite = soundOn;
                AudioManager.instance.ManageBgSound();
            }
            else
            {
                sound.sprite = soundOff;
                AudioManager.instance.ManageBgSound();
            }
        }

        if (PlayerPrefs.HasKey("Sfx"))
        {
            if (PlayerPrefs.GetString("Sfx").Equals("On"))
            {
                sfx.sprite = sfxOn;
            }
            else
            {
                sfx.sprite = sfxOff;
            }
        }
        #endregion

        AllStaticVariables.instance.deviceBackBtnFunction.AddListener(CheckExitPopUp);
    }

    #region Setting Button Clicks
    public void MusicClicked()
    {
        if (PlayerPrefs.GetString("Sound").Equals("On"))
        {
            PlayerPrefs.SetString("Sound", "Off");
            sound.sprite = soundOff;
        }
        else
        {
            PlayerPrefs.SetString("Sound", "On");
            sound.sprite = soundOn;
        }           

        AudioManager.instance.ManageBgSound();
        AudioManager.instance.PlayClickSound();
    }

    public void SfxClicked()
    {
        if (PlayerPrefs.GetString("Sfx").Equals("On"))
        {
            sfx.sprite = sfxOff;
            PlayerPrefs.SetString("Sfx", "Off");
        }
        else
        {
            sfx.sprite = sfxOn;
            PlayerPrefs.SetString("Sfx", "On");
        }
        AudioManager.instance.PlayClickSound();

    }
    #endregion

    #region Exit PopUp Methods
    internal void CheckExitPopUp()
    {
        if (exitPopUp.activeSelf)
            exitPopUp.SetActive(false);
        else
            exitPopUp.SetActive(true);
    }

    public void YesBtnClicked()
    {
        AudioManager.instance.PlayClickSound();
        Application.Quit();
    }

    public void NoBtnClicked()
    {
        AudioManager.instance.PlayClickSound();
        exitPopUp.SetActive(false);
    }
    #endregion

    #region Load LevelScene
    public void LoadScene(string sceneName)
    {
        AudioManager.instance.PlayClickSound();
        AllStaticVariables.instance.LoadScene(sceneName);
    }
    #endregion
}
