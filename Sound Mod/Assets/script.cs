using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;
    public AudioClip MyClip;
    private bool HasInstantiated;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetAudioSources",1);
    }

    // Update is called once per frame
    void Update()
    {
        if (ServiceProvider.Instance.PlayerAircraft.Controls.FireWeapons == true) {
            DetectCannonFiring();
        }
        if (ServiceProvider.Instance.PlayerAircraft.Controls.FireWeapons == false) {
            HasInstantiated = false;
        }
    }

    private void GetAudioSources() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
            AS.clip = MyClip;
            }
        }
    }

    private void DetectCannonFiring() {
        AudioSource LaunchSound = GameObject.Find("LaunchSound").GetComponent<AudioSource>();
        if (LaunchSound.isPlaying == true) {
            if (HasInstantiated == false) {
                GameObject NewAudioSource = Instantiate(GameObject.Find("LaunchSound"));
                NewAudioSource.GetComponent<AudioSource>().Play();

                HasInstantiated = true;
            }
        }
    }
}