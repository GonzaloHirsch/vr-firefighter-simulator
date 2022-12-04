using UnityEngine;

public class Radio : MonoBehaviour
{
    private AudioSource[] audioSources;
    public bool isOn = true;

    void Awake()
    {
        this.audioSources = this.GetComponentsInChildren<AudioSource>();
    }

    public void OnPointerClick()
    {
        this.isOn = !this.isOn;
        this.ToggleRadio();
    }

    public void OnPointerEnter()
    {
        if (this.isOn) NarratorController.Instance.NarrateRadioOff();
        else NarratorController.Instance.NarrateRadioOn();
    }

    private void ToggleRadio()
    {
        foreach (AudioSource source in this.audioSources)
        {
            if (this.isOn) source.UnPause();
            else source.Pause();
        }
    }
}
