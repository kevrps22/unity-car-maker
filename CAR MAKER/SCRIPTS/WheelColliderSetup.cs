using UnityEngine;

public class WheelColliderSetup : MonoBehaviour
{
    public GameObject carControllerObject;
    public GameObject wheelMesh;
    public string wheelName;
    public bool isFrontWheel;

    private WheelCollider AddWheelCollider(GameObject parent, GameObject wheelMeshGO, string name, bool isFrontWheel)
    {
        GameObject wheel = new GameObject();
        wheel.transform.parent = parent.transform;
        wheel.transform.localPosition = wheelMeshGO.transform.localPosition;
        wheel.transform.localRotation = Quaternion.identity;
        wheel.name = name;

        WheelCollider wheelCollider = wheel.AddComponent<WheelCollider>();
        wheelCollider.mass = 40.0f;

        WheelFrictionCurve wfcForward = wheelCollider.forwardFriction;
        wfcForward.extremumSlip = 0.05f;
        wfcForward.extremumValue = 1.0f;
        wfcForward.asymptoteSlip = 2.0f;
        wfcForward.asymptoteValue = 2.0f;
        wheelCollider.forwardFriction = wfcForward;

        WheelFrictionCurve wfcSideways = wheelCollider.forwardFriction;
        wfcSideways.extremumSlip = 0.05f;
        wfcSideways.extremumValue = 1.0f;

        Renderer wheelRenderer = wheelMeshGO.GetComponent<Renderer>();

        if (wheelRenderer)
        {
            wheelCollider.radius = wheelRenderer.bounds.size.y / 2;
        }

        return wheelCollider;
    }
}
