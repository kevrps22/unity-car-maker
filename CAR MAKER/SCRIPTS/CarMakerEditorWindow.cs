using UnityEditor;
using UnityEngine;

public class CarMakerEditorWindow : EditorWindow
{
    private string carName = "NewCar";
    private GameObject selectedCar;
    private GameObject[] selectedWheelVisuals = new GameObject[4];
    private float colliderMass = 40.0f;
    private float forwardExtremumSlip = 0.05f;
    private float forwardExtremumValue = 1.0f;
    private float forwardAsymptoteSlip = 2.0f;
    private float forwardAsymptoteValue = 2.0f;
    private float sidewaysExtremumSlip = 0.05f;
    private float sidewaysExtremumValue = 1.0f;

    [MenuItem("Custom/Car Maker")]
    public static void ShowWindow()
    {
        GetWindow<CarMakerEditorWindow>("Car Maker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Car Settings", EditorStyles.boldLabel);
        carName = EditorGUILayout.TextField("Car Name", carName);

        GUILayout.Label("Select Car", EditorStyles.boldLabel);
        selectedCar = EditorGUILayout.ObjectField(selectedCar, typeof(GameObject), true) as GameObject;

        GUILayout.Label("Select Wheel Visuals", EditorStyles.boldLabel);
        selectedWheelVisuals[0] = EditorGUILayout.ObjectField("Front Right Wheel", selectedWheelVisuals[0], typeof(GameObject), true) as GameObject;
        selectedWheelVisuals[1] = EditorGUILayout.ObjectField("Front Left Wheel", selectedWheelVisuals[1], typeof(GameObject), true) as GameObject;
        selectedWheelVisuals[2] = EditorGUILayout.ObjectField("Rear Right Wheel", selectedWheelVisuals[2], typeof(GameObject), true) as GameObject;
        selectedWheelVisuals[3] = EditorGUILayout.ObjectField("Rear Left Wheel", selectedWheelVisuals[3], typeof(GameObject), true) as GameObject;

        GUILayout.Label("Collider Parameters", EditorStyles.boldLabel);
        colliderMass = EditorGUILayout.FloatField("Collider Mass", colliderMass);
        forwardExtremumSlip = EditorGUILayout.FloatField("Forward Extremum Slip", forwardExtremumSlip);
        forwardExtremumValue = EditorGUILayout.FloatField("Forward Extremum Value", forwardExtremumValue);
        forwardAsymptoteSlip = EditorGUILayout.FloatField("Forward Asymptote Slip", forwardAsymptoteSlip);
        forwardAsymptoteValue = EditorGUILayout.FloatField("Forward Asymptote Value", forwardAsymptoteValue);
        sidewaysExtremumSlip = EditorGUILayout.FloatField("Sideways Extremum Slip", sidewaysExtremumSlip);
        sidewaysExtremumValue = EditorGUILayout.FloatField("Sideways Extremum Value", sidewaysExtremumValue);

        if (GUILayout.Button("Generate Car"))
        {
            GenerateCar();
        }
    }

    private void GenerateCar()
    {
        if (selectedCar != null && selectedWheelVisuals.Length == 4)
        {
            MeshCollider carMeshCollider = selectedCar.AddComponent<MeshCollider>();
            carMeshCollider.convex = true;

            Rigidbody carRigidbody = selectedCar.AddComponent<Rigidbody>();

            CarMover carMover = selectedCar.AddComponent<CarMover>();

            GameObject wheelsColliderEmpty = new GameObject("WheelsCollider");
            wheelsColliderEmpty.transform.SetParent(selectedCar.transform);

            foreach (GameObject wheelVisual in selectedWheelVisuals)
            {
                if (wheelVisual != null)
                {
                    GameObject wheelCollider = new GameObject("WheelCollider_" + wheelVisual.name);
                    wheelCollider.transform.SetParent(wheelsColliderEmpty.transform);
                    wheelCollider.transform.position = wheelVisual.transform.position;

                    WheelCollider wheelColliderComponent = wheelCollider.AddComponent<WheelCollider>();
                    // Configure the WheelCollider with collider parameters
                    wheelColliderComponent.mass = colliderMass;
                    WheelFrictionCurve forwardFriction = wheelColliderComponent.forwardFriction;
                    forwardFriction.extremumSlip = forwardExtremumSlip;
                    forwardFriction.extremumValue = forwardExtremumValue;
                    forwardFriction.asymptoteSlip = forwardAsymptoteSlip;
                    forwardFriction.asymptoteValue = forwardAsymptoteValue;
                    wheelColliderComponent.forwardFriction = forwardFriction;
                    WheelFrictionCurve sidewaysFriction = wheelColliderComponent.sidewaysFriction;
                    sidewaysFriction.extremumSlip = sidewaysExtremumSlip;
                    sidewaysFriction.extremumValue = sidewaysExtremumValue;
                    wheelColliderComponent.sidewaysFriction = sidewaysFriction;

                    // Attach a MeshCollider to the wheelVisual GameObject
                    MeshCollider wheelMeshCollider = wheelVisual.AddComponent<MeshCollider>();
                    wheelMeshCollider.convex = true;

                    // Attach a Rigidbody to the wheelCollider GameObject
                    Rigidbody wheelRigidbody = wheelCollider.AddComponent<Rigidbody>();

                    // Attach the wheel visual to the CarMover using AttachWheel
                    carMover.AttachWheel(wheelVisual.transform);
                }
            }
        }
    }
}
