using UnityEngine;
using UnityEditor;

public class MouseFollow : MonoBehaviour
{
    [Range(0f, 20f)]
    public float sensitivity = 10f;
    private float pitch;
    private float yaw;
    private Camera cam;
    void Awake()
    {
        this.cam = Camera.main;
    }

    // Taken from https://stackoverflow.com/questions/66248977/camera-follow-player-when-moving-mouse-with-unity-3d
    void Update()
    {
        // Work only in the editor in play mode as a debug
        if (EditorApplication.isPlaying)
        {
            this.yaw += this.sensitivity * Input.GetAxis("Mouse X");
            this.pitch -= this.sensitivity * Input.GetAxis("Mouse Y");
            this.cam.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
        }
    }
}
