using UnityEngine;

public class RopeHolder : MonoBehaviour
{
    public GameObject ropeEnd;
    public Vector3 relativeParentPosition;

    void Update () {
        this.ropeEnd.transform.position = transform.TransformPoint(this.relativeParentPosition);
    }
}
