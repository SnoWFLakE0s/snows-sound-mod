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
        Invoke("ReplaceCannonSound",0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (LaunchSound != null) {
            SoundFallOff();
            DetectCannonFiring();
            SoundOverlap();   
        }        
    }

    private void ReplaceCannonSound() {
        Debug.Log("Replacing Cannon Sounds");
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
            Debug.Log("Cannon fired");
            Debug.Log(LaunchSound.volume);
            CannonFiredAgain = true;
        } else {
            CannonFiredAgain = false;
        }
        LastAudioTime = CurrentAudioTime;
    }

    private void SoundFallOff() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
                LaunchSound = AS;
                float Distance = Vector3.Distance(AS.transform.position, Camera.main.transform.position);
                AS.volume = 1 - Mathf.Sqrt(Distance) / 10;
            }
        }
    }

    private void SoundOverlap() {
        if (CannonFiredAgain) {
            LaunchSound.time = LastAudioTime;
            LaunchSound.Play();
            GameObject NewLaunchSound = Instantiate(GameObject.Find("LaunchSound"));
            NewLaunchSound.GetComponent<AudioSource>().Play();
            Debug.Log("Cannon SFX was still playing. Overlapping sound.");
        }
    }
}