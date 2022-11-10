using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseController : MonoBehaviour
{
    public HoseHandleMovement hoseHandle;
    public ParticleSystem hoseWaterPs;
    public float waterDelayTime;

    private bool hoseIsOn;

    // Start is called before the first frame update
    void Awake()
    {
        this.hoseHandle = GameObject.FindGameObjectWithTag("HoseHandle").GetComponent<HoseHandleMovement>();   
        this.hoseIsOn = false;
        this.HandleHoseWater();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.hoseHandle.getIsTurning() && ActionMapper.GetDevicesTrigger()) 
        {
            this.hoseIsOn = !this.hoseIsOn;
            this.hoseHandle.ActivateHandleMovement(this.hoseIsOn);
            StartCoroutine(this.HandleHoseWaterWithDelay(this.waterDelayTime));
        }
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
