using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;
    private AudioSource LaunchSound;
    private AudioSource NewLaunchSound;
    public AudioClip AutoCannonSound;
    public AudioClip LargeCannonSound;
    public AudioClip ArtillerySound;
    public AudioClip NavalGunSound;
    private float LastAudioTime;
    private float CurrentAudioTime;
    private bool CannonFiredAgain;
    private float CannonCaliber;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ReplaceCannonSound",1);
    }

    // Update is called once per frame
    void Update()
    {
        if (LaunchSound != null) {
            DetectCannonFiring();
            SoundOverlap();   
        }        
    }

    private void ReplaceCannonSound() {
        Debug.Log("Replacing Cannon Sound");
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
                LaunchSound = AS;
                CannonCaliber = LaunchSound.transform.parent.Find("Base").localScale.x;
                if (CannonCaliber <= 0.06) {
                    //Up to 30mm is "autocannon"
                    AS.clip = AutoCannonSound;
                } else if (CannonCaliber > 0.06 && CannonCaliber <= 0.25) {
                    //Up to 125mm
                    AS.clip = LargeCannonSound;
                } else if (CannonCaliber > 0.25 && CannonCaliber <= 0.4) {
                    //Up to 200mm
                    AS.clip = ArtillerySound;
                } else if (CannonCaliber > 0.4) {
                    //200mm+
                    AS.clip = NavalGunSound;
                }
            }
        }
    }

    private void DetectCannonFiring() {
        CurrentAudioTime = LaunchSound.time;
        if ((LastAudioTime > CurrentAudioTime) && LaunchSound.isPlaying) {
            CannonFiredAgain = true;
            Debug.Log("Cannon fired");
        } else {
            CannonFiredAgain = false;
        }
        LastAudioTime = CurrentAudioTime;
    }

    private void SoundOverlap() {
        if (CannonFiredAgain) {
            LaunchSound.time = LastAudioTime;
            LaunchSound.Play();
            GameObject NewLaunchSound = Instantiate(GameObject.Find("LaunchSound"));
            NewLaunchSound.GetComponent<AudioSource>().Play();
            Debug.Log("SFX was still playing. Overlapping sound.");
        }
    }
}