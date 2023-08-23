using UnityEngine;
using System.Collections.Generic;

public class WheelDatas
{
    public Transform visual;
}

public class CarMover : MonoBehaviour
{
    public List<WheelDatas> wheels = new List<WheelDatas>();

    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    public float moveSpeed = 10f; // Vitesse de déplacement
    public float rotationAngle = 60f; // Angle de rotation des roues avant
    public float rotationMove = 60f; // Angle de rotation des roues arrière

    private float initialRotationY; // Rotation y initiale des roues avant
    private float initialRotationX; // Rotation x initiale des roues arrière

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotationY = frontLeftWheel.localRotation.eulerAngles.y;
        initialRotationX = rearLeftWheel.localRotation.eulerAngles.x;
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");

        // Faites tourner les roues avant en fonction de l'input horizontal
        float rotation = rotationInput * rotationAngle + initialRotationY;
        frontLeftWheel.localRotation = Quaternion.Euler(0, rotation, 0);
        frontRightWheel.localRotation = Quaternion.Euler(0, rotation, 0);

        // Faites tourner les roues arrière en fonction de l'input vertical (W ou S)
        float rearRotationInput = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            rearRotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rearRotationInput = 1f;
        }

        float rearRotation = rearRotationInput * rotationMove + initialRotationX;
        rearLeftWheel.localRotation = Quaternion.Euler(rearRotation, 0, 0);
        rearRightWheel.localRotation = Quaternion.Euler(rearRotation, 0, 0);

        // Calcule la direction de rotation (gauche/négatif ou droite/positif)
        int rotationDirection = 0;
        if (rotationInput < 0)
        {
            rotationDirection = -1;
        }
        else if (rotationInput > 0)
        {
            rotationDirection = 1;
        }

        // Tourner la voiture en fonction de la direction de rotation
        transform.Rotate(Vector3.up, rotationDirection * rotationAngle * Time.deltaTime);

        // Déplacez la voiture vers l'avant ou l'arrière en fonction de l'input vertical
        Vector3 moveDirection = transform.forward * forwardInput * moveSpeed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    public void AttachWheel(Transform visual)
    {
        wheels.Add(new WheelDatas
        {
            visual = visual,
        });
    }

    private void AssignWheels()
    {
        Transform wheelsVisual = transform.Find("WheelsVisual");
        if (wheelsVisual != null)
        {
            frontLeftWheel = wheelsVisual.Find("FrontLeftWheel");
            frontRightWheel = wheelsVisual.Find("FrontRightWheel");
            rearLeftWheel = wheelsVisual.Find("RearLeftWheel");
            rearRightWheel = wheelsVisual.Find("RearRightWheel");
        }
    }
}
