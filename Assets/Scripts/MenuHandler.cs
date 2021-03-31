
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip startSound;

    public void LoadGameWrapper()
    {
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {
        audioSource.PlayOneShot(startSound);
        new WaitForSeconds(.75f);
        SceneManager.LoadSceneAsync("PersistentScene");
        SceneManager.LoadSceneAsync("Level1",  LoadSceneMode.Additive);
        yield return null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
