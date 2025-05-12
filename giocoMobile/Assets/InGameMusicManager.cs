using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMusicManager : MonoBehaviour
{
    private static InGameMusicManager instance;
    private static AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusicInGame(false, backgroundMusic);
        }


        /* **************************************************************************** QUI INIZIA LA MODIFICA *******************************/
        musicSlider.value = AudioSettingsManager.MusicVolume;
        SetVolume(musicSlider.value);
        /* ******************************************************************************* QUI FINISCE LA MODIFICA ***************************/
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); }); 
    }

    

    public static void SetVolume(float volume)
    {
        /* **************************************************************************** QUI INIZIA LA MODIFICA *******************************/
        AudioSettingsManager.MusicVolume = volume;  
        /* ******************************************************************************* QUI FINISCE LA MODIFICA ***************************/
        audioSource.volume = volume;
    }

    public static void PlayBackgroundMusicInGame(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }
        if (audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public static void PauseBackGroundMusicInGame()
    {
        audioSource.Pause();
    }


}
