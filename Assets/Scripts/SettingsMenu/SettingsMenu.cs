using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject tosPanel;
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void OpenCredits()
    {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    public void OpenTOS()
    {
        settingsPanel.SetActive(false);
        tosPanel.SetActive(true);
    }

    public void CloseTOS()
    {
        tosPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }


}
