using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Calls the game Scene
    public string startSceneName = "SampleScene";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        // start the game
        SceneManager.LoadScene(startSceneName);
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit(69);
    }
}
