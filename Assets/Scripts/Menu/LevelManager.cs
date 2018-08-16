using UnityEngine;
using UnityEngine.SceneManagement;
using BaseClasses;
using UnityEngine.UI;

public class LevelManager : BaseSingleton<LevelManager>
{
    public GameObject LevelSelectorContent;

    /// <summary>
    /// To load the scene at number
    /// </summary>
    /// <param SceneNumber="i"></param>
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void LoadNextLevel()
    {
        int n = SceneManager.GetActiveScene().buildIndex;
        int k = SceneManager.sceneCountInBuildSettings;
        if (n+1 >= k)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(n + 1);
    }
    
    public void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            SpawnAllLevels();
        }
    }

    public void SpawnAllLevels()
    {
        if (!FindObjectOfType<GridLayoutGroup>())
        {
            Debug.LogError("Couldn't find the GridLayoutGroup that you were looking for");
            return;
        }
        
        Transform scrollRect = FindObjectOfType<GridLayoutGroup>().transform;
        // Mainmenu, LevelSelecter 
        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            GameObject levelButton = Instantiate(LevelSelectorContent, scrollRect) as GameObject;
            levelButton.GetComponentInChildren<Text>().text = "Level " + (i-1);
            levelButton.GetComponent<LevelButton>().Level = i;
        }
    }
}
