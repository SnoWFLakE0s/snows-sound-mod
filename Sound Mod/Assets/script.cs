using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;
    private AudioSource LaunchSound;
    private AudioSource NewLaunchSound;
    public AudioClip MuteSound;

    public AudioClip AutoCannonSound;
    public AudioClip LargeCannonSound;
    public AudioClip ArtillerySound;
    public AudioClip NavalGunSound;

    private bool LastWasPlaying;
    private bool CannonFired;
    private int LastAudioTime;
    private int CurrentAudioTime;
    private int ShotsFired;

    private float CannonCaliber;
        
    // Start is called before the first frame update
    void Start()
    {
        Invoke("MuteCannonSound",0.5f);
        Invoke("NewCannonSound",0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if ((LaunchSound != null) && (Time.timeScale != 0)) {
            DetectCannonFiring();
        }

        if (CannonFired == true) {
            PlayCannonSound();
            ShotsFired++;
            Debug.Log("Cannon fired " + ShotsFired.ToString() + " times");
        } 
    }

    // Swaps out default cannon sound for a mute, soundless 250ms clip.
    private void MuteCannonSound() {
        Debug.Log("Muting default cannon sounds");
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
                LaunchSound = AS;
                AS.clip = MuteSound;
            }
        }
    }

    private void NewCannonSound() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
                LaunchSound = AS;
                NewLaunchSound = AS.gameObject.AddComponent<AudioSource>();
                NewLaunchSound.spatialBlend = 1.0f;

                CannonCaliber = LaunchSound.transform.parent.Find("Base").localScale.x;
                if (CannonCaliber <= 0.06) {
                    //Up to 30mm is "autocannon"
                    NewLaunchSound.clip = AutoCannonSound;
                } else if (CannonCaliber > 0.06 && CannonCaliber <= 0.25) {
                    //Up to 125mm
                    NewLaunchSound.clip = LargeCannonSound;
                } else if (CannonCaliber > 0.25 && CannonCaliber <= 0.4) {
                    //Up to 200mm
                    NewLaunchSound.clip = ArtillerySound;
                } else if (CannonCaliber > 0.4) {
                    //200mm+
                    NewLaunchSound.clip = NavalGunSound;
                }
            }
        }
    }

    private void DetectCannonFiring() {
            CurrentAudioTime = LaunchSound.timeSamples;
            if (LaunchSound.isPlaying && !LastWasPlaying) {
                Debug.Log("Audio started");
                CannonFired = true;
            }
            else if ((LastAudioTime > CurrentAudioTime) && (LaunchSound.isPlaying)) {
                Debug.Log("Audio overlapped");
                CannonFired = true;
            } 
            else {
                CannonFired = false;
            }

            LastWasPlaying = LaunchSound.isPlaying;
            LastAudioTime = CurrentAudioTime;
    }

    private void PlayCannonSound() {
        float Distance = Vector3.Distance(LaunchSound.transform.parent.position, Camera.main.transform.position);
        NewLaunchSound.PlayDelayed(Distance/343);
        //NewLaunchSound.PlayOneShot(AutoCannonSound,1.0f);
    }

}