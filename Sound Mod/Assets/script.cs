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
        public int LastAudioTime;
        public int CurrentAudioTime;
    }
<<<<<<< Updated upstream
=======

    class WingGunSoundController
    {   
        //Reference AudioSources
        public AudioSource OriginalGunSound;
        public AudioSource ReplacementGunSound;
        //Firing Detection 
        public bool ShotState;
        public float ShotTimer;
        //Gun Properties (use RPS to play the clip as many times as needed)
        public float roundsPerSecond;
    }
    
    class MinigunSoundController
    {   
        //Reference AudioSources
        public AudioSource OriginalMinigunSound;
        public AudioSource ReplacementMinigunSound;
        //Firing Detection 
        public int state = 0;
        public int prevState = 0;
    }
>>>>>>> Stashed changes
        
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
                //Changing properties of the new object
                cannon.OriginalLaunchSound = LaunchSound;
                cannon.ReplacementLaunchSound = NewLaunchSound;
                //Adding the new object to the list
                cannonSounds.Add(cannon);

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

<<<<<<< Updated upstream
=======
    private void PlayCannonSound() {
        foreach (LaunchSoundController cannon in LaunchSoundList) {
            if (cannon.ShotState == true) {
                float soundDelay = Vector3.Distance(cannon.OriginalLaunchSound.transform.parent.position, Camera.main.transform.position) / 343;
                StartCoroutine(PlayAfterDelay(cannon.ReplacementLaunchSound, soundDelay, cannon.ReplacementLaunchSound.clip));
            }
        }
    }

    private void ReplaceBombExplosion() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "BombExplosionSound") {
                BombExplosionSound = AS;
                if (Vector3.Distance(BombExplosionSound.transform.parent.position, Camera.main.transform.position) > 500) {
                    AS.clip = NewBombExplosion_Far;
                } else {
                    AS.clip = NewBombExplosion_Close;
                }
            }
        }
    }

    private void ReplaceGunSound() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "Wing Gun") {
                WingGunSound = AS;
                AS.clip = MuteClip;

                NewWingGunSound = AS.gameObject.AddComponent<AudioSource>();

                //NewWingGun sound properties.
                NewWingGunSound.spatialBlend = 1.0f;
                NewWingGunSound.dopplerLevel = 0.0f;

                NewWingGunSound.clip = NewWingGunClip;

                //Adding to reference lists
                WingGunSoundController gun = new WingGunSoundController();
                //Changing properties of the new object
                gun.OriginalGunSound = WingGunSound;
                gun.ReplacementGunSound = NewWingGunSound;

                //Firing rate
                IPart part = WingGunSound.GetComponentInParent<IPartScript>().Part;
                XElement wingGunXml;
                foreach (var modifier in part.Modifiers) {
                    if (modifier.StateElementName == "Gun.State") {
                        wingGunXml = modifier.GenerateStateXml();
                        gun.roundsPerSecond = float.Parse(wingGunXml.Attribute("roundsPerSecond").Value);
                        break;
                    }
                }

                //Adding the new object to the list
                WingGunSoundList.Add(gun);
            }
        }
    }

    private void DetectGunFiring() {
        foreach (WingGunSoundController gun in WingGunSoundList) {
            //ShotTimer tracks the time passed
            if (gun.OriginalGunSound.isPlaying) {
                gun.ShotTimer += Time.deltaTime;
            } else if (gun.OriginalGunSound.isPlaying == false) {
                gun.ShotTimer = 0;
            }
        
            //Checks if enough time has passed between shots AND if the gun is actually "firing"
            if ((gun.ShotTimer > 1/gun.roundsPerSecond) && (gun.OriginalGunSound.isPlaying)) {
                gun.ShotTimer = 0;
                gun.ShotState = true;
            } else {
                gun.ShotState = false;
            }
        }
    }

    private void PlayGunSound() {
        foreach (WingGunSoundController gun in WingGunSoundList) {
            if (gun.ShotState == true) {
                float soundDelay = Vector3.Distance(gun.OriginalGunSound.transform.parent.position, Camera.main.transform.position) / 343;
                StartCoroutine(PlayAfterDelay(gun.ReplacementGunSound, soundDelay, gun.ReplacementGunSound.clip));
            }
        }
    }

    private void ReplaceMinigunSound() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "Minigun") {
                MinigunSound = AS;
                AS.clip = MuteClip;

                NewMinigunSound = AS.gameObject.AddComponent<AudioSource>();

                //NewMinigun sound properties.
                NewMinigunSound.spatialBlend = 1.0f;
                NewMinigunSound.dopplerLevel = 0.0f;

                //Adding to reference lists
                MinigunSoundController minigun = new MinigunSoundController();
                //Changing properties of the new object
                minigun.OriginalMinigunSound = MinigunSound;
                minigun.ReplacementMinigunSound = NewMinigunSound;

                //Adding the new object to the list
                MinigunSoundList.Add(minigun);
            }
        }
    }

    private void DetectMinigunFiring() {
        foreach (MinigunSoundController minigun in MinigunSoundList) {
            
            Debug.Log("Gun state: " + minigun.state);

            /*
            Initial Standby State - 0
            Start State - 1
            Loop Check State - 2
            Loop State - 3
            Stop Check State - 4
            Stop State - 5
            */

            if (minigun.state == 0) {
                if (minigun.OriginalMinigunSound.isPlaying) {
                    minigun.state = 1;
                }
                if (!minigun.OriginalMinigunSound.isPlaying) {
                    minigun.state = 0;
                }
            }
            
            //States have internal switches that are only active when they are in a new state
            //Start State
            if (minigun.state == 1) {
                if (minigun.prevState != minigun.state) {
                    float soundDelay = Vector3.Distance(minigun.OriginalMinigunSound.transform.parent.position, Camera.main.transform.position) / 343;
                    //Coroutine for play start clip, wait length, and change state
                    StartCoroutine(PlayMinigunStartSound(minigun, soundDelay));
                }
            }

            //Loop State
            if (minigun.state == 2) {
                if (minigun.prevState != minigun.state) {
                    float soundDelay = Vector3.Distance(minigun.OriginalMinigunSound.transform.parent.position, Camera.main.transform.position) / 343;
                    //Coroutine for play loop sound, wait length, and change state
                    StartCoroutine(PlayMinigunLoopSound(minigun, soundDelay));
                }
            }

            //Stop State
            if (minigun.state == 3) {
                if (minigun.prevState != minigun.state) {
                    float soundDelay = Vector3.Distance(minigun.OriginalMinigunSound.transform.parent.position, Camera.main.transform.position) / 343;
                    //Coroutine for play stop sound, wait length, and change state
                    StartCoroutine(PlayMinigunStopSound(minigun, soundDelay));
                }
            }

            minigun.prevState = minigun.state;

        }
    }

    IEnumerator PlayMinigunStartSound(MinigunSoundController minigun, float time) {
        StartCoroutine(PlayAfterDelay(minigun.ReplacementMinigunSound, time, NewMinigunClip_Start));
        yield return new WaitForSeconds(time + NewMinigunClip_Start.length);
        if (minigun.OriginalMinigunSound.isPlaying) {
            //If still firing after start sound, go to loop
            minigun.state = 2;
        }
        if (!minigun.OriginalMinigunSound.isPlaying) {
            //If stopped firing after start, go to stop
            minigun.state = 3;
        }
    }

    IEnumerator PlayMinigunLoopSound(MinigunSoundController minigun, float time) {
        StartCoroutine(PlayAfterDelay(minigun.ReplacementMinigunSound, time, NewMinigunClip_Loop));
        yield return new WaitForSeconds(time + NewMinigunClip_Loop.length);
        if (minigun.OriginalMinigunSound.isPlaying) {
            //If still firing after a loop, go to loop again
            minigun.state = 2;
        }
        if (!minigun.OriginalMinigunSound.isPlaying) {
            //If stopped firing after a loop, go to stop
            minigun.state = 3;
        }
    }

    IEnumerator PlayMinigunStopSound(MinigunSoundController minigun, float time) {
        StartCoroutine(PlayAfterDelay(minigun.ReplacementMinigunSound, time, NewMinigunClip_End));
        yield return new WaitForSeconds(time + NewMinigunClip_End.length);
        minigun.state = 0;
    }

    private void ReplaceFlareSound() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "Countermeasure Dispenser") {
                FlareSound = AS;
                AS.clip = NewFlareClip;
            }
        }
    }

>>>>>>> Stashed changes
    //Coroutine for PlayOneShot delay
    IEnumerator PlayAfterDelay(AudioSource source, float time, AudioClip clip) {
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