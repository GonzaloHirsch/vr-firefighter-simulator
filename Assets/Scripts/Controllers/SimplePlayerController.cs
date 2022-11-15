using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    // Teleporting stuff
    [Header("Interaction")]
    public float maxDistance = 50;
    [Header("Camera")]
    // Keep track of the camera offset in order to account for it in the raycast
    public GameObject cameraOffset = null;
    // Reference to the gazed at object
    private GameObject gazedAtObject = null;
    // Raycast mask, only checks for collisions on the given layers, saves up computations
    private int raycastMask;

    void Awake()
    {
        // Create mask by setting the explicit tags
        this.raycastMask = LayerMask.GetMask(Constants.TAG_INTERACTABLE);
    }

    void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed at
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, this.raycastMask))
        {
            // GameObject detected in front of the camera.
            if (this.gazedAtObject != hit.transform.gameObject)
            {
                if (this.canSendGazeEvents()) gazedAtObject?.SendMessage("OnPointerExit", null, SendMessageOptions.DontRequireReceiver);
                this.gazedAtObject = hit.transform.gameObject;
                if (this.canSendGazeEvents()) gazedAtObject.SendMessage("OnPointerEnter", null, SendMessageOptions.DontRequireReceiver);
                // New GameObject.
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            if (this.canSendGazeEvents()) gazedAtObject?.SendMessage("OnPointerExit", null, SendMessageOptions.DontRequireReceiver);
            this.gazedAtObject = null;
        }

        // Checks for screen touches.
        if (ActionMapper.GetDevicesTrigger() && this.gazedAtObject != null)
        {
            // Send options to not require receiver just in case, to avoid the error
            gazedAtObject?.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private bool canSendGazeEvents()
    {
        return this.gazedAtObject != null && this.gazedAtObject.CompareTag(Constants.TAG_INTERACTABLE);
    }

    // Debug, dibuja un rayo en la dirección donde está mirando la cámara
    // void OnDrawGizmos()
    // {
    //    Debug.DrawRay(this.transform.position + this.cameraOffset.transform.localPosition, Camera.main.transform.forward, Color.red, maxDistance);
    // }
}
