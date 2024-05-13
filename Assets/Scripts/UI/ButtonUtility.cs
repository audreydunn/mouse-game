using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUtility : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame() 
    {
        audioSource.Play();
        SceneManager.LoadScene("Main Scene BETA");
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        // Delete High Scores on Quitting the game.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        audioSource.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoToMainMenu()
    {
        audioSource.Play();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        Cursor.visible = true;
    }
}
