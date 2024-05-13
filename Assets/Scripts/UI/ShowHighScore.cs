using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHighScore : MonoBehaviour
{
    // High score support.
    private float fastestTime;
    public TextMeshProUGUI fastestTimeText;
    private float minutes;
    private float seconds;


    // Update is called once per frame
    void Update()
    {
        SetHighScoreText();
    }


    private void SetHighScoreText()
    {
        fastestTime = PlayerPrefs.GetFloat("HighScore");
        if (fastestTime > 0)
        {
            minutes = Mathf.FloorToInt(fastestTime / 60);
            seconds = Mathf.FloorToInt(fastestTime % 60);
            fastestTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            fastestTimeText.text = "NONE YET";
        }
    }
}
