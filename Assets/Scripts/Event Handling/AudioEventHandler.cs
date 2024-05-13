using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventHandler : MonoBehaviour
{

    public EventSound3D eventSound3DPrefab;

    // Get player
    public GameObject player;

    public AudioClip chewingAudio;
    public AudioClip[] mouseWalkingAudio = null;
    public AudioClip mouseTrapAudio;
    public AudioClip mouseInPainAudio;
    public AudioClip[] mouseSqueakAudio = null;
    public AudioClip explosionAudio;
    public AudioClip catAttackAudio;
    public AudioClip catPurringAudio;
    public AudioClip ghostCatChaseAudio;
    public AudioClip ghostCatAttackAudio;
    public AudioClip ghostCatFadeAudio;
    public AudioClip pressurePlateAudio;
    public AudioClip mouseHealAudio;
    public AudioClip boingAudio;
    public AudioClip spikesAudio;
    public AudioClip catChirpAudio;

    private UnityAction<Vector3> chewingEventListener;
    private UnityAction<Vector3> mouseWalkingEventListener;
    private UnityAction<Vector3> mouseTrapEventListener;
    private UnityAction<Vector3> mouseInPainEventListener;
    private UnityAction<Vector3> mouseSqueakEventListener;
    private UnityAction<Vector3> explosionEventListener;
    private UnityAction<Vector3> catAttackEventListener;
    private UnityAction<Vector3> catPurringEventListener;
    private UnityAction<Vector3> ghostCatChaseEventListener;
    private UnityAction<Vector3> ghostCatAttackEventListener;
    private UnityAction<Vector3> ghostCatFadeEventListener;
    private UnityAction<Vector3> pressurePlateEventListener;
    private UnityAction<Vector3> mouseHealEventListener;
    private UnityAction<Vector3> boingEventListener;
    private UnityAction<Vector3> spikesEventListener;
    private UnityAction<Vector3> catChirpEventListener;

    private EventSound3D currentCatAttack = null;
    private EventSound3D currentCatPurring = null;
    private EventSound3D currentGhostCatChase = null;
    private EventSound3D currentGhostCatAttack = null;
    private EventSound3D currentGhostCatFade = null;


    void Awake()
    {
        chewingEventListener = new UnityAction<Vector3>(chewingEventHandler);
        mouseWalkingEventListener = new UnityAction<Vector3>(mouseWalkingEventHandler);
        mouseTrapEventListener = new UnityAction<Vector3>(mouseTrapEventHandler);
        mouseInPainEventListener = new UnityAction<Vector3>(mouseInPainEventHandler);
        mouseSqueakEventListener = new UnityAction<Vector3>(mouseSqueakEventHandler);
        explosionEventListener = new UnityAction<Vector3>(explosionEventHandler);
        catAttackEventListener = new UnityAction<Vector3>(catAttackEventHandler);
        catPurringEventListener = new UnityAction<Vector3>(catPurringEventHandler);
        ghostCatChaseEventListener = new UnityAction<Vector3>(ghostCatChaseEventHandler);
        ghostCatAttackEventListener = new UnityAction<Vector3>(ghostCatAttackEventHandler);
        ghostCatFadeEventListener = new UnityAction<Vector3>(ghostCatFadeEventHandler);
        pressurePlateEventListener = new UnityAction<Vector3>(pressurePlateEventHandler);
        mouseHealEventListener = new UnityAction<Vector3>(mouseHealEventHandler);
        boingEventListener = new UnityAction<Vector3>(boingEventHandler);
        spikesEventListener = new UnityAction<Vector3>(spikesEventHandler);
        catChirpEventListener = new UnityAction<Vector3>(catChirpEventHandler);
    }


    // Use this for initialization
    void Start()
    {
    }


    void OnEnable()
    {
        EventManager.StartListening<ChewingEvent, Vector3>(chewingEventListener);
        EventManager.StartListening<MouseWalkingEvent, Vector3>(mouseWalkingEventListener);
        EventManager.StartListening<MouseTrapEvent, Vector3>(mouseTrapEventListener);
        EventManager.StartListening<MouseInPainEvent, Vector3>(mouseInPainEventListener);
        EventManager.StartListening<MouseSqueakEvent, Vector3>(mouseSqueakEventListener);
        EventManager.StartListening<ExplosionEvent, Vector3>(explosionEventListener);
        EventManager.StartListening<CatGrowlEvent, Vector3>(catAttackEventListener);
        EventManager.StartListening<CatSleepingEvent, Vector3>(catPurringEventListener);
        EventManager.StartListening<GhostCatChaseEvent, Vector3>(ghostCatChaseEventListener);
        EventManager.StartListening<GhostCatAttackEvent, Vector3>(ghostCatAttackEventListener);
        EventManager.StartListening<GhostCatFadeEvent, Vector3>(ghostCatFadeEventListener);
        EventManager.StartListening<PressurePlateEvent, Vector3>(pressurePlateEventListener);
        EventManager.StartListening<mouseHealEvent, Vector3>(mouseHealEventListener);
        EventManager.StartListening<BoingEvent, Vector3>(boingEventListener);
        EventManager.StartListening<BoingEvent, Vector3>(boingEventListener);
        EventManager.StartListening<SpikesEvent, Vector3>(spikesEventListener);
        EventManager.StartListening<CatChirpEvent, Vector3>(catChirpEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<ChewingEvent, Vector3>(chewingEventListener);
        EventManager.StopListening<MouseWalkingEvent, Vector3>(mouseWalkingEventListener);
        EventManager.StopListening<MouseTrapEvent, Vector3>(mouseTrapEventListener);
        EventManager.StopListening<MouseInPainEvent, Vector3>(mouseInPainEventListener);
        EventManager.StopListening<MouseSqueakEvent, Vector3>(mouseSqueakEventListener);
        EventManager.StopListening<ExplosionEvent, Vector3>(explosionEventListener);
        EventManager.StopListening<CatGrowlEvent, Vector3>(catAttackEventListener);
        EventManager.StopListening<CatSleepingEvent, Vector3>(catPurringEventListener);
        EventManager.StopListening<GhostCatChaseEvent, Vector3>(ghostCatChaseEventListener);
        EventManager.StopListening<GhostCatAttackEvent, Vector3>(ghostCatAttackEventListener);
        EventManager.StopListening<GhostCatFadeEvent, Vector3>(ghostCatFadeEventListener);
        EventManager.StopListening<PressurePlateEvent, Vector3>(pressurePlateEventListener);
        EventManager.StopListening<mouseHealEvent, Vector3>(mouseHealEventListener);
        EventManager.StopListening<BoingEvent, Vector3>(boingEventListener);
        EventManager.StopListening<SpikesEvent, Vector3>(spikesEventListener);
        EventManager.StopListening<CatChirpEvent, Vector3>(catChirpEventListener);
    }


    void chewingEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.chewingAudio;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.2f;

            snd.audioSrc.Play();
        }
    }

    void mouseWalkingEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.mouseWalkingAudio[Random.Range(0, mouseWalkingAudio.Length)];

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.2f;

            snd.audioSrc.Play();
        }
    }

    void mouseTrapEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.mouseTrapAudio;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.5f;

            snd.audioSrc.Play();
        }
    }

    void mouseInPainEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.mouseInPainAudio;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.5f;

            snd.audioSrc.Play();
        }
    }

    void mouseSqueakEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.mouseSqueakAudio[Random.Range(0, mouseSqueakAudio.Length)];

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.5f;

            snd.audioSrc.Play();
        }
    }

    void explosionEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        { 
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.explosionAudio;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.5f;

            snd.audioSrc.Play();
        }
    }

    void catAttackEventHandler (Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            if (currentCatAttack == null)
            {
                currentCatAttack = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                currentCatAttack.audioSrc.clip = this.catAttackAudio;

                currentCatAttack.audioSrc.minDistance = 10f;
                currentCatAttack.audioSrc.maxDistance = 500f;
                currentCatAttack.audioSrc.volume = 0.5f;

                currentCatAttack.audioSrc.Play();
            }

        }
    }

    void catPurringEventHandler(Vector3 worldPos)
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, worldPos);

        if (distanceToPlayer <= 20f)
        {
            if (eventSound3DPrefab)
            {
                if (currentCatPurring == null)
                {
                    currentCatPurring = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                    currentCatPurring.audioSrc.clip = this.catPurringAudio;

                    currentCatPurring.audioSrc.minDistance = 10f;
                    currentCatPurring.audioSrc.maxDistance = 20f;
                    currentCatPurring.audioSrc.volume = 0.4f;

                    currentCatPurring.audioSrc.Play();
                }

            }
        }


    }

    void ghostCatChaseEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            if (currentGhostCatChase == null)
            {
                currentGhostCatChase = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                currentGhostCatChase.audioSrc.clip = this.ghostCatChaseAudio;

                currentGhostCatChase.audioSrc.minDistance = 10f;
                currentGhostCatChase.audioSrc.maxDistance = 500f;
                currentGhostCatChase.audioSrc.volume = 0.5f;

                currentGhostCatChase.audioSrc.Play();
            }

        }
    }

    void ghostCatAttackEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            if (currentGhostCatAttack == null)
            {
                currentGhostCatAttack = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                currentGhostCatAttack.audioSrc.clip = this.ghostCatAttackAudio;

                currentGhostCatAttack.audioSrc.minDistance = 10f;
                currentGhostCatAttack.audioSrc.maxDistance = 500f;
                currentGhostCatAttack.audioSrc.volume = 0.5f;

                currentGhostCatAttack.audioSrc.Play();
            }

        }
    }

    void ghostCatFadeEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            if (currentGhostCatFade == null)
            {
                currentGhostCatFade = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                currentGhostCatFade.audioSrc.clip = this.ghostCatFadeAudio;

                currentGhostCatFade.audioSrc.minDistance = 10f;
                currentGhostCatFade.audioSrc.maxDistance = 500f;
                currentGhostCatFade.audioSrc.volume = 0.5f;

                currentGhostCatFade.audioSrc.Play();
            }

        }
    }

    void pressurePlateEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.pressurePlateAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 10f;
            snd.audioSrc.volume = 0.8f;

            snd.audioSrc.Play();
        }
    }

    void mouseHealEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.mouseHealAudio;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;
            snd.audioSrc.volume = 0.5f;

            snd.audioSrc.Play();
        }
    }

    void boingEventHandler(Vector3 worldPos)
    {
        if (eventSound3DPrefab)
        {
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.boingAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 10f;
            snd.audioSrc.volume = 0.5f;

            snd.audioSrc.Play();
        }
    }

    void spikesEventHandler(Vector3 worldPos)
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, worldPos);

        if (distanceToPlayer <= 15f)
        {
            if (eventSound3DPrefab)
            {
                EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                snd.audioSrc.clip = this.spikesAudio;

                snd.audioSrc.volume = 0.3f;
                snd.audioSrc.minDistance = 10f;
                snd.audioSrc.maxDistance = 15f;

                snd.audioSrc.Play();
            }
        }
    }

    void catChirpEventHandler(Vector3 worldPos)
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, worldPos);

        if (distanceToPlayer <= 15f)
        {
            if (eventSound3DPrefab)
            {
                EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                snd.audioSrc.clip = this.catChirpAudio;

                snd.audioSrc.volume = 0.7f;
                snd.audioSrc.minDistance = 10f;
                snd.audioSrc.maxDistance = 15f;

                snd.audioSrc.Play();
            }
        }
    }
}
