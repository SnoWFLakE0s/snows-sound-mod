using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jundroo.SimplePlanes.ModTools.Interfaces.Parts;
using System.Xml.Linq;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;

    /*
    Naming Conventions:

    For AudioSources, use original source name + "Sound" (if not included)
    For new replacement AudioSources, use "New" + Original audiosource name

    For audio clips, use name + "Clip"

    For classes, use original source name + "Controller"

    For lists, use original source name + "Sound" + "List"
    */

    //Cannon AudioSources
    private AudioSource LaunchSound;
    private AudioSource NewLaunchSound;

    //Bomb AudioSources
    private AudioSource BombExplosionSound;

    //WingGun AudioSources
    private AudioSource WingGunSound;
    private AudioSource NewWingGunSound;

    //Minigun AudioSources
    private AudioSource MinigunSound;
    private AudioSource NewMinigunSound;

    //Flare AudioSources
    private AudioSource FlareSound;

    //General AudioClip
    public AudioClip MuteClip;

    //Cannon Audioclips
    public AnimationCurve Falloff;
    public AudioRolloffMode rolloffMode;
    public AudioClip HeavyGunClip;
    public AudioClip AutoCannonClip;
    public AudioClip SmallCannonClip;
    public AudioClip MediumCannonClip;
    public AudioClip LargeCannonClip;
    public AudioClip ArtilleryClip;
    public AudioClip MediumArtilleryClip;
    public AudioClip LargeArtilleryClip;
    public AudioClip NavalGunClip;
    public AudioClip LargeNavalGunClip;

    //Bomb Audioclips
    public AudioClip NewBombExplosion_Close;
    public AudioClip NewBombExplosion_Far;    

    //WingGun Audioclips
    public AudioClip NewWingGunClip;

    //Minigun Audioclips
    public AudioClip NewMinigunClip_Start;
    public AudioClip NewMinigunClip_Loop;
    public AudioClip NewMinigunClip_End;

    //Flare Audioclips
    public AudioClip NewFlareClip;

    List<LaunchSoundController> LaunchSoundList = new List<LaunchSoundController>();
    List<WingGunSoundController> WingGunSoundList = new List<WingGunSoundController>();
    List<MinigunSoundController> MinigunSoundList = new List<MinigunSoundController>();

    private float CannonCaliber;

    class LaunchSoundController
    {
        //Reference AudioSources
        public AudioSource OriginalLaunchSound;
        public AudioSource ReplacementLaunchSound;
        //Firing Detection
        public bool ShotState;
        public bool LastWasPlaying;
        public int LastAudioTime;
        public int CurrentAudioTime;
    }

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
        public bool LastWasPlaying;
    }
        
    // Swaps out default cannon sound for a mute, soundless 250ms clip. Then creates replacement AudioSources to link the audioclips to.
    private void ReplaceCannonSound() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            if (AS.name == "LaunchSound") {
                LaunchSound = AS;
                AS.clip = MuteClip;

                NewLaunchSound = AS.gameObject.AddComponent<AudioSource>();
                
                //NewLaunchSound sound properties.
                NewLaunchSound.spatialBlend = 1.0f;
                //NewLaunchSound.outputAudioMixerGroup = LaunchSound.outputAudioMixerGroup;
                NewLaunchSound.dopplerLevel = 0.1f;
                NewLaunchSound.maxDistance = 3000.0f;
                NewLaunchSound.rolloffMode = AudioRolloffMode.Custom;
                NewLaunchSound.SetCustomCurve(AudioSourceCurveType.CustomRolloff, Falloff);

                CannonCaliber = LaunchSound.transform.parent.Find("Base").localScale.x;
                if (CannonCaliber <= 0.026) {
                    //Up to 12.7mm
                    NewLaunchSound.clip = HeavyGunClip;
                } else if (CannonCaliber > 0.026 && CannonCaliber <= 0.06) {
                    //Up to 30mm
                    NewLaunchSound.clip = AutoCannonClip;
                } else if (CannonCaliber > 0.06 && CannonCaliber <= 0.1) {
                    //Up to 50mm
                    NewLaunchSound.clip = SmallCannonClip;
                } else if (CannonCaliber > 0.1 && CannonCaliber <= 0.15) {
                    //Up to 75mm
                    NewLaunchSound.clip = MediumCannonClip;
                } else if (CannonCaliber > 0.15 && CannonCaliber <= 0.2) {
                    //Up to 100mm
                    NewLaunchSound.clip = LargeCannonClip;
                } else if (CannonCaliber > 0.2 && CannonCaliber <= 0.25) {
                    //Up to 125mm
                    NewLaunchSound.clip = ArtilleryClip;
                } else if (CannonCaliber > 0.25 && CannonCaliber <= 0.3) {
                    //Up to 150mm
                    NewLaunchSound.clip = MediumArtilleryClip;
                } else if (CannonCaliber > 0.3 && CannonCaliber <= 0.35) {
                    //Up to 175mm
                    NewLaunchSound.clip = LargeArtilleryClip;
                } else if (CannonCaliber > 0.35 && CannonCaliber <= 0.4) {
                    //Up to 200mm
                    NewLaunchSound.clip = NavalGunClip;
                } else if (CannonCaliber > 0.4) {
                    //250mm+
                    NewLaunchSound.clip = LargeNavalGunClip;
                }
                
                //Create object
                LaunchSoundController cannon = new LaunchSoundController();
                //Changing properties of the new object
                cannon.OriginalLaunchSound = LaunchSound;
                cannon.ReplacementLaunchSound = NewLaunchSound;
                //Adding the new object to the list
                LaunchSoundList.Add(cannon);

            }
        }
    }

    private void DetectCannonFiring() {
        foreach (LaunchSoundController cannon in LaunchSoundList) {
            cannon.CurrentAudioTime = cannon.OriginalLaunchSound.timeSamples;
                if (cannon.OriginalLaunchSound.isPlaying && !cannon.LastWasPlaying) {
                    cannon.ShotState = true;
                }
                else if ((cannon.LastAudioTime > cannon.CurrentAudioTime) && (cannon.OriginalLaunchSound.isPlaying)) {
                    cannon.ShotState = true;
                } 
                else {
                    cannon.ShotState = false;
                }

                cannon.LastWasPlaying = cannon.OriginalLaunchSound.isPlaying;
                cannon.LastAudioTime = cannon.CurrentAudioTime;
        }
    }

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
            //Checking for "started firing"
            if (!minigun.LastWasPlaying && minigun.OriginalMinigunSound.isPlaying) {
                minigun.ReplacementMinigunSound.clip = NewMinigunClip_Start;
                float soundDelay = Vector3.Distance(minigun.OriginalMinigunSound.transform.parent.position, Camera.main.transform.position) / 343;
                StartCoroutine(PlayAfterDelay(minigun.ReplacementMinigunSound, soundDelay, minigun.ReplacementMinigunSound.clip));
            }
        }
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

    //Coroutine for PlayOneShot delay
    IEnumerator PlayAfterDelay(AudioSource source, float time, AudioClip clip)
    {
        yield return new WaitForSeconds(time);
        source.PlayOneShot(clip);
    }

    //Testing function to print out all Audiosource names
    private void AudioSourceTest() {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources) {
            Debug.Log(AS.name);
        }
    }

    void Start()
    {
        Invoke("ReplaceCannonSound",1.0f);
        Invoke("ReplaceGunSound",1.0f);
        Invoke("ReplaceMinigunSound",1.0f);
        Invoke("ReplaceFlareSound",1.0f);
        Invoke("ReplaceBombExplosion",1.0f);
        //Invoke("AudioSourceTest",1.0f);
    }

    void Update()
    {
        if ((LaunchSound != null) && (Time.timeScale != 0)) {
            DetectCannonFiring();
            PlayCannonSound();
        }
        if ((WingGunSound != null) && (Time.timeScale != 0)) {
            DetectGunFiring();
            PlayGunSound();
        }
        if ((MinigunSound != null) && (Time.timeScale != 0)) {
            DetectMinigunFiring();
        }
        if ((BombExplosionSound != null) && (Time.timeScale != 0)) {
            ReplaceBombExplosion();
        }
    }

}