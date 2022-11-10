using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseController : MonoBehaviour
{
    public HoseHandleMovement hoseHandle;

    // Start is called before the first frame update
    void Awake()
    {
        this.hoseHandle = GameObject.FindGameObjectWithTag("HoseHandle").GetComponent<HoseHandleMovement>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 10 ) {
            this.hoseHandle.ActivateHandleMovement(true);
        } else {
            this.hoseHandle.ActivateHandleMovement(false);
        }

    }
}
