using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HealthTracker : MonoBehaviour
{
    public bool debugDealOneDamage = false;
    public bool debugHealOneDamage = false;

    public Image[] heartContainers;
    public Sprite emptyHeart;
    public Sprite fullHeart;

    public Animator mouseAnimator;
    public CanvasGroup gameOver;
    public GameObject gotHitScreen;

    public int maxHealth = 3;
    public int curHealth;
    [SerializeField] private UIManager eventSystem;

    void Start()
    {
        if (!mouseAnimator)
        {
            mouseAnimator = GetComponent<Animator>();
        }
    }

    private void Awake()
    {
        curHealth = maxHealth;
        if (heartContainers.Length == 0 || heartContainers is null)
        {
            Debug.Log("hearthContainers list not found or empty");
        }
        if (fullHeart is null)
        {
            Debug.Log("fullHeart sprite is null");
        }
        if (emptyHeart is null)
        {
            Debug.Log("emptyHeart sprite is null");
        }
        updateHealthbar();
    }

    private void updateHealthbar()
    {
        if (emptyHeart is null || fullHeart is null || heartContainers is null || heartContainers.Length == 0)
        {
            Debug.Log("Healthbar update Failed. Missing either a heartContainers reference or an emtpyHeart sprite");
        }
        else
        {
            for (int i = 0; i < curHealth; i++)
            {
                heartContainers[i].sprite = fullHeart;
            }
            for (int i = curHealth; i < maxHealth; i++)
            {
                heartContainers[i].sprite = emptyHeart;
            }
        }

    }

    public void LoseHealth(int amount)
    {
        if (curHealth > 0)
        {
            // don't do more damage than current health
            if (curHealth < amount) 
            {
                amount = curHealth;
            }
            curHealth -= amount;
            // update heart containers
            updateHealthbar();
            // play pain sound
            EventManager.TriggerEvent<MouseInPainEvent, Vector3>(transform.position);

            // Enable got hit screen.
            gotHitScreen.SetActive(true);

            if (curHealth > maxHealth) 
            {
                Debug.Log("Error: Health should not be above max health");
            }
            if (curHealth <= 0) 
            {
                // lock mouse into fail animation state
                mouseAnimator.Play("Fail");
                mouseAnimator.SetBool("isLost", true);

                // apply game over effect to UI
                eventSystem.ShowGameOver();
            }
        }

        // Disable hit screen.
        Invoke("SetHitScreenFalse", 0.2f); // disable after 0.2 seconds
    }

    public void GainHealth(int amount)
    {
        if (curHealth <= maxHealth)
        {
            // don't do more damage than current health
            if (curHealth + amount > maxHealth )
            {
                amount = maxHealth - curHealth;
            }
            curHealth += amount;
            updateHealthbar();
            // play relieve? sound
            //EventManager.TriggerEvent<MouseInPainEvent, Vector3>(transform.position);

            // Enable got healed screen.
            //gotHitScreen.SetActive(true);

            if (curHealth > maxHealth)
            {
                Debug.Log("Error: Health should not be above max health");
            }
        }
        // Disable relieve? screen.
        //Invoke("SetHitScreenFalse", 0.2f); // disable after 0.2 seconds
    }

    void SetHitScreenFalse()
    {
        gotHitScreen.SetActive(false);
    }

    void Update()
    {
        if (debugDealOneDamage)
        {
            debugDealOneDamage = false;
            LoseHealth(1);
        }

        if (debugHealOneDamage)
        {
            debugHealOneDamage = false;
            GainHealth(1);
        }
    }

    public int GetCurHealth()
    {
        return curHealth;
    }
}
