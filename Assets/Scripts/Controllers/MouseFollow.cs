using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
        // Player must hold right click to enable
#if UNITY_EDITOR
        if (EditorApplication.isPlaying && Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            this.HandleMouseInput();
        }
#endif
    }

    void HandleMouseInput()
    {
        this.yaw += this.sensitivity * Input.GetAxis("Mouse X");
        this.pitch -= this.sensitivity * Input.GetAxis("Mouse Y");
        this.cam.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
    }
}
