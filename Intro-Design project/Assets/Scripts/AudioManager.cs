using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Auio manager settings
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// in the sound class we handle the audio clips and their properties
    /// </summary>

    public string name; // name of the audio clip
    public AudioClip clip; // reference on the audio clip

    public bool loop = false; // do we make the sound loop 

    [Range(0f, 1f)]
    public float volume = 0.7f; // volume of the clip aka how loud it will be
    [Range(0.5f, 1.5f)]
    public float pitch = 1f; // pitch of the sound aka how high or low the sound is

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f; // random volume modificator
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f; // random pitch modificator

    private AudioSource source; //source of the clip

    public void SetSource(AudioSource _source) // setting the audio source
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        if (source != null)
        {
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f)); // setting the source pitch
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)); // setting the source volume
            source.Play(); // play the source
        }
    }

    public void Stop()
    {
        source.Stop(); // stop playing the audio source
    }
}
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// This class handles the behaviour of the Audiomanager
    /// </summary>

    private static bool audioManagerExists;

    [SerializeField]
    private Sound[] sounds = null; // array of sounds

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this); // don't destroy the audio manager when changing scene
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_ " + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlaySound("Title");
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++) // checks the array to find a clip with the name given in parameters
        {
            if (sounds[i].name == _name)
            { // if the name matches a sound play it
                sounds[i].Play();
                return;
            }
        }
        //no sound with _name
        Debug.LogWarning("AudioManager: No sound found in list: " + _name);
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Stop();
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {// if a sound matches the name, stop playing it.
                sounds[i].Stop();
                return;
            }
        }
        //no sound with _name
        Debug.LogWarning("AudioManager: No sound found in list: " + _name);
    }
    // Update is called once per frame
    void Update()
    {

    }
}