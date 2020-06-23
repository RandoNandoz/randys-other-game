using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<EnemyAI> currentEnemyGameObjects = new List<EnemyAI>();
    public static GameManager instance;

    private int score;

    public Text scoreText;

    private AudioSource myAudioSource;



    public string mainMenuScene = "MainMenu";
    // Awake is called before any starting commences.
    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    { 
        UpdateScoreText();
        myAudioSource = GetComponent<AudioSource>();
        currentEnemyGameObjects = FindObjectsOfType<EnemyAI>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText();
    }

    public void AddScore(int scr)
    {
        score += scr;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void PlayPain(AudioClip clip)
    {
        if (clip != null)
        {
            myAudioSource.PlayOneShot(clip);
        }
    }

    public void GoToLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ResetLevel()
    {
        for (int i = 0; i < currentEnemyGameObjects.Count; i++)
        {
            currentEnemyGameObjects[i].ThanosSnap();
        }
    }
}
