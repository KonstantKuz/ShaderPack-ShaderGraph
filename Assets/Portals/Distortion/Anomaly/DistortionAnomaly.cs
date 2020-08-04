using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Rendering;

public class DistortionAnomaly : MonoBehaviour
{
    [SerializeField] private Transform anomalyModel;
    [SerializeField] private Camera refractionCamera;
    [SerializeField] private Light light;
    [SerializeField] private float lightIntensity;
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdateAnomaly;
    }
    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateAnomaly;
    }
    
    private void UpdateAnomaly(ScriptableRenderContext context, Camera camera)
    {
        
        if (camera.cameraType == CameraType.Game && camera.tag == "MainCamera")
        {
            HandleRefractionCamera(camera.transform.position);
            HandleModel(camera.transform.position);
            LightSetActive(true);
            HandleLightIntensity();
        }
        else
        {
            LightSetActive(false);
        }
    }

    private void HandleRefractionCamera(Vector3 mainCamPosition)
    {
        if (refractionCamera == null)
        {
            return;
        }
        
        refractionCamera.transform.position = mainCamPosition;
        refractionCamera.transform.rotation = Quaternion.LookRotation(transform.position-refractionCamera.transform.position);
    }

    private void HandleModel(Vector3 mainCamPosition)
    {
        anomalyModel.transform.rotation = Quaternion.LookRotation(mainCamPosition - transform.position);
    }

    private void LightSetActive(bool value)
    {
        if (light.enabled != value)
        {
            light.enabled = value;
        }
    }

    private void HandleLightIntensity()
    {
        float remapedSin = (float) Math.Sin(Time.time);
        remapedSin = Remap(remapedSin, -1, 1, 1, 2);
        light.intensity = remapedSin * lightIntensity;
    }
 
    public float Remap (float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
