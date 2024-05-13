using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CheeseCollector : MonoBehaviour
{
    // How much cheese the player has collected.
    public int cheeseCount = 0;
    // public float scaleIncrement = 0.1f; // OBSOLETE-Amount by which to increase player's scale

    // UI text component to display count of "Cheese" objects collected.
    public TextMeshProUGUI countText;
    public GameObject winScreen;
    public int cheeseAmount = 25;

    // Init Timer.
    private float timerCount = 0;
    private float minutes;
    private float seconds;

    // UI text component to display count of game timer and final time.
    public TextMeshProUGUI currTimerText;
    public TextMeshProUGUI winFinalTimerText;
    private float lastFastestTime;

    // High score support.
    private float fastestTime;
    public TextMeshProUGUI fastestTimeText;

    // For controller support:
    public GameObject winScreenFirstButton;

    void Start()
    {
        winScreen.SetActive(false);
        currTimerText.gameObject.SetActive(true);
        SetCountText();

        // PlayerPrefs.SetFloat("HighScore", 0); // TEST: RESET HIGHSCORE
    }

    void Update()
    {
        // Increment game timer.
        timerCount += Time.deltaTime;
        minutes = Mathf.FloorToInt(timerCount / 60);
        seconds = Mathf.FloorToInt(timerCount % 60);

        // Update timer display.
        SetCurrTimerText();
    }

    public int CollectCheese()
    {
        cheeseCount++;

        // Update the count display.
        if (countText) {
            SetCountText();
        }

        WinGame();

        return cheeseCount;
    }

    public int GetCheeseCount()
    {
        return cheeseCount;
    }

    public int GetCheeseAmount()
    {
        return cheeseAmount;
    }

    // Function to update the displayed count of "Cheese" objects collected.
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = cheeseCount.ToString() + " / " + cheeseAmount;
    }

    void WinGame()
    {
        if (cheeseCount >= cheeseAmount)
        {
            // On-win, pause game.
            Time.timeScale = 0f;

            // Set Win screen as active.
            winScreen.SetActive(true);
            currTimerText.gameObject.SetActive(false);

            // Store final time value on the screen.
            SetFinalTimerText();
            EventSystem.current.SetSelectedGameObject(winScreenFirstButton);
            Cursor.visible = true;

            // Check highest score (shortest time taken to complete game).
            // Store the current "finalTime" ONLY if it is higher than the
            // current High Score.
            SaveHighScore();
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
        winFinalTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timerCount > PlayerPrefs.GetFloat("HighScore", 0))
        {
            winFinalTimerText.color = new Color(1, 0, 0, 1);
        }
        else if (PlayerPrefs.GetFloat("HighScore") == 0)
        {
            winFinalTimerText.color = new Color(1, 0, 0, 1);
        }
        else
        {
            winFinalTimerText.color = new Color(0, 1, 0, 1);
        }
    }

    private void SaveHighScore()
    {
        if (timerCount < PlayerPrefs.GetFloat("HighScore", 0) || PlayerPrefs.GetFloat("HighScore") == 0)
        {
            PlayerPrefs.SetFloat("HighScore", timerCount);
        }
    }
}
