using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public CanvasGroup pauseMenu;
    public CanvasGroup gameOver;

    public GameObject pauseMenuFirstButton;
    public GameObject gameOverFirstButton;
    public GameObject messageBoard;

    private bool gameOverActive = false;

    // Call this method to switch to the pause menu
    public void ShowPauseMenu()
    {
        if (!gameOverActive) 
        {
            pauseMenu.interactable = true;
            pauseMenu.blocksRaycasts = true;
            pauseMenu.alpha = 1f;
            Time.timeScale = 0f;
            
            // Set the first button of the menu to be selected
            EventSystem.current.SetSelectedGameObject(null); // Clears current selection
            EventSystem.current.SetSelectedGameObject(pauseMenuFirstButton);
            Cursor.visible = true;
        }
    }

    public void DisablePauseMenu()
    {
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        pauseMenu.alpha = 0f;
        Time.timeScale = 1f;
        
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.visible = false;
    }

    // Call this method to switch to the game over screen
    public void ShowGameOver()
    {
        gameOverActive = true;

        gameOver.interactable = true;
        gameOver.blocksRaycasts = true;
        gameOver.alpha = 1f;
        
        // Set the first button of the menu to be selected
        EventSystem.current.SetSelectedGameObject(null); // Clears current selection
        EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
        Cursor.visible = true;
    }

    public void addMessageUIM(string text)
    {
        MessageBoardControl sn = messageBoard.GetComponent<MessageBoardControl>();
        sn.addMessage(text);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            if (pauseMenu.interactable) 
            {
                DisablePauseMenu();
            } 
            else 
            {
                ShowPauseMenu();
            }
        } 
    }
}
