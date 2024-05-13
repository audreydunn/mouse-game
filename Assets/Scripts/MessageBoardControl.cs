using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class MessageBoardControl : MonoBehaviour
{
    public GameObject messageTemplate;
    public CanvasGroup pauseMenu;
    public int BoardTimeBeforeFade = 10;
    public int BoardFadeDuration = 1;
    public GameObject messageParent;
    private CanvasGroup boardCanvas;
    private bool fadeFlag;
    private bool mouse_over = false;
    private float countToFade;
    private float curAlpha;
    private string controls = "Controls:\n * ESC / Start: Pause\n * WASD / left joystick: move\n * Spacebar / X button:  jump";
    private string objective = "Current objective:\n * Collect cheese as fast as you can.\n * Beware the cat and traps.\n * Survive.";

    public void addMessage(string text)
    {
        GameObject newMessage = Instantiate(messageTemplate);
        TextMeshProUGUI messageText = newMessage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        messageText.text = text;
        newMessage.SetActive(true);
        newMessage.transform.SetParent(messageParent.transform, false);
        //drop first message
        if (messageParent.transform.childCount > 10)
        {
            //Debug.Log(messageParent.transform.childCount);
            //string toDeleteText = messageParent.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            //Debug.Log("got child");
            //Debug.Log(toDeleteText);
            Object.DestroyImmediate(messageParent.transform.GetChild(0).gameObject);
        }
        float y = 0;
        for (int i = messageParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject message = messageParent.transform.GetChild(i).gameObject;
            RectTransform pos = message.GetComponent<RectTransform>();
            pos.anchorMin = new Vector2(0.03f, 1f - (0.1f * (y + 1)));
            pos.anchorMax = new Vector2(0.97f, 1f - (0.002f + 0.1f * y));
            y++;
        }
        // restart alpha and fade counter on new message
        countToFade = BoardTimeBeforeFade * 1f;
        boardCanvas.alpha = 1f;
        curAlpha = 1f;
        fadeFlag = false;
    }

    public void pointerEnter()
    {
        mouse_over = true;
        //Debug.Log("Mouse enter");
        //addMessage("test mouse over");

    }
    public void pointerExit()
    {
        mouse_over = false;
        //Debug.Log("Mouse exit");
    }
    private void Awake()
    {
        boardCanvas = GetComponent<CanvasGroup>();
        boardCanvas.alpha = 1f;
        curAlpha = 1f;
        boardCanvas.interactable = true;
        fadeFlag = false;
        countToFade = BoardTimeBeforeFade * 1f;
        addMessage(controls);
        addMessage(objective);
    }

    void Update()
    {
        if (pauseMenu.interactable) // hide on pause menu
        {
            boardCanvas.alpha = 0f;

        }
        else
        {
            boardCanvas.alpha = curAlpha;
            if (mouse_over)
            {
                // restart alpha and fade counter on mouse over
                //Debug.Log("Mouse Over");
                countToFade = BoardTimeBeforeFade * 1f;
                boardCanvas.alpha = 1f;
                curAlpha = 1f;
                fadeFlag = false;
            }
            else
            {
                if (countToFade > 0)
                {
                    countToFade -= Time.deltaTime;
                    if (countToFade <= 0)
                    {
                        fadeFlag = true;
                    }
                }

                if (fadeFlag)
                {
                    curAlpha -= Time.deltaTime / BoardFadeDuration;
                    boardCanvas.interactable = true;
                    if (curAlpha <= 0f)
                    {
                        curAlpha = 0f;
                        fadeFlag = false;
                    }
                    boardCanvas.alpha = curAlpha;
                }
            }
        }
        
        
    }
}
