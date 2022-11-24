using System.Linq;
using UnityEngine;

public class FireController : Framework.MonoBehaviorSingleton<FireController>
{
    [Header("Fires")]
    [SerializeField]
    private int totalLitFires = 0;
    private IFireDependant[] fireDependants;
    private Fire[] fires;
    [Range(0,1)]
    public float initialFireProbability;
    public AudienceController audienceController;
    public float maximumTurnOffLimit;

    void Start() 
    {
        // Get all the fire dependents
        this.fireDependants = FindObjectsOfType<MonoBehaviour>().OfType<IFireDependant>().ToArray();
        // Get all the fires
        this.fires = FindObjectsOfType<Fire>();
        // Initialize the fires to their on/off state
        // Must be done before initializing neighbors to avoid errors
        foreach (Fire fire in this.fires) {
            fire.SetIsOn(Random.Range(0f, 1f) <= this.initialFireProbability);
            if (fire.isOn) this.totalLitFires++;
        }
        // Initialize the fires to find their neighbors
        for (int i = 0; i < this.fires.Length; i++) {
            this.fires[i].FindNeighbors(this.fires, i);
        }
        // Initialize visuals
        FireVisualsController.Instance.InitializeVisuals();
        // Set the start time
        State.Instance.Reset();
        State.Instance.approvalLimit = this.maximumTurnOffLimit;
        State.Instance.hasPlayed = true;
        State.Instance.startTime = Time.time;
    }

    public void FireChanged(int delta) {
        this.totalLitFires += delta;
        this.NotifyAllFireDependants();
        // GAME OVER SITUATION
        if (this.totalLitFires == 0) {
            State.Instance.endTime = Time.time;
            audienceController.StartVictory();
        }
    }

    void NotifyAllFireDependants()
    {
        foreach (IFireDependant fd in this.fireDependants)
        {
            fd.NotifyFireChange(this.totalLitFires);
        }
    }

    public int GetTotalFires() {
        return this.fires.Length;
    }
}
