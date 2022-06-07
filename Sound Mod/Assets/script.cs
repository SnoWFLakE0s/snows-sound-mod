using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    //Variables
    private AudioSource[] AudioSources;
    private AudioSource LaunchSound;
    private AudioSource NewLaunchSound;

<<<<<<< Updated upstream
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
=======
    /*
    Naming Conventions:

    For AudioSources, use original source name + "Sound" (if not included)
    For new replacement AudioSources, use "New" + Original audiosource name

    For audio clips, use name + "Clip"

    For classes, use original source name + "Controller"

    For lists, use original source name + "Sound" + "List"
    */

    //Cannon AudioSources
    private AudioSource LaunchSound, NewLaunchSound;
    //Bomb AudioSources
    private AudioSource BombExplosionSound;
    //WingGun AudioSources
    private AudioSource WingGunSound, NewWingGunSound;
    //Minigun AudioSources
    private AudioSource MinigunSound, NewMinigunSound;
    //Flare AudioSources
    private AudioSource FlareSound;
    //Turbojet AudioSources
    private AudioSource TurbojetSound, NewTurbojetSound;

    //General AudioClip
    public AudioClip MuteClip;
    //Cannon Audioclips
    public AnimationCurve Falloff;
    public AudioRolloffMode rolloffMode;
    public AudioClip HeavyGunClip, AutoCannonClip, SmallCannonClip, MediumCannonClip, LargeCannonClip, ArtilleryClip, MediumArtilleryClip, LargeArtilleryClip, NavalGunClip, LargeNavalGunClip;
    //Bomb Audioclips
    public AudioClip NewBombExplosion_Close, NewBombExplosion_Far;
    //WingGun Audioclips
    public AudioClip NewWingGunClip;
    //Minigun Audioclips
    public AudioClip NewMinigunClip_Loop;
    //Flare Audioclips
    public AudioClip NewFlareClip;
    //Turbojet Audioclips
    public AnimationCurve EngineFalloff;
    public AudioRolloffMode engineRolloffMode;
    public AudioClip NewTurbojetClip_Start, NewTurbojetClip_Idle, NewTurbojetClip_On, NewTurbojetClip_Afterburner;
    //Sonic Boom
    public AudioClip SonicBoom;

    //AudioSource Lists
    List<LaunchSoundController> LaunchSoundList = new List<LaunchSoundController>();
    List<WingGunSoundController> WingGunSoundList = new List<WingGunSoundController>();
    List<MinigunSoundController> MinigunSoundList = new List<MinigunSoundController>();
    List<TurbojetSoundController> AfterburningTurbojetSoundList = new List<TurbojetSoundController>();

    //Sound controllers
    class LaunchSoundController
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
>>>>>>> Stashed changes
        
=======

    class TurbojetSoundController
    {
        //Reference AudioSources
        public bool enabled = false;
        public AudioSource OriginalAfterburningTurbojetSound;
        public AudioSource ReplacementAfterburningTurbojetSound;
        public int state = -1;
        public int prevState = 0;
        /*
        Reflection script field.
        Contains the engine exhaust boolean and the afterburner percentage.
        Relevant Accessors:
            Engine on:
                .SmokeSystemEmission.enabled
            Afterburner percentage:
                .AfterburningSmokeSystemEmission.enabled
        */
        public JetEngineAfterburningScript AfterburnerScript;
        //Sonic Boom Controllers
        public AudioSource SonicBoomSound;
        public float alphaActualPrevious = 0.0f;
        public Vector3 EngineVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 LastEnginePosition = new Vector3(0.0f, 0.0f, 0.0f);
        public float LastDistanceToEngine = 0.0f;
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
<<<<<<< Updated upstream
                NewMinigunSound.dopplerLevel = 0.0f;
=======
                NewMinigunSound.dopplerLevel = 0.1f;
                NewMinigunSound.loop = true;
                NewMinigunSound.rolloffMode = AudioRolloffMode.Custom;
                NewMinigunSound.SetCustomCurve(AudioSourceCurveType.CustomRolloff, Falloff);
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
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
=======

            }
            else if (!minigun.OriginalMinigunSound.isPlaying)
            {
                minigun.state = 0;
                if (minigun.state != minigun.prevState)
                {
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
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
=======
    private void ReplaceTurbojetSound()
    {
        AudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource AS in AudioSources)
        {
            if (AS.name == "AfterburningTurbojet")
            {
                TurbojetSound = AS;
                AS.clip = MuteClip;
                AS.priority = 255;

                NewTurbojetSound = AS.gameObject.AddComponent<AudioSource>();
                NewTurbojetSound.spatialBlend = 1.0f;
                NewTurbojetSound.dopplerLevel = 0.4f;
                NewTurbojetSound.rolloffMode = AudioRolloffMode.Custom;
                NewTurbojetSound.SetCustomCurve(AudioSourceCurveType.CustomRolloff, EngineFalloff);
                NewTurbojetSound.loop = true;

                NewTurbojetSound.clip = NewTurbojetClip_Start;
                NewTurbojetSound.volume = 0.2f;

                TurbojetSoundController turbojet = new TurbojetSoundController
                {
                    OriginalAfterburningTurbojetSound = TurbojetSound,
                    ReplacementAfterburningTurbojetSound = NewTurbojetSound
                };

                JetEngineAfterburningScript AfterburningEngineScript = JetEngineAfterburningScript.Wrap(AS.GetComponentInParent(JetEngineAfterburningScript.RealType));
                turbojet.AfterburnerScript = AfterburningEngineScript;

                AfterburningTurbojetSoundList.Add(turbojet);
                StartCoroutine(TurbojetEngineStartupSound(turbojet));
            }
        }
    }

    IEnumerator TurbojetEngineStartupSound(TurbojetSoundController turbojet)
    {
        float soundDelay = Vector3.Distance(turbojet.ReplacementAfterburningTurbojetSound.transform.parent.position, Camera.main.transform.position) / 343;
        StartCoroutine(PlayLoopAfterDelay(turbojet.ReplacementAfterburningTurbojetSound, soundDelay));
        yield return new WaitForSeconds(turbojet.ReplacementAfterburningTurbojetSound.clip.length);
        turbojet.enabled = true;
        turbojet.prevState = -1;
    }

    private void PlayTurbojetSound()
    {
        foreach (TurbojetSoundController turbojet in AfterburningTurbojetSoundList)
        {
            if (turbojet.enabled)
            {
                if (!turbojet.AfterburnerScript.SmokeSystemEmission.enabled && !turbojet.AfterburnerScript.AfterburningSmokeSystemEmission.enabled)
                {
                    turbojet.state = 0;
                    turbojet.ReplacementAfterburningTurbojetSound.volume = 0.2f;
                    if (turbojet.state != turbojet.prevState)
                    {
                        float soundDelay = Vector3.Distance(turbojet.ReplacementAfterburningTurbojetSound.transform.parent.position, Camera.main.transform.position) / 343;
                        StartCoroutine(ChangeEngineSoundAfterDelay(turbojet.ReplacementAfterburningTurbojetSound, soundDelay, NewTurbojetClip_Idle));
                    }
                }
                else if (turbojet.AfterburnerScript.SmokeSystemEmission.enabled && !turbojet.AfterburnerScript.AfterburningSmokeSystemEmission.enabled)
                {
                    turbojet.state = 1;
                    if (turbojet.state == turbojet.prevState)
                    {
                        turbojet.ReplacementAfterburningTurbojetSound.volume = 0.4f + 0.6f * turbojet.AfterburnerScript.EngineAudioSource.volume;
                        turbojet.ReplacementAfterburningTurbojetSound.pitch = 0.8f + turbojet.AfterburnerScript.EngineAudioSource.volume;
                    }
                    if (turbojet.state != turbojet.prevState)
                    {
                        float soundDelay = Vector3.Distance(turbojet.ReplacementAfterburningTurbojetSound.transform.parent.position, Camera.main.transform.position) / 343;
                        StartCoroutine(ChangeEngineSoundAfterDelay(turbojet.ReplacementAfterburningTurbojetSound, soundDelay, NewTurbojetClip_On));
                    }
                }
                else if (turbojet.AfterburnerScript.AfterburningSmokeSystemEmission.enabled)
                {
                    turbojet.state = 2;
                    if (turbojet.state != turbojet.prevState)
                    {
                        turbojet.ReplacementAfterburningTurbojetSound.volume = 1.0f;
                        float soundDelay = Vector3.Distance(turbojet.ReplacementAfterburningTurbojetSound.transform.parent.position, Camera.main.transform.position) / 343;
                        StartCoroutine(ChangeEngineSoundAfterDelay(turbojet.ReplacementAfterburningTurbojetSound, soundDelay, NewTurbojetClip_Afterburner));
                    }
                }
                turbojet.prevState = turbojet.state;
>>>>>>> Stashed changes
            }
        }
    }

<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
    IEnumerator ChangeEngineSoundAfterDelay(AudioSource source, float time, AudioClip clip)
    {
        yield return new WaitForSeconds(time);
        source.clip = clip;
        source.Stop();
        source.Play();
    }

    private void AddSonicBoom()
    {
        foreach (TurbojetSoundController turbojet in AfterburningTurbojetSoundList)
        {
            turbojet.SonicBoomSound = turbojet.OriginalAfterburningTurbojetSound.gameObject.AddComponent<AudioSource>();
            turbojet.SonicBoomSound.clip = SonicBoom;
            turbojet.SonicBoomSound.dopplerLevel = 0.4f;
            turbojet.SonicBoomSound.volume = 0.2f;
        }
    }

    private void DetectSonicBoom()
    {
        foreach (TurbojetSoundController turbojet in AfterburningTurbojetSoundList)
        {
            float altitude = turbojet.OriginalAfterburningTurbojetSound.transform.parent.position.y;
            turbojet.EngineVelocity = (turbojet.OriginalAfterburningTurbojetSound.transform.parent.position - turbojet.LastEnginePosition) / Time.deltaTime;
            float C_obj = turbojet.EngineVelocity.magnitude;

            if (!turbojet.SonicBoomSound.isPlaying)
            {
                // Check if exceeding mach at the given altitude.
                if (C_obj / SpeedOfSound(altitude) > 1.0f)
                {
                    // Checks if the camera's position is at the mach wave edge.
                    // Generate vector from camera to aircraft, then find the
                    // angle between that vector and the aircraft velocity vector
                    // in order to determine if the angle is approximately the
                    // mach wave angle point.

                    Vector3 LineOfSightVector = turbojet.OriginalAfterburningTurbojetSound.transform.parent.position - Camera.main.transform.position;
                    // alphaActual is the current angle between the aircraft velocity vector and the LoS vector
                    float alphaActual = Vector3.Angle(turbojet.EngineVelocity, LineOfSightVector);
                    // alphaMach is the required Mach wave half-angle for the given altitude
                    float alphaMach = Mathf.Asin(SpeedOfSound(altitude) / C_obj) * (180 / Mathf.PI);
                    if (Mathf.Abs(alphaActual - alphaMach) < 10.0f)
                    {
                        if (alphaActual < turbojet.alphaActualPrevious)
                        {
                            if ((alphaActual < alphaMach) && (alphaMach < turbojet.alphaActualPrevious))
                            {
                                PlaySonicBoom(turbojet);
                            }
                        }
                        else if (turbojet.alphaActualPrevious < alphaActual)
                        {
                            if ((turbojet.alphaActualPrevious < alphaMach) && (alphaMach < alphaActual))
                            {
                                PlaySonicBoom(turbojet);
                            }
                        }
                    }
                    turbojet.alphaActualPrevious = alphaActual;
                    Debug.Log("CURRENT α: " + alphaActual + "| REQ'D α: " + alphaMach);
                }
            }
            Debug.Log("CURRENT M: " + C_obj / SpeedOfSound(altitude));
            turbojet.LastEnginePosition = turbojet.OriginalAfterburningTurbojetSound.transform.parent.position;
            turbojet.LastDistanceToEngine = Vector3.Distance(turbojet.OriginalAfterburningTurbojetSound.transform.parent.position, Camera.main.transform.position);
        }
    }

    private void PlaySonicBoom(TurbojetSoundController turbojet)
    {
        // Additional check to ensure sonic booms don't occur
        // at the front of a waveform (aka plane approaching)
        bool EngineApproach = Vector3.Distance(turbojet.OriginalAfterburningTurbojetSound.transform.parent.position, Camera.main.transform.position) < turbojet.LastDistanceToEngine;
        if (!EngineApproach)
        {
            turbojet.SonicBoomSound.Play();
            Debug.Log("Sonic Boom Played");
        }
    }

    private float SpeedOfSound(float altitude)
    {
        if (altitude < 0f) { return 329.60f; }
        if (altitude <= 11000.0f)
        {
            return -0.0041f * altitude + 340.78f;
        }
        if (altitude <= 20000.0f)
        {
            return 295.07f;
        }
        if (altitude <= 32000.0f)
        {
            return 0.0007f * altitude + 281.64f;
        }
        if (altitude <= 47000.0f)
        {
            return 0.0018f * altitude + 246.45f;
        }
        return 329.60f;
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
<<<<<<< Updated upstream
        Invoke("ReplaceCannonSound",1.0f);
=======
        Invoke("ReplaceCannonSound", 1.0f);
        Invoke("ReplaceGunSound", 1.0f);
        Invoke("ReplaceMinigunSound", 1.0f);
        Invoke("ReplaceFlareSound", 1.0f);
        Invoke("ReplaceBombExplosion", 1.0f);
        Invoke("ReplaceTurbojetSound", 1.0f);
        Invoke("AddSonicBoom", 1.0f);
        //Invoke("AudioSourceTest",1.0f);
>>>>>>> Stashed changes
    }

    void Update()
    {
        if ((LaunchSound != null) && (Time.timeScale != 0)) {
            DetectCannonFiring();
<<<<<<< Updated upstream
=======
            PlayCannonSound();
        }
        if ((WingGunSound != null) && (Time.timeScale != 0))
        {
            DetectGunFiring();
            PlayGunSound();
        }
        if ((MinigunSound != null) && (Time.timeScale != 0))
        {
            DetectMinigunFiring();
        }
        if ((BombExplosionSound != null) && (Time.timeScale != 0))
        {
            ReplaceBombExplosion();
        }
        if ((TurbojetSound != null) && (Time.timeScale != 0))
        {
            PlayTurbojetSound();
            DetectSonicBoom();
>>>>>>> Stashed changes
        }
        PlayCannonSound();
    }

}