using UnityEngine;

public class FireBasedAudioManager : MonoBehaviour, IFireDependant
{
    // Audio source to be played
    public AudioSource audioSource;
    // How many fires must be lit to play the audio
    public int fireLimit;
    // Time between the audio source is played
    public float timeInterval;
    // Indicates whether we are above the fire limit so the audio must be played
    private bool shouldPlay;
    // Last time audio was played
    private float lastPlayed = 0f;

    // Start is called before the first frame update
    void Awake()
    {
       this.audioSource = this.GetComponent<AudioSource>(); 
       this.shouldPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.shouldPlay) {
            if (this.lastPlayed >= this.timeInterval)
            {
                this.audioSource.Play();
                this.lastPlayed = 0f;
            }  
            this.lastPlayed += Time.deltaTime;    
        }  
    }

    public void NotifyFireChange(int fireCount) 
    {
        this.shouldPlay = fireCount >= fireLimit;
    }
}
