using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public AudioSource bgmSource, soundSource, unexpectedSource;
    public AudioClip bgmClip, OddBallClip, std1Clip, std2Clip, std3Clip, unexpectedClip;
    
    const float MaxVolume_BGM = 5.0f;
    //const float MaxVolume_target = 1f;
    //const float MaxVolume_distractor = 1f;
    static float CurrentVolumeNormalized_BGM = 0.1f;
    //static float CurrentVolumeNormalized_target = 1f;
    //static float CurrentVolumeNormalized_distractor = 1f;
    static bool isMuted = false;

    //the following variables are used for auditory stimuli
    private int oddballPos = 0;
    private bool playUnexpected = false;
    private int playOddball = 0;
    private int surpriseIndex = 0;

    //ISI durations for 13 tones, add more if more tones need to play
    private List<float> isi = new List<float> { 0.9f, 1.2f, 1.0f, 0.9f,
        1.7f, 1.5f, 1.0f, 1.2f, 1.5f, 1.0f, 1.7f, 1.5f, 0.9f };

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
            bgmSource.PlayOneShot(bgmClip, 0.3f);
        }
        else throw new UnityException("bgmSource is NULL! - PlayBGM() failed");
    }

    public void PlayOddball() {
        if (soundSource != null) {
            soundSource.PlayOneShot(OddBallClip, 0.3f);
        }
        else throw new UnityException("soundSource is NULL! - PlayBGM() failed");
    }

    public void PlayStdSound1() {
        if (soundSource != null) {
            soundSource.PlayOneShot(std1Clip, 0.3f);
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
    public void PlaySoundStimuli(bool unexpectedSound) {
        //playUnexpected = unexpectedSound;
        Debug.Log("AudioManager - isUnexpectedSound is " + unexpectedSound);
        if (soundSource != null)
            StartCoroutine(PlaySounds());
        else
            throw new UnityException("soundSource is NULL! - Instance()");
    }

    IEnumerator PlaySounds() {
        RandomizeISI();

        int index = 0;
        //Random.InitState(System.DateTime.Now.Millisecond);
        //Debug.Log("System.DateTime.Millisecond: " + System.DateTime.Now.Millisecond);

        //int surpriseIndex = Random.Range(2, isi.Count-2);
        Debug.Log("surpriseIndex: " + surpriseIndex.ToString());

        while (index < isi.Count) {
           // Debug.Log("Index: " + index + "; ISI: " + isi[index]);

            yield return new WaitForSeconds(isi[index]);
            
            //Randomize spatial position of current tone for task
            //int pan = Random.Range(-1, 2);
            //soundSource.panStereo = pan;

            if (surpriseIndex == index && playUnexpected && unexpectedSource != null) {
                //Randomize spatial position of unexpected tone
                //unexpectedSource.panStereo = Random.Range(-1, 2);
                unexpectedSource.PlayOneShot(unexpectedClip, 0.3f);
            }
            if (oddballPos == index && playOddball == 1)
                soundSource.PlayOneShot(OddBallClip, 0.5f);
            else {
                int stdIndex = Random.Range(1, 3);
                if (stdIndex == 1)
                    soundSource.PlayOneShot(std1Clip, 0.5f);
                else if (stdIndex == 2)
                    soundSource.PlayOneShot(std2Clip, 0.5f);
                else
                    soundSource.PlayOneShot(std3Clip, 0.5f);
            }
            index++;
        }
    }

    //change this to pass in index
    public void SetOddballPosition(int position) {
        //randomly select position of oddball sound
        oddballPos = position;
        //oddballPos = Random.Range(0, isi.Count);
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
        soundSource.panStereo = ear;
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

    public void SetUnexpectedEar (int ear)
    {
        unexpectedSource.panStereo = ear;
    }

    // Update is called once per frame
    void Update() {
        
    }

    static float GetBGMVolume() {
        return isMuted ? 0f : MaxVolume_BGM * CurrentVolumeNormalized_BGM;
    }
}
