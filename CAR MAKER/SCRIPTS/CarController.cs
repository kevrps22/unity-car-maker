using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WheelData
{
    public WheelCollider collider;
    public Transform visual;
    public bool isSteering;
    public bool isMotor;
}

public class CarController : MonoBehaviour
{
    public List<WheelData> wheels = new List<WheelData>();
    public CarSettings carSettings;

    private Rigidbody rb;
    private float speed = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = carSettings.mass;
        rb.drag = carSettings.drag;
        rb.centerOfMass = carSettings.centerOfMass;

        foreach (WheelData wheel in wheels)
        {
            wheel.collider.mass = carSettings.mass / wheels.Count; // Distribute mass among wheels
        }
    }

    private void UpdateWheelVisuals(WheelData wheel)
    {
        Vector3 position;
        Quaternion rotation;
        wheel.collider.GetWorldPose(out position, out rotation);
        wheel.visual.position = position;
        wheel.visual.rotation = rotation;
    }

    public void AttachWheel(WheelCollider collider, Transform visual, bool isSteering, bool isMotor)
    {
        wheels.Add(new WheelData
        {
            collider = collider,
            visual = visual,
            isSteering = isSteering,
            isMotor = isMotor
        });
    }

    private void FixedUpdate()
    {
        speed = rb.velocity.magnitude;
        float motor = carSettings.motorTorque * Input.GetAxis("Vertical");
        float steering = carSettings.steeringAngle * Input.GetAxis("Horizontal");

        foreach (WheelData wheel in wheels)
        {
            if (wheel.isSteering)
            {
                wheel.collider.steerAngle = steering;
            }

            if (wheel.isMotor)
            {
                wheel.collider.motorTorque = motor;
            }
        }
    }
}

[System.Serializable]
public class CarSettings
{
    public float mass = 1200f;
    public float drag = 0.2f;
    public Vector3 centerOfMass = new Vector3(0, -0.5f, 0);
    public float motorTorque = 500f;
    public float steeringAngle = 30f;
    public float handBrake = 1500f;
}
