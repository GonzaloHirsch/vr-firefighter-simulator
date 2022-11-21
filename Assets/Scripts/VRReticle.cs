using UnityEngine;
using UnityEditor;

public class VRReticle : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject reticlePrefab;
    private GameObject reticleInstance;
    public float distanceFromCamera;

    void Awake()
    {
        this.mainCamera = Camera.main;
        // Only works if in Android or the editor
        if (Application.platform == RuntimePlatform.Android || EditorApplication.isPlaying)
        {
            // Instantiate reticle and make it a child of the camera
            this.reticleInstance = GameObject.Instantiate(this.reticlePrefab, Vector3.zero, Quaternion.identity, this.mainCamera.transform);
            // Set the distance 
            this.reticleInstance.transform.position = this.mainCamera.transform.position + (this.mainCamera.transform.forward.normalized * this.distanceFromCamera);
            Debug.Log(this.reticleInstance.transform.position);
        }
    }
}
