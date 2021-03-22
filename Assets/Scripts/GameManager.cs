using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField]
    private int winScore = 50;
    [SerializeField]
    private int timeRemaining = 60;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private GameObject gameText;
    [SerializeField]
    private GameObject goMenu;
    [SerializeField]
    private TMP_Text goText;
    [SerializeField]
    private TMP_Text winText;
    [SerializeField]
    private AudioSource goMusic;


    private void Start()
    {
        goMusic.ignoreListenerPause = true;
        StartCoroutine(TimeCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score + " :Objects Destroyed";
        timeText.text = "Time Remaining: " + timeRemaining;
        winText.text = "Suck " + winScore + " objects before the time runs out!";
        
        if (timeRemaining == 0)
        {
            GameOver(false);
        }
        else if(score >= winScore)
        {
            GameOver(true);
        }
            
    }

    public void AddScore()
    {
        score++;
    }

    IEnumerator TimeCountdown()
    {
        while(timeRemaining > 0)
        {
            //timeText.text = "Time Remaining: " + timeRemaining;

            yield return new WaitForSeconds(1f);

            timeRemaining--;
        }
    }

    public void GameOver(bool win)
    {
        if (!AudioListener.pause)
            AudioListener.pause = true;
        Time.timeScale = 0f;
        gameText.SetActive(false);

        if (win)
            goText.text = "Congrats, you got " + winScore + " objects and have " + timeRemaining + " time left!";
        else
            goText.text = "Aww, better luck next time!";

        goMenu.SetActive(true);
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        if (AudioListener.pause)
            AudioListener.pause = false;

        goMusic.Stop();

        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
