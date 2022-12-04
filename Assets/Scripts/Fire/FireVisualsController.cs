using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FireVisualsController : Framework.MonoBehaviorSingleton<FireVisualsController>, IFireDependant
{
    [Header("Flicker")]
    public Vector2 flickerIntensity;
    public float flickerMultiplier = 1f;
    public float timeToFlicker = 0.5f;
    public float flickerDelta = 0f;

    [Header("Sounds")]
    public AudioSource[] audioSources;
    public AudioClip[] breakingSounds;
    public float timeToBreak;
    public float breakDelta = 0f;
    public Vector2 delayLimits;

    [Header("Lights")]
    public Light[] lights;
    public Vector3[] lightPositions;
    public float[] lightIntensities;
    [SerializeField]
    public List<Fire>[] fires;
    public float maxFireDistance = 10f;

    [Header("Smoke")]
    public ParticleSystem[] allSmoke;
    public Vector2 lifetimeSettings;
    private int totalFires;

    [Header("Debug")]
    public bool debug = false;

    void Start()
    {
        // Separate them according to the closest list
        this.fires = new List<Fire>[this.lights.Length];
        // Store the original intensities and positions
        this.lightPositions = new Vector3[this.lights.Length];
        this.lightIntensities = new float[this.lights.Length];
    }

    void Update()
    {
        // Light flickering
        if (this.flickerDelta >= this.timeToFlicker)
        {
            this.flickerDelta = 0f;
            this.FlickerLights();
        }
        this.flickerDelta += Time.deltaTime;
        // Breaking sounds
        if (this.breakDelta >= this.timeToBreak)
        {
            this.breakDelta = 0f;
            this.BreakSounds();
        }
        this.breakDelta += Time.deltaTime;
    }

    public void NotifyFireChange(int fireCount)
    {
        this.DetermineLightPositions();
        this.UpdateSmoke(fireCount);
    }

    /* ------------------------------------------------------------------------------ */
    /* SMOKE HANDLING */
    /* ------------------------------------------------------------------------------ */

    private void UpdateSmoke(int fireCount)
    {
        foreach (ParticleSystem smoke in this.allSmoke)
        {
            var main = smoke.main;
            main.startLifetime = this.lifetimeSettings.x + Mathf.Clamp(fireCount / (float)this.totalFires, 0f, 1f) * (this.lifetimeSettings.y - this.lifetimeSettings.x);
        }
    }

    /* ------------------------------------------------------------------------------ */
    /* SOUND HANDLING */
    /* ------------------------------------------------------------------------------ */

    private void BreakSounds()
    {
        for (int i = 0; i < this.lights.Length; i++)
        {
            // If light is active, play the sound with a random delay
            if (this.lights[i].gameObject.activeInHierarchy)
            {
                StartCoroutine(this.PlaySound(this.audioSources[i], this.breakingSounds[Random.Range(0, this.breakingSounds.Length)], Random.Range(this.delayLimits.x, this.delayLimits.y)));
            }
        }
    }

    IEnumerator PlaySound(AudioSource audioSource, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(clip);
    }

    /* ------------------------------------------------------------------------------ */
    /* FIRE HANDLING */
    /* ------------------------------------------------------------------------------ */

    private void SeparateFires()
    {
        // Get all the fires
        Fire[] allFires = FindObjectsOfType<Fire>();
        // Separate them according to the nearest neighbor
        List<Fire> tmp;
        for (int i = 0; i < this.lights.Length; i++)
        {
            tmp = new List<Fire>();
            foreach (Fire f in allFires)
            {
                if (Vector3.Distance(this.lights[i].transform.position, f.transform.position) <= this.maxFireDistance)
                {
                    tmp.Add(f);
                }
            }
            this.fires[i] = tmp;
        }
    }

    private void DetermineLightPositions()
    {
        for (int i = 0; i < this.lights.Length; i++)
        {
            float newY = 0;
            float newX = 0;
            int on = 0;
            foreach (Fire fire in this.fires[i])
            {
                newY += (fire.isOn ? fire.transform.position.y : 0);
                newX += (fire.isOn ? fire.transform.position.x : 0);
                on += (fire.isOn ? 1 : 0);
            }
            // If no more neighbors are on, turn the light off
            if (this.lights[i].gameObject.activeInHierarchy && on == 0)
            {
                this.lights[i].gameObject.SetActive(false);
            }
            else if (!this.lights[i].gameObject.activeInHierarchy && on > 0)
            {
                this.lights[i].gameObject.SetActive(true);
            }
            // If there are on fires, move position
            if (on > 0)
            {
                this.lights[i].transform.position = new Vector3(newX / this.fires[i].Count, newY / this.fires[i].Count, this.lights[i].transform.position.z);
                this.lightPositions[i] = this.lights[i].transform.position;
            }
        }
    }

    public void InitializeVisuals()
    {
        this.SeparateFires();
        this.DetermineLightPositions();
        this.totalFires = FireController.Instance.GetTotalFires();
    }

    public void FlickerLights()
    {
        Vector3 delta;
        float newIntensity;
        for (int i = 0; i < this.lights.Length; i++)
        {
            delta = new Vector3(Random.Range(0f, 1f) * this.flickerMultiplier, Random.Range(0f, 1f) * this.flickerMultiplier, 0f);
            newIntensity = Random.Range(this.flickerIntensity[0], this.flickerIntensity[1]);
            this.lights[i].transform.position = this.lightPositions[i] + delta;
            this.lights[i].intensity = newIntensity;
        }
    }

    /* ------------------------------------------------------------------------------ */
    /* DEBUG */
    /* ------------------------------------------------------------------------------ */

    void OnDrawGizmosSelected()
    {
        if (this.debug)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            foreach (Light l in this.lights)
            {
                Gizmos.DrawSphere(l.transform.position, this.maxFireDistance);
            }
        }
    }
}
