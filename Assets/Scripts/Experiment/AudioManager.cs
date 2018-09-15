using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public AudioSource bgmSource, soundSource, unexpectedSource;
    public AudioClip bgmClip, OddBallClip, std2Clip, std3Clip, unexpectedClip1, unexpectedClip2;
    
    //const float MaxVolume_BGM = 5.0f;
    //const float MaxVolume_target = 1f;
    //const float MaxVolume_distractor = 1f;
    //static float CurrentVolumeNormalized_BGM = 0.1f;
    //static float CurrentVolumeNormalized_target = 1f;
    //static float CurrentVolumeNormalized_distractor = 1f;
    //static bool isMuted = false;

    //the following variables are used for auditory stimuli
    private int oddballPos = -99;
    private bool playUnexpected = false;
    private int unexpectedSpeed = -1; //1=regular, 2=fast
    private int playOddball = 0;
    private int surpriseIndex = -99;
    private int oddballEar = -99;
    private int surpriseEar = -99;

    int[] earSettings = new int[] { -1, 1 };
    //ISI durations for 14 tones, add more if more tones need to play
    private List<float> isi = new List<float> { 0.9f, 1.2f, 1.0f, 0.9f,
        1.7f, 1.5f, 1.0f, 1.2f, 1.5f, 1.0f, 1.7f, 1.5f, 0.9f, 1.0f};

    //Returns the same instance of singleton class
    public static AudioManager Instance() {
        if (_instance == null) {
            //check for AudioManager gameobject to set _instance singleton
            _instance = GameObject.FindObjectOfType<AudioManager>();

            //add Manager as a component if the object doesn't exist
            if (_instance == null) {
                GameObject container = new GameObject("AudioManager");
                _instance = container.AddComponent<AudioManager>();
            }
            //bgmSource should exist, if not throw exception
            if (_instance.bgmSource == null)
                throw new UnityException("bgmSource is NULL! - Instance()");

            //soundSource should exist, if not throw exception
            if (_instance.soundSource == null)
                throw new UnityException("soundSource is NULL! - Instance()");
            if (_instance.unexpectedSource == null)
                throw new UnityException("unexpectedSource is NULL! - Instance()");
        }
        return _instance;
    }

    void Awake() {
        //if the singleton hasn't been initialized yet
        if (_instance == null) {
            _instance = GameObject.FindObjectOfType<AudioManager>();

            if (_instance == null) {
                GameObject container = new GameObject("AudioManager");
                _instance = container.AddComponent<AudioManager>();
            }

            //add the background sound source
            if (bgmSource == null) {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.spatialBlend = 0.0f;
                bgmSource.loop = true;
            }
            if (soundSource == null) {
               // Debug.Log("AudioManager.Awake() - soundSource is null");
                soundSource = gameObject.AddComponent<AudioSource>();
                soundSource.spatialBlend = 0.0f;
                soundSource.loop = false;
            }

            //add soundsource for unexpected sound if needed
            if (unexpectedSource == null) {
                // Debug.Log("AudioManager.Awake() - unexpectedSource is null");
                unexpectedSource = gameObject.AddComponent<AudioSource>();
                unexpectedSource.spatialBlend = 0.0f;
                unexpectedSource.loop = false;
            }
        }
        //Seed the randomizer once
        //Random.InitState(System.DateTime.Now.Millisecond);
        //Debug.Log("System.DateTime.Millisecond: " + System.DateTime.Now.Millisecond);
    }

    public void PlayBGM() {
        if(bgmSource != null) {
            bgmSource.PlayOneShot(bgmClip, 0.55f);
        }
        else throw new UnityException("bgmSource is NULL! - PlayBGM() failed");
    }

    public void PlayOddball() {
        if (soundSource != null) {
            soundSource.PlayOneShot(OddBallClip, 0.3f);
        }
        else throw new UnityException("soundSource is NULL! - PlayBGM() failed");
    }

    public void PlayStdSound2()
    {
        if (soundSource != null) {
            soundSource.PlayOneShot(std2Clip, 0.3f);
        }
        else throw new UnityException("soundSource is NULL! - PlayBGM() failed");
    }

    public void PlayStdSound3()
    {
        if (soundSource != null) {
            soundSource.PlayOneShot(std3Clip, 0.3f);
        }
        else throw new UnityException("soundSource is NULL! - PlayBGM() failed");
    }
    public void StopBGM() {
        if (bgmSource != null) {
            bgmSource.Stop();
        }
        else throw new UnityException("bgmSource is NULL!");
    }

    public void RandomizeISI() {
        for (int i = 0; i < isi.Count; i++) {
            float temp = isi[i];
            int randomIndex = Random.Range(i, isi.Count);
            isi[i] = isi[randomIndex];
            isi[randomIndex] = temp;
        }

        //for (int i = 0; i < isi.Count; i++) {
          //  Debug.Log("isi is: " + isi[i].ToString());
        //}
    }

    //need to pass in whether target sound is played
    public void PlaySoundStimuli() {
        if (soundSource != null)
            StartCoroutine(PlaySounds());
        else
            throw new UnityException("soundSource is NULL! - Instance()");
    }

    IEnumerator PlaySounds() {
        RandomizeISI();

        int index = 0;
        
        while (index < isi.Count) {
            // Debug.Log("Index: " + index + "; ISI: " + isi[index]);


            yield return new WaitForSeconds(isi[index]);

            if (surpriseIndex == index && playUnexpected && unexpectedSource != null)
            {
                //Randomize spatial position of unexpected tone
                unexpectedSource.panStereo = surpriseEar;
                if (unexpectedSpeed == 1)
                    unexpectedSource.PlayOneShot(unexpectedClip1, 0.8f);
                else if (unexpectedSpeed == 2)
                    unexpectedSource.PlayOneShot(unexpectedClip2, 0.8f);
            }
            if (oddballPos == index && playOddball == 1)
            {
                soundSource.panStereo = oddballEar;
                soundSource.PlayOneShot(OddBallClip, 0.35f);
            }
            else
            {
                soundSource.panStereo = earSettings[Random.Range(0, earSettings.Length)];
                int stdIndex = Random.Range(1, 3);
                if (stdIndex == 1)
                    soundSource.PlayOneShot(std2Clip, 0.95f);
                else
                    soundSource.PlayOneShot(std3Clip, 0.30f);
            }
            index++;
        }
    }

    //change this to pass in index
    public void SetOddballPosition(int position) {
        oddballPos = position;
    }

    public int GetOddballPosition() {
        return oddballPos;
    }

    public void SetOddballOccurrence(bool present) {
        if (present)
            playOddball = 1;
        else
            playOddball = 0;
    }

    public int GetOddballOccurrence() {
        return playOddball;
    }
    
    public void SetOddballEar(int ear) {
        oddballEar = ear;
    }

    public int GetOddballEar() {
        return oddballEar;
    }

    public void SetUnexpectedSound(bool present) {
        if (present)
            playUnexpected = true;
        else
            playUnexpected = false;
    }

    public void SetUnexpectedSoundPos(int position) {
        surpriseIndex = position;
    }

    public int GetUnexpectedSoundPos() {
        return surpriseIndex;
    }

    public void SetUnexpectedEar (int ear) {
        surpriseEar = ear;
    }

    public int GetUnexpectedSoundEar() {
        return surpriseEar;
    }
    
    public void SetUnexpectedSoundCondition(int condition) {
        unexpectedSpeed = condition;
    } 
    // Update is called once per frame
    void Update() {
        
    }

    //static float GetBGMVolume() {
      //  return isMuted ? 0f : MaxVolume_BGM * CurrentVolumeNormalized_BGM;
    //}
}
