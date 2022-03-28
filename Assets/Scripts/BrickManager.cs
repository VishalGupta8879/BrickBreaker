using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public TextMeshProUGUI remaingCount;
    public int totalCount;
    // Start is called before the first frame update
    void Start()
    {
        remaingCount.text = totalCount.ToString();
        GameManager.instance.totalBrick++;
    }

    #region Brick Information Setter
    //Set the remaining hit count of current brick
    internal void SetCount()
    {
        AudioManager.instance.PlayClickSound();
        totalCount--;
        if(totalCount < 0)
        {
            transform.gameObject.SetActive(false);
            GameManager.instance.breakedBrick++;

            AudioManager.instance.PlayBreakSound();

            GameManager.instance.blastEffect.SetActive(true);
            GameManager.instance.blastEffect.transform.position = transform.position;

            Invoke("HideBlastEffect", 1f);
            if (GameManager.instance.breakedBrick == GameManager.instance.totalBrick)
                GameManager.instance.SetLevelCleared();
            return;
        }
        if (totalCount == 0)
            remaingCount.enabled = false;
        remaingCount.text = totalCount.ToString();
    }

    //Hide the brick breaker animation
    private void HideBlastEffect()
    {
        GameManager.instance.blastEffect.SetActive(false);
    }
    #endregion
}
