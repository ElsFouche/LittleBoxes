using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.0f;
    [SerializeField] private Axis rotationAxis = Axis.X;

    private Vector3 rotatorAxis;

    private void Awake()
    {
        switch (rotationAxis)
        {
            case Axis.X:
                rotatorAxis = Vector3.right;
                break;
            case Axis.Y:
                rotatorAxis = Vector3.up;
                break;
            case Axis.Z:
                rotatorAxis = Vector3.forward;
                break;
            default:
                rotatorAxis = Vector3.forward;
                break;
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotatorAxis, rotationSpeed);
        transform.rotation = new quaternion(transform.rotation.x % 360.0f, transform.rotation.y % 360.0f, transform.rotation.z % 360.0f, transform.rotation.w);
    }
}
