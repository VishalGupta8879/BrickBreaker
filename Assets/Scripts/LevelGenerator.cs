using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGenerator : MonoBehaviour
{

    [Header("Level Generator")]
    [SerializeField]
    private GameObject lvlPrefab;
    [SerializeField]
    private Transform content;

    private List<GameObject> allPrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
        allPrefabs = new List<GameObject>();
        AllStaticVariables.instance.deviceBackBtnFunction.AddListener(delegate()
        {
            LoadScene("HomeScreen");
        });
        SetLevels();
    }

    internal void SetLevels()
    {
        for(int i = 0; i < AllStaticVariables.instance.totalLevel; i++)
        {
            GameObject prefab = Instantiate(lvlPrefab,content);
            prefab.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            int levelNo = i + 1;
            
            if((i + 1) <= AllStaticVariables.instance.clearedLevelNo)
            {
                prefab.transform.GetComponent<Button>().onClick.RemoveAllListeners();
                prefab.transform.GetComponent<Button>().onClick.AddListener(delegate { LevelClicked(levelNo); });
                prefab.transform.GetChild(1).gameObject.SetActive(false);
            }                
            else
                prefab.transform.GetChild(1).gameObject.SetActive(true);
            allPrefabs.Add(prefab);
        }
    }

    internal void LevelClicked(int levelNo)
    {
        AllStaticVariables.instance.LogMessage("LevelNo "+levelNo);
        AllStaticVariables.instance.currentLevel = levelNo;
        LoadScene("GamePlay");
    }

    public void LoadScene(string sceneName)
    {
        AllStaticVariables.instance.LoadScene(sceneName);
    }
}
