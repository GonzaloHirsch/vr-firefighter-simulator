using UnityEngine;

public class NarratorController : Framework.MonoBehaviorSingleton<NarratorController>
{
    public AudioSource audioSource;
    public AudioClip radioOnClip;
    public AudioClip radioOffClip;
    public AudioClip alarmClip;

    public void NarrateAlarm() {
        this.PlayClip(this.alarmClip);
    }
    
    public void NarrateRadioOn() {
        this.PlayClip(this.radioOnClip);
    }
    
    public void NarrateRadioOff() {
        this.PlayClip(this.radioOffClip);
    }

    private void PlayClip(AudioClip clip) {
        if (!this.audioSource.isPlaying) this.audioSource.Stop();
        this.audioSource.PlayOneShot(clip);
    }
}
