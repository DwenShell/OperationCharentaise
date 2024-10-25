using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject guiWin;
    public string sceneName;
 
    public void OnClickPlay()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
