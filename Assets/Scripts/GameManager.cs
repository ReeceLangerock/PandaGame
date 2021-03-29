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
    public Vector3 lastStarPosition;
    [SerializeField] private AudioClip yay;
    [SerializeField] private GameObject confetti;
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        GameOver.SetActive(false);
        StartCoroutine(SceneController.Instance.FadeOutAndIn(0f, 0f, .65f));
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
        Instantiate(confetti, new Vector3(lastStarPosition.x, lastStarPosition.y + 12f, lastStarPosition.z), Quaternion.identity);
        GameOverStarsText.text = starsGathered + " stars collected!";

    }

    public void PlayAgain()
    {

        starsGathered = 0;
        StartCoroutine(SceneController.Instance.FadeOutAndIn(.25f, .75f, .5f));
        SceneManager.LoadScene("Level1");
    }


}
