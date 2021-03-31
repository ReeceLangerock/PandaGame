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
    private GameObject confettiRef;
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
        Debug.Log(lastStarPosition);
        if (lastStarPosition != null)
        {
            confettiRef = Instantiate(confetti, new Vector3(lastStarPosition.x, lastStarPosition.y + 12f, lastStarPosition.z), Quaternion.identity);
        }
        GameOverStarsText.text = starsGathered + " stars collected!";

    }

    public void PlayAgain()
    {
        starsGathered = 0;
        GameOver.SetActive(false);
        Scene level1 = SceneManager.GetSceneByName("Level1");

        Destroy(confettiRef);

        if (level1.IsValid())
        {
            SceneManager.UnloadSceneAsync("Level1");
        }
        else
        {
            SceneManager.UnloadSceneAsync("Level2");
        }
        SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive);
        StartCoroutine(SceneController.Instance.FadeOutAndIn(.5f, 1.75f, .75f));
    }


}
