using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public float movingSpeed = 0f;
    public Vector3 movingDir = Vector3.zero;
    public Vector3 rotationSpeed = new Vector3();
    private float minSpeed = 1f;
    private float maxSpeed = 10f;
    private float maxRotation = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Random moving speed
        movingSpeed = Random.Range(minSpeed, maxSpeed);
        // Random moving direction
        movingDir = Random.onUnitSphere;
        // Random Rotation speed
        rotationSpeed = new Vector3(
            Random.Range(-maxRotation, maxRotation),
            Random.Range(-maxRotation, maxRotation),
            Random.Range(-maxRotation, maxRotation));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (PersistentSettings.Instance.stage)
        {
            case 1:
                // no movement
                break;
            case 2:
                // Only displacement
                rb.MovePosition(transform.position + movingDir * movingSpeed * Time.fixedDeltaTime);
                break;
            case 3:
                // Translation and Rotation around Y
                rb.MovePosition(transform.position + movingDir * movingSpeed * Time.fixedDeltaTime);
                transform.Rotate(new Vector3(0, 0, rotationSpeed.z) * Time.deltaTime, Space.Self);
                break;
            default:
                // TRanslation and rotation around Y and Z
                rb.MovePosition(transform.position + movingDir * movingSpeed * Time.fixedDeltaTime);
                transform.Rotate(new Vector3(0, rotationSpeed.y, rotationSpeed.z) * Time.deltaTime, Space.Self);
                break;
        }
        
    }
}
