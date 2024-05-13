using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

// This script manages when the win state occurs based on cheeses collected,
// calculate the time it took the player to complete the game, and finally,
// give out a final score. This script needs to be attached to the Player and
// to elements in the Win Screen. This script requires the CheeseCollector.cs
// and HealthTracker.cs components to calculate final score.

[RequireComponent(typeof(CheeseCollector))]
[RequireComponent(typeof(HealthTracker))]
public class PlayerProgressTracker : MonoBehaviour
{
    // Init cheese vars.
    private CheeseCollector cheeseTracker;
    private int cheeseCount;
    private int cheeseAmount;

    // Init health tracker var.
    private HealthTracker healthTracker;
    private int curHealth;

    // Init Timer.
    private float timerCount = 0;
    private float minutes;
    private float seconds;

    // UI text component to display count of game timer and final time.
    public TextMeshProUGUI currTimerText;
    public TextMeshProUGUI winFinalTimerText;

    // Win screen vars.
    public GameObject winScreen;
    public GameObject ratingStar_1;
    public GameObject ratingStar_2;
    public GameObject ratingStar_3;

    // For win screen controller support:
    public GameObject winScreenFirstButton;





    //----------------------------------------//
    // Awake is called even before start.
    private void Awake()
    {
        cheeseTracker = GetComponent<CheeseCollector>();
        if (cheeseTracker == null)
            Debug.Log("Cheese Collector could not be found");
        healthTracker = GetComponent<HealthTracker>();
        if (healthTracker == null)
            Debug.Log("Health Tracker could not be found");
    }


    // Start is called before the first frame update
    private void Start()
    {
        // Get the player's initial Cheese and Health vars.
        RetrievePlayerData();

        // Always init the game timer to 0.
        timerCount = 0;

        // Initially set Win Screen as disabled.
        winScreen.SetActive(false);
    }


    // Update is called once per frame
    private void Update()
    {
        // Retrieve the player's info each tick.
        RetrievePlayerData();

        // Continue the game timer.
        GameTimer();

        // Detect if game has been won.
        DetectWinGame();
    }
    //----------------------------------------//





    private void RetrievePlayerData()
    {
        // Get current Cheese vars from CheeseCollector reference.
        cheeseCount = cheeseTracker.GetCheeseCount();
        cheeseAmount = cheeseTracker.GetCheeseAmount();

        // Get current Health var from HealthTracker reference.
        curHealth = healthTracker.GetCurHealth();
    }


    void GameTimer()
    {
        // Increment game timer.
        timerCount += Time.deltaTime;
        minutes = Mathf.FloorToInt(timerCount / 60);
        seconds = Mathf.FloorToInt(timerCount % 60);

        // Update timer display.
        SetCurrTimerText();
    }


    private void DetectWinGame()
    {
        if (cheeseCount >= cheeseAmount)
        {
            Time.timeScale = 0f;
            winScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(winScreenFirstButton);
        }
    }


    private void SetCurrTimerText()
    {
        // Update the CURRENT count of Time passed, string.Format("{0:00}:{1:00}", minutes, seconds).
        currTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    private void SetFinalTimerText()
    {
        // Update the FINAL displayed count of Time passed, string.Format("{0:00}:{1:00}", minutes, seconds).
        winFinalTimerText.text = "Your Final Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Function to display the final rating starts.
}
