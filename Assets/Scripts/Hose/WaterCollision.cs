using UnityEngine;
using System.Collections.Generic;

public class WaterCollision : MonoBehaviour
{
    public ParticleSystem splashPrefab;
    private ParticleSystem splash;
    private ParticleSystem pSystem;
    private List<ParticleCollisionEvent> collisionEvents;
    private ParticleCollisionEvent collisionEvent;

    void Awake()
    {
        // Create instance of splash
        this.splash = GameObject.Instantiate(this.splashPrefab);
        this.splash.Stop();
        // Get particle system and initialize list for later
        this.pSystem = this.GetComponent<ParticleSystem>();
        this.collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        // Get collision events
        int numCollisionEvents = this.pSystem.GetCollisionEvents(other, this.collisionEvents);
        // Get component for water interaction
        IWaterInteractable go = other.gameObject.GetComponent<IWaterInteractable>();
        // If the component allows for interaction and there are collisions, send the event
        if (numCollisionEvents > 0)
        {
            this.collisionEvent = this.collisionEvents[0];
            // Only send water hit if there's a gameobject
            if (go != null)
            {
                go.WaterHit(collisionEvent.normal);
            }
            this.HandleSplash(collisionEvent.intersection, collisionEvent.normal);
        }
    }

    void HandleSplash(Vector3 position, Vector3 normal)
    {
        this.splash.gameObject.transform.position = position;
        this.splash.gameObject.transform.up = normal;
        this.splash.Play();
    }
}
