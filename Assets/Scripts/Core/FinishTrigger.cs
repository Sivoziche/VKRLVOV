using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == playerTag)
        {
            FinishReached();
            //string levelName = getLevelName();
            //string nextLevelName = getNextLevelName(levelName);
            //SceneManager.LoadScene(nextLevelName);

            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }
    }

    private void FinishReached()
    {
        Debug.Log("Finish");
    }

    private string getLevelName()
    {
        Scene scene = SceneManager.GetActiveScene();
        string levelName = scene.name;
        Debug.Log("Current level: " + levelName);
        return levelName;
    }

    private string getNextLevelName(string levelName)
    {
        string currLevelName = "";
        int levelNum = levelName[levelName.Length - 1];

        return currLevelName;
    }
}
