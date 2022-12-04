using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    public Vector3 direction;
    private Vector3 initialPosition;

    private float newCoordinate;

    void Start()
    {
        this.initialPosition = this.transform.position;
    }

    void Update()
    {
        this.newCoordinate = Mathf.Sin(Time.time * this.speed) * this.maxDistance;
        this.transform.position = this.initialPosition + this.direction * this.newCoordinate;
    }
}
