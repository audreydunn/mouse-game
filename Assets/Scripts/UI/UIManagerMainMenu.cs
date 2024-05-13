using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManagerMainMenu : MonoBehaviour
{
    public GameObject creditsScreen;
    public GameObject mainMenuFirstButton;
    public GameObject creditsFirstButton;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start ()
    {
        creditsScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (creditsScreen.activeInHierarchy)
            {
                ExitMainMenuCredits();
            }
        }
    }

    public void EnterMainMenuCredits()
    {
        if (creditsScreen != null)
        {
            audioSource.Play();
            creditsScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(creditsFirstButton);
        }
    }

    public void ExitMainMenuCredits()
    {
        if (creditsScreen != null)
        {
            audioSource.Play();
            creditsScreen.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        }
    }
}
