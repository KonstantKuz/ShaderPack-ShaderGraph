using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal pairPortal;
    [SerializeField] private Camera portalCamera;
    [SerializeField] private Collider collider;
    private readonly Vector3 reflectedScale = new Vector3(-1,1,-1);
    [SerializeField] private Vector3 directionOffset;

    public Vector3 viewForward
    {
        get { return portalCamera.transform.forward; }
    }
    
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdatePortal;
    }
    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdatePortal;
    }

    private void OnTriggerEnter(Collider other)
    {
        pairPortal.DisableCol();

        Vector3 relativePosition = transform.InverseTransformPoint(other.transform.position);
        relativePosition.x = -relativePosition.x;
        other.transform.position = pairPortal.transform.TransformPoint(relativePosition);
        // Vector3 offset = other.transform.position - transform.position;
        // offset.y = -offset.y;
        // other.transform.position = pairPortal.transform.position - offset;
        other.transform.forward = pairPortal.viewForward;

        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            float velocity = rigidbody.velocity.magnitude;
            float angularVelocity = rigidbody.angularVelocity.magnitude;
            rigidbody.velocity = pairPortal.viewForward * velocity;
            rigidbody.angularVelocity = pairPortal.viewForward * angularVelocity;
        }
    }

    public void DisableCol()
    {
        StartCoroutine(DisableCol());

        IEnumerator DisableCol()
        {
            collider.enabled = false;
            yield return new WaitForSeconds(0.2f);
            collider.enabled = true;
        }
    }

    private void UpdatePortal(ScriptableRenderContext context, Camera camera)
    {
        if (camera.cameraType == CameraType.Game && camera.tag == "MainCamera")
        {
            portalCamera.projectionMatrix = camera.projectionMatrix;

            Vector3 relativePosition = transform.InverseTransformPoint(camera.transform.position);
            relativePosition = Vector3.Scale(relativePosition, reflectedScale);
            portalCamera.transform.position = pairPortal.transform.TransformPoint(relativePosition);
        
            Vector3 relativeRotation = transform.InverseTransformDirection(camera.transform.forward + directionOffset);
            relativeRotation = Vector3.Scale(relativeRotation, reflectedScale);
            portalCamera.transform.forward = pairPortal.transform.TransformDirection(relativeRotation);
        }
    }
}
