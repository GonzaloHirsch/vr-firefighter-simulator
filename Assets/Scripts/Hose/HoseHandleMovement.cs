using UnityEngine;

public class HoseHandleMovement : MonoBehaviour
{
    public Vector3 onRotation;
    public Vector3 offRotation;
    public float rotationSpeed;
    private float step;
    private Quaternion onRotationQuat;
    private Quaternion offRotationQuat;
    private Quaternion towardsRotation;
    private bool isTurning;


    void Awake() 
    {
        this.isTurning = false;
        this.step = this.rotationSpeed * Time.deltaTime;
        this.onRotationQuat = Quaternion.Euler(this.onRotation); 
        this.offRotationQuat = Quaternion.Euler(this.offRotation); 
        this.transform.localRotation = this.offRotationQuat;
    }

    void Update() 
    {
        if (isTurning) {
            this.MoveHoseHandle();
        }
    }

    public bool getIsTurning() {
        return this.isTurning;
    }

    public void ActivateHandleMovement(bool turningOn) 
    {
        this.isTurning = true;
        this.towardsRotation = turningOn ? this.onRotationQuat : this.offRotationQuat;
    }

    private void MoveHoseHandle()
    {        
        // Rotate from current rot to the quat rotation slowly
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,  
            this.towardsRotation, 
            this.step
        );

        // When the turning has finalized, set the flag to false to avoid movement
        if (Quaternion.Angle(transform.localRotation, this.towardsRotation) == 0) {
            this.isTurning = false;
        }
    }
}
