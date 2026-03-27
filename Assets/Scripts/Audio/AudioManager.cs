using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
        public bool loop = false;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sounds;

    private Dictionary<string, Sound> soundDictionary;


    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound s in sounds)
        {
            GameObject soundObj = new GameObject("Sound_" + s.name);
            soundObj.transform.parent = this.transform;

            s.source = soundObj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

            soundDictionary.Add(s.name, s);
        }
    }

    // Play SFX
    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }

    // Stop SFX
    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Stop();
        }
    }

    // Play One Shot (good for repeated SFX like gunshots)
    public void PlayOneShot(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.PlayOneShot(s.clip);
        }
    }
}