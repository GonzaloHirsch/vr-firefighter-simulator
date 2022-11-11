using UnityEngine;

public class RopeHolder : MonoBehaviour
{
    public GameObject ropeEnd;
    public Vector3 relativeParentPosition;
    public Vector3 relativeParentRotation;

    void Awake() {
        this.SetPosition();
        this.SetRotation();
    }

    void Update () {
        this.SetPosition();
        this.SetRotation();
    }

    void SetPosition() {
        this.ropeEnd.transform.position = transform.TransformPoint(this.relativeParentPosition);
    }
    
    void SetRotation() {
        // this.ropeEnd.transform.rotation = Quaternion.Euler(this.relativeParentRotation);
        this.ropeEnd.transform.up = Camera.main.transform.up;
    }
}
