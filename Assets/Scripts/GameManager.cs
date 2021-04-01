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
    public GameObject Quit;
    private int starsGathered;
    public Vector3 lastStarPosition;
    [SerializeField] private AudioClip yay;
    [SerializeField] private AudioClip continueSound;
    [SerializeField] private AudioClip song2;
    [SerializeField] private AudioClip song1;
    [SerializeField] private GameObject confetti;
    private GameObject confettiRef;
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        GameOver.SetActive(false);
        StartCoroutine(SceneController.Instance.FadeOutAndIn(0f, 0f, .65f));
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StarsText.enabled = false;
            Quit.SetActive(true);
        }
    }

    public void handleQuit()
    {
        Application.Quit();

    }

    public void handleCancel()
    {
        StarsText.enabled = true;
        Quit.SetActive(false);
        audioSource.PlayOneShot(continueSound);
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
        if (lastStarPosition != null)
        {
            confettiRef = Instantiate(confetti, new Vector3(lastStarPosition.x, lastStarPosition.y + 12f, lastStarPosition.z), Quaternion.identity);
        }
        GameOverStarsText.text = "You found " + starsGathered + " stars!";

    }

    public void PlayAgain()
    {
        starsGathered = 0;
        StarsText.text = "Stars: " + starsGathered;
        GameOver.SetActive(false);
        Scene level1 = SceneManager.GetSceneByName("Level1");

        Destroy(confettiRef);
        audioSource.Stop();

        if (level1.IsValid())
        {
            SceneManager.UnloadSceneAsync("Level1");
        }
        else
        {
            SceneManager.UnloadSceneAsync("Level2");
        }
        SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive);
        StartCoroutine(SceneController.Instance.FadeOut(.25f));
        int randomSong = Mathf.RoundToInt(Random.Range(0, 2));
        AudioClip song = randomSong == 1 ? song1 : song2;
        StarsText.enabled = true;
        audioSource.clip = song;
        audioSource.Play();

    }


}
