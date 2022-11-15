using UnityEngine;
using TMPro;

public class Radio : MonoBehaviour
{
    private AudioSource[] audioSources;
    public TMP_Text instructionText;
    public bool isOn = true;

    void Awake()
    {
        this.audioSources = this.GetComponentsInChildren<AudioSource>();
    }

    public void OnPointerClick()
    {
        this.isOn = !this.isOn;
        this.ToggleRadio();
        this.instructionText.text = $"Turn {(this.isOn ? "off" : "on")}";
    }

    public void OnPointerEnter()
    {
        this.instructionText.text = $"Turn {(this.isOn ? "off" : "on")}";
        this.instructionText.gameObject.SetActive(true);
    }
    public void OnPointerExit()
    {
        this.instructionText.gameObject.SetActive(false);
    }

    private void ToggleRadio()
    {
        foreach (AudioSource source in this.audioSources)
        {
            if (this.isOn)
            {
                source.UnPause();
            }
            else
            {
                source.Pause();
            }
        }
    }
}
