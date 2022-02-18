using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;
    private AudioSource LaunchSound;
    private AudioSource NewLaunchSound;

    //Audioclips
    public AudioClip MuteSound;
    public AudioClip AutoCannonSound;
    public AudioClip LargeCannonSound;
    public AudioClip ArtillerySound;
    public AudioClip NavalGunSound;

    List<LaunchSoundTracking> cannonSounds = new List<LaunchSoundTracking>();
    private int ShotsFired;

    private float CannonCaliber;

    class LaunchSoundTracking 
    {
        //Reference AudioSources
        public AudioSource OriginalLaunchSound;
        public AudioSource ReplacementLaunchSound;
        //Firing Detection
        public bool CannonShotState;
        public bool LastWasPlaying;
        public bool LastAudioTime;
        public bool CurrentAudioTime;
    }
        
    // Swaps out default cannon sound for a mute, soundless 250ms clip. Then creates replacement AudioSources to link the audioclips to.
    private void ReplaceCannonSound() {
        Debug.Log("Muting default cannon sounds.");
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
                LaunchSound = AS;
                AS.clip = MuteSound;

                Debug.Log("Creating new replacement AudioSource.");
                NewLaunchSound = AS.gameObject.AddComponent<AudioSource>();
                NewLaunchSound.spatialBlend = 1.0f;

                CannonCaliber = LaunchSound.transform.parent.Find("Base").localScale.x;
                if (CannonCaliber <= 0.06) {
                    //Up to 30mm
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
                
                //Adding to reference lists
                LaunchSoundTracking cannon = new LaunchSoundTracking();
                    LaunchSoundTracking.OriginalLaunchSound = LaunchSound;
                    LaunchSoundTracking.ReplacementLaunchSound = NewLaunchSound;

            }
        }
    }

    private void DetectCannonFiring() {
        foreach (LaunchSoundTracking cannon in cannonSounds) {
            cannon.CurrentAudioTime = cannon.OriginalLaunchSound.timeSamples;
                if (cannon.OriginalLaunchSound.isPlaying && !cannon.LastWasPlaying) {
                    Debug.Log("SFX started.");
                    cannon.CannonShotState = true;
                }
                else if ((cannon.LastAudioTime > cannon.CurrentAudioTime) && (cannon.OriginalLaunchSound.isPlaying)) {
                    Debug.Log("SFX overlapped.");
                    cannon.CannonShotState = true;
                } 
                else {
                    cannon.CannonShotState = false;
                }

                cannon.LastWasPlaying = cannon.OriginalLaunchSound.isPlaying;
                cannon.LastAudioTime = cannon.CurrentAudioTime;
        }
    }

    //Coroutine for PlayOneShot delay
    IEnumerator PlayAfterDelay(AudioSource source, float time, AudioClip clip)
    {
        yield return new WaitForSeconds(time);
        source.PlayOneShot(clip);
    }

    private void PlayCannonSound() {
        foreach (LaunchSoundTracking cannon in cannonSounds) {
            if (cannon.CannonShotState == true) {
                float soundDelay = Vector3.Distance(LaunchSound.transform.parent.position, Camera.main.transform.position) / 343;
                StartCoroutine(PlayAfterDelay(cannon.ReplacementLaunchSound, soundDelay, cannon.ReplacementLaunchSound.clip));
            }
        }
    }

    void Start()
    {
        Invoke("ReplaceCannonSound",1.0f);
    }

    void Update()
    {
        if ((LaunchSound != null) && (Time.timeScale != 0)) {
            DetectCannonFiring();
        }
        PlayCannonSound();
    }

}