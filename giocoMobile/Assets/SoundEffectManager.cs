using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null) { 
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume) { 
        audioSource.volume = volume;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        /* **************************************************************************** QUI INIZIA LA MODIFICA *******************************/
        sfxSlider.value=AudioSettingsManager.SFXVolume;
        SetVolume(sfxSlider.value);
        /* ******************************************************************************* QUI FINISCE LA MODIFICA ***************************/
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void OnValueChanged()
    {
        /* **************************************************************************** QUI INIZIA LA MODIFICA *******************************/
        float volume = sfxSlider.value;
        AudioSettingsManager.SFXVolume = volume;
        /* ******************************************************************************* QUI FINISCE LA MODIFICA ***************************/
        SetVolume(sfxSlider.value);
    }
}
