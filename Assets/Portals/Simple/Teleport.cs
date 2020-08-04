using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportTo;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 relativePosition = transform.InverseTransformPoint(other.transform.position);
        //relativePosition.x = -relativePosition.x;
        other.transform.position = teleportTo.transform.TransformPoint(relativePosition);
    }
}
