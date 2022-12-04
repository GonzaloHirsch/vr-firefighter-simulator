using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour, IWaterInteractable
{
    [Header("Debug")]
    public bool debug = false;

    [Header("State")]
    public bool isOn = false;
    public float timeToOnMin = 3f;
    public float timeToOnMax = 3f;
    public float timeToOn;
    [SerializeField]
    private float timeToOnDelta = 0f;
    [SerializeField]
    private bool shouldTurnOn = false;

    [Header("Neighbors")]
    [SerializeField]
    private int onNeighbors = 0;
    private bool hasDownNeighbor = false;
    public float maxNeighborDistance = 4f;
    [Range(1f, 3f)]
    public float downMultiplier = 1f;
    public float downNeighborDist = 0.5f;

    [Header("Fire Turning Off")]
    private bool isTurningOff = false;
    public float maxHitInterval = 2f;
    public float currentHitInterval = 0f;
    public float totalHitTime = 10f;
    public float currentHitTime = 0f;

    [Header("Neighbors")]
    // This list shouldn't be initialized in the code
    // We can leave it like this, in case the programmer decides to manually input some of them
    public List<Fire> neighbors;
    public int onNeighborsThreshold = 2;
    public float minNeighborDownDistance = 3f;

    [Header("Particle System")]
    public GameObject mainEffectGo;
    private ParticleSystem pSystem;

    void Awake()
    {
        this.pSystem = this.mainEffectGo.GetComponent<ParticleSystem>();
        this.timeToOn = Random.Range(this.timeToOnMin, this.timeToOnMax);
    }

    void Update()
    {
        // Handle turning on
        if (this.shouldTurnOn && !this.isOn)
        {
            this.timeToOnDelta += (Time.deltaTime * (this.hasDownNeighbor ? this.downMultiplier : 1f));
            // If over the limit with the neighbors
            if (this.timeToOnDelta >= this.timeToOn)
            {
                this.isOn = true;
                this.shouldTurnOn = false;
                this.timeToOnDelta = 0f;
                this.isTurningOff = false;
                this.currentHitTime = 0f;
                this.UpdateFireState(true);
            }
        }
        // Handle turning off
        if (this.isTurningOff && this.isOn) {
            // Add to the timers
            this.currentHitTime += Time.deltaTime;
            this.currentHitInterval += Time.deltaTime;
            // If the current interval timer is larger than the limit, it stops turning off
            if (this.currentHitInterval >= this.maxHitInterval) {
                this.isTurningOff = false;
                this.currentHitTime = 0f;
            } else if (this.currentHitTime >= this.totalHitTime) {
                // Fire turns off
                this.isOn = false;
                this.shouldTurnOn = false;
                this.timeToOnDelta = 0f;
                this.isTurningOff = false;
                this.currentHitTime = 0f;
                this.UpdateFireState(true);
            }
        }
    }

    public void NotifyNeighborState(bool fireOn)
    {
        // Update the count
        if (fireOn) this.onNeighbors++;
        else this.onNeighbors--;
        this.CheckNeighborState();
    }

    /* ------------------------------------------------------------------------------ */
    /* STATE */
    /* ------------------------------------------------------------------------------ */

    public void UpdateFireState(bool notify)
    {
        if (this.isOn)
        {
            this.pSystem.Play();
        }
        else
        {
            this.pSystem.Stop();
        }
        if (notify)
        {
            this.NotifyAllNeighbors();
            FireController.Instance.FireChanged(this.isOn ? 1 : -1);
        }
    }

    public void WaterHit(Vector3 normal)
    {
        // Reset timer when hit
        this.isTurningOff = true;
        this.currentHitInterval = 0f;
    }

    // Method to externally turn on the fire, without notifying the neighbors
    public void SetIsOn(bool _isOn)
    {
        this.isOn = _isOn;
        this.UpdateFireState(false);
    }

    /* ------------------------------------------------------------------------------ */
    /* NEIGHBORS */
    /* ------------------------------------------------------------------------------ */

    public void FindNeighbors(Fire[] allFires, int fireId)
    {
        // Count how many of the neighbors are on
        this.onNeighbors = 0;
        // Find all the neighbors within the given distance
        for (int i = 0; i < allFires.Length; i++)
        {
            // If neighbor is within distance, add it
            if (i != fireId && Vector3.Distance(this.gameObject.transform.position, allFires[i].gameObject.transform.position) < this.maxNeighborDistance)
            {
                neighbors.Add(allFires[i]);
                if (allFires[i].isOn)
                {
                    this.onNeighbors++;
                }
            }
        }
        // Check on the neighbor state
        this.CheckNeighborState();
    }

    private void CheckNeighborState()
    {
        /*
            If we are over the neighbor limit, the fire is off and is not turning on, 
            it should start counting
        */
        if (this.onNeighbors >= this.onNeighborsThreshold)
        {
            if (!this.isOn && !this.shouldTurnOn)
            {
                this.shouldTurnOn = true;
                this.timeToOnDelta = 0f;
                // Find if any of the neihbors is down
                this.hasDownNeighbor = false;
                foreach (Fire neighbor in this.neighbors) {
                    this.hasDownNeighbor = this.hasDownNeighbor || (this.transform.position.y - neighbor.transform.position.y > this.minNeighborDownDistance);
                }
            }
        }
        else
        {
            this.shouldTurnOn = false;
            this.timeToOnDelta = 0f;
        }
    }

    private void NotifyAllNeighbors()
    {
        foreach (Fire fire in this.neighbors) fire.NotifyNeighborState(this.isOn);
    }

    /* ------------------------------------------------------------------------------ */
    /* DEBUG */
    /* ------------------------------------------------------------------------------ */

    void OnDrawGizmosSelected()
    {
        if (debug)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.maxNeighborDistance);
        }
    }

    private IEnumerator TurnOffSprite()
    {
        Component[] particleSystems = GetComponentsInChildren(typeof(ParticleSystem));
        for (int i = 0; i < 10; i++)
        {
            for(int s=0; s < particleSystems.Length; s++)
            {
                ParticleSystem system = (ParticleSystem)particleSystems[s];
                ParticleSystem.MainModule main = system.main;
                main.maxParticles = system.main.maxParticles / 2;
                yield return new WaitForSeconds(0.01f);
            }
        }
        this.pSystem.Stop();
        for (int s = 0; s < particleSystems.Length; s++)
        {
            ParticleSystem system = (ParticleSystem)particleSystems[s];
            system.Stop();
        }
    }

    private IEnumerator TurnOnSprite()
    {
        Component[] particleSystems = GetComponentsInChildren(typeof(ParticleSystem));
        for (int s = 0; s < particleSystems.Length; s++)
        {
            ParticleSystem system = (ParticleSystem)particleSystems[s];
            ParticleSystem.MainModule main = system.main;
            main.maxParticles = 1000;
        }
        this.pSystem.Play();
        for (int s = 0; s < particleSystems.Length; s++)
        {
            ParticleSystem system = (ParticleSystem)particleSystems[s];
            system.Play();
        }
        yield return new WaitForSeconds(0.01f);
    }


}
