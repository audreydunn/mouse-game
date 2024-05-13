using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource defaultMusic;
    public AudioSource catChaseMusic;
    public GameObject cat1;
    public GameObject cat2;
    private CatAIForrestEdits catAI1;
    private CatAIForrestEdits catAI2;

    public float fadeDuration = 1f;
    public float transitionDelay = 0.5f;
    private float stateTimer = 0f;
    private bool isCat1ChasingOrSearching;
    private bool isCat2ChasingOrSearching;

    private bool catChaseMusicPlaying = false;
    private bool defaultMusicPlaying = false;

    private void Awake()
    {
        catAI1 = cat1.GetComponent<CatAIForrestEdits>();
        catAI2 = cat2.GetComponent<CatAIForrestEdits>();
        defaultMusic.Play();
        defaultMusicPlaying = true;
        isCat1ChasingOrSearching = false;
        isCat2ChasingOrSearching = false;
    }


    private void Update()
    {
        // Wait for some time before checking to switch music
        stateTimer += Time.deltaTime;
        if (stateTimer > transitionDelay)
        {
            isCat1ChasingOrSearching = catAI1 != null && (catAI1.currentState == CatAIForrestEdits.State.Chasing || catAI1.currentState == CatAIForrestEdits.State.Searching);
            isCat2ChasingOrSearching = catAI2 != null && (catAI2.currentState == CatAIForrestEdits.State.Chasing || catAI2.currentState == CatAIForrestEdits.State.Searching);

            if (catChaseMusicPlaying == false &&
                (isCat1ChasingOrSearching || isCat2ChasingOrSearching))
            {
                StartCoroutine(CrossFadeMusic(catChaseMusic, defaultMusic, fadeDuration));
                catChaseMusicPlaying = true;
                defaultMusicPlaying = false;
                stateTimer = 0f;    // Reset timer
            }

            else if (defaultMusicPlaying == false &&
                !isCat1ChasingOrSearching &&
                !isCat2ChasingOrSearching)
            {
                StartCoroutine(CrossFadeMusic(defaultMusic, catChaseMusic, fadeDuration));
                defaultMusicPlaying = true;
                catChaseMusicPlaying = false;
                stateTimer = 0f;    // Reset timer
            }
        }
    }

    public static IEnumerator CrossFadeMusic(AudioSource newMusic, AudioSource oldMusic, float fadeDuration)
    {
        newMusic.Play();
        newMusic.volume = 0f;

        float elapsedTime = 0;
        float startVolume = oldMusic.volume;    // Volume of current playing music
        float targetVolume = oldMusic.volume;   // Target volume of new music

        while (elapsedTime < fadeDuration)
        {
            // Fade in new music
            newMusic.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeDuration);
            oldMusic.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        newMusic.volume = targetVolume;
        oldMusic.volume = 0f;

        oldMusic.Stop();
    }

}
