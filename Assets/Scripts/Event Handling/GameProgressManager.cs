using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    // Timer variables.
    private float gameTimerCount;

    // Game state.
    private bool gameIsWon;

    // Final score.
    private int gameScore;

    // Start is called before the first frame update
    void Start()
    {
        gameIsWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameTimer();
    }

    void GameTimer()
    {
        // If the game is not over, increment game timer.
        if (!gameIsWon)
        {
            gameTimerCount += Time.deltaTime;
        }
    }

    void WinOrLose()
    {

    }
}
