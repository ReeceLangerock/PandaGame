using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonobehaviour<GameManager>
{

    public TextMeshProUGUI StarsText;
    public TextMeshProUGUI GameOverStarsText;
    public GameObject GameOver;
    private int starsGathered;
    private int totalStars;
    [SerializeField] private AudioClip yay;
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        StartCoroutine(SceneController.Instance.FadeOutAndIn(0f, 0f, .65f));
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
    }


    public void IncrementStarsGathered(bool isLastStar)
    {

        starsGathered++;
        SetStarsText();
        if (isLastStar)
        {
            HandleEndGame();
        }
    }

    private void SetStarsText()
    {
        StarsText.text = "Stars: " + starsGathered;

    }

    private void HandleEndGame()
    {
        StarsText.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(yay);
        GameOver.SetActive(true);
        GameOverStarsText.text = starsGathered + " stars collected!";

    }

    public void PlayAgain()
    {
        StartCoroutine(SceneController.Instance.FadeOutAndIn(.25f, .75f, .5f));
        SceneManager.LoadScene("Level1");

    }


}
