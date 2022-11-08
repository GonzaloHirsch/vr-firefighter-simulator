using UnityEngine;

public class Fire : MonoBehaviour, IWaterInteractable
{
    public bool isOn = false;
    public float timeToOn = 3f;
    public Fire[] neighbors;
    public int onNeighborsThreshold = 2;

    private ParticleSystem pSystem;
    private AudioSource aSource;
    [SerializeField]
    private int onNeighbors = 0;
    [SerializeField]
    private float timeToOnDelta = 0f;
    [SerializeField]
    private bool shouldTurnOn = false;

    void Awake()
    {
        this.pSystem = this.GetComponent<ParticleSystem>();
        this.aSource = this.GetComponentInChildren<AudioSource>();
        this.UpdateFireState(false);
        // Set initial count of on neighbors
        foreach (Fire fire in this.neighbors) if (fire.isOn) this.onNeighbors++;
        this.CheckNeighborState();
    }

    void Update()
    {
        if (this.shouldTurnOn)
        {
            this.timeToOnDelta += Time.deltaTime;
            // If over the limit with the neighbors
            if (this.timeToOnDelta >= this.timeToOn)
            {
                this.isOn = true;
                this.shouldTurnOn = false;
                this.timeToOnDelta = 0f;
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

    public void UpdateFireState(bool notify)
    {
        if (this.isOn)
        {
            this.pSystem.Play();
            this.aSource.Play();
            if (notify) this.NotifyAllNeighbors();
        }
        else
        {
            this.pSystem.Stop();
            this.aSource.Stop();
        }
    }

    public void TurnOff() {
        this.UpdateFireState(true);
    }

    public void WaterHit(Vector3 normal) {
        // TODO: IMPLEMENT
    }
}
