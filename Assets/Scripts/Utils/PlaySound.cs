using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public bool canPlay = false;
    private AudioSource source;
    void Awake()
    {
        this.source = this.GetComponent<AudioSource>();
    }

    public void PlaySoundShot() {
        if (this.source != null && this.canPlay) this.source.PlayOneShot(this.source.clip);
    }
}
