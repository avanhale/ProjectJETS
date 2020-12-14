using UnityEngine;

public class PlaySpaceRelativity : MonoBehaviour
{
    public static Transform transformT;
    public static Transform cameraT;
    private void Awake()
    {
        transformT = transform.parent;
        cameraT = GetComponentInParent<VRTK.VRTK_SDKSetup>().actualHeadset.transform;
    }

    public static void TransformPlaySpaceTo(Transform targetT)
    {
        transformT.position = targetT.position;
        transformT.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(targetT.forward, Vector3.up), Vector3.up);
    }

    public static void TransformCameraTo(Transform targetT, bool includeYPos = false, bool includeXZRot = false)
    {
        RotateCameraTo(targetT.rotation, includeXZRot);
        MoveCameraTo(targetT.position, includeYPos);
    }

    public static void MoveCameraTo(Vector3 targetPosition, bool includeY = false)
    {
        transformT.position = CameraRelativePosition(targetPosition, includeY);
    }

    public static void RotateCameraTo(Quaternion targetRotation, bool includeXZ = false)
    {
        transformT.rotation = CameraRelativeRotation(targetRotation, includeXZ);
    }

    public static Vector3 CameraRelativePosition(Vector3 targetPosition, bool includeY = false)
    {
        Vector3 position = targetPosition - transformT.TransformVector(cameraT.localPosition);
        if (!includeY) position.y = targetPosition.y;
        return position;
    }

    public static Quaternion CameraRelativeRotation(Quaternion targetRotation, bool includeXZ = false)
    {
        Quaternion rotation;
        if (includeXZ) rotation = targetRotation * Quaternion.Inverse(cameraT.localRotation);
        else rotation = CameraRelativeRotationY(targetRotation);
        return rotation;
    }

    static Quaternion CameraRelativeRotationY(Quaternion targetRotation)
    {
        Vector3 cameraProjected = Vector3.ProjectOnPlane(cameraT.forward, Vector3.up);
        Vector3 targetProjected = Vector3.ProjectOnPlane(targetRotation * Vector3.forward, Vector3.up);
        return transformT.rotation * Quaternion.FromToRotation(cameraProjected, targetProjected);
    }

    public static Vector3 RelativeDirection(Vector3 targetDirection)
    {
        return transformT.TransformDirection(targetDirection);
    }
}
