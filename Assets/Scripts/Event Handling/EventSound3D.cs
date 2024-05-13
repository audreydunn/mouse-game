/*
 * Team: Mouse Trap Studios
 * Notes: This is a direct copy from the EventSound3D script from the milestones
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EventSound3D : MonoBehaviour {

    public AudioSource audioSrc;

   // private bool startedPlaying = false;

	// Use this for initialization
	void Awake () {
        audioSrc = GetComponent<AudioSource>();
	}
      
	
	// Update is called once per frame
	void Update () {

//        if (audioSrc.isPlaying)
//        {
//            startedPlaying = true;
//
//        }
		
        if (!audioSrc.isPlaying)
        {
            Destroy(this.gameObject);
        }

	}
}
