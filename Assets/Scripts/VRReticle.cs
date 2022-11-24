using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VRReticle : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject reticlePrefab;
    private GameObject reticleInstance;
    public float distanceFromCamera;

    void Awake()
    {
        this.mainCamera = Camera.main;
#if UNITY_EDITOR
        this.PlaceReticle();
#endif
#if UNITY_ANDROID
        this.PlaceReticle();
#endif
    }

    void PlaceReticle()
    {
        // Instantiate reticle and make it a child of the camera
        this.reticleInstance = GameObject.Instantiate(this.reticlePrefab, Vector3.zero, Quaternion.identity, this.mainCamera.transform);
        // Set the distance 
        this.reticleInstance.transform.position = this.mainCamera.transform.position + (this.mainCamera.transform.forward.normalized * this.distanceFromCamera);
    }
}
