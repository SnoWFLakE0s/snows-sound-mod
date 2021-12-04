using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;
    public AudioClip MyClip;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetAudioSources",1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetAudioSources(){
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources){
            if (AS.name == "LaunchSound"){
            AS.clip = MyClip;
            }
        }
    }
}