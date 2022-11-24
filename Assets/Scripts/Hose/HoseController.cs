using System.Collections;
using UnityEngine;

public class HoseController : MonoBehaviour
{
    public HoseHandleMovement hoseHandle;
    public ParticleSystem hoseWaterPs;
    public AudioSource audioSource;
    public AudioClip hoseAudioStart;
    public AudioClip hoseAudioDuring;
    public float waterDelayTime;
    public float changeStateInterval;

    private float lastChangeState = 10f;
    private bool hoseIsOn;

    // Start is called before the first frame update
    void Awake()
    {
        this.hoseHandle = GameObject.FindGameObjectWithTag("HoseHandle").GetComponent<HoseHandleMovement>();   
        this.audioSource = this.GetComponent<AudioSource>();
        this.hoseIsOn = false;
        this.HandleHoseWater();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsValidChange() && !this.hoseHandle.getIsTurning() && ActionMapper.GetDevicesTrigger()) 
        {
            this.hoseIsOn = !this.hoseIsOn;
            this.hoseHandle.ActivateHandleMovement(this.hoseIsOn);
            this.HandleHoseAudio();
            StartCoroutine(this.HandleHoseWaterWithDelay(this.waterDelayTime));
            this.lastChangeState = 0f;
        }
        this.lastChangeState += Time.deltaTime;
    }

    bool IsValidChange() 
    {
        return this.lastChangeState >= this.changeStateInterval;
    }

    void HandleHoseAudio()
    {
        if (this.hoseIsOn) {
            StartCoroutine(this.StartHoseAudio());
        } else {
            this.EndHoseAudio();
        }
    }

    IEnumerator StartHoseAudio()
    {
        this.audioSource.clip = this.hoseAudioStart;
        this.audioSource.Play();
        yield return new WaitForSeconds(this.audioSource.clip.length);
        this.audioSource.clip = this.hoseAudioDuring;
        this.audioSource.Play();
    }

    void EndHoseAudio()
    {
        StartCoroutine(AudioUtils.FadeOut(this.audioSource, 0.7f));
    }

    IEnumerator HandleHoseWaterWithDelay(float delayTime) 
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        // Turn water on or off
        this.HandleHoseWater();
    }

    void HandleHoseWater() 
    {
        if (this.hoseIsOn) {
            this.hoseWaterPs.Play();
        } else {
            this.hoseWaterPs.Stop();
        }
    }

}
