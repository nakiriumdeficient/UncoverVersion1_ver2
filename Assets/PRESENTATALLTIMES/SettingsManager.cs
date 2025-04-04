using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Awake()
    {
        // Auto-assign AudioMixer if not manually assigned
        if (audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("MainAudioMixer");
        }
    }
    void Start()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer is not assigned in the Inspector!");
            return; // Prevents null reference errors
        }

        float masterVol =0f, musicVol =0f, sfxVol = 0f;
        audioMixer.GetFloat("MasterVolume", out masterVol);
        audioMixer.GetFloat("MusicVolume", out musicVol);
        audioMixer.GetFloat("SFXVolume", out sfxVol);

        masterSlider.value = Mathf.Pow(10, masterVol / 20);
        musicSlider.value = Mathf.Pow(10, musicVol / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVol / 20);
    }

    public void OnMasterVolumeChanged(float value)
    {
        Debug.Log("Master Volume Changed: " + value);

        float volume = Mathf.Lerp(-80f, 0f, value); // This will scale the slider value from 0-1 to -80 (mute) to 0 (max volume)
                                                    
        // Log the calculated volume for debugging
        Debug.Log("Mapped Volume: " + volume);

        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void OnMusicVolumeChanged(float value)
    {
        float volume = Mathf.Lerp(-80f, 0f, value); // This will scale the slider value from 0-1 to -80 (mute) to 0 (max volume)
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void OnSFXVolumeChanged(float value)
    {
        float volume = Mathf.Lerp(-80f, 0f, value); // This will scale the slider value from 0-1 to -80 (mute) to 0 (max volume)
        audioMixer.SetFloat("SFXVolume", volume);
    }
}
