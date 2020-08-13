using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringEffect : MonoBehaviour
{
    [SerializeField] private float firingProgressFrom;
    [SerializeField] private float firingProgressTo;
    [SerializeField] private float firingTime;
    [SerializeField] private ParticleSystem fireEmbers;
    [SerializeField] private MeshRenderer renderer;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Fire();
    }
    
    [ContextMenu("Fire")]
    public void Fire()
    {
        fireEmbers.Play();
        StartCoroutine(Fire());
        IEnumerator Fire()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            float effectTime = 0;
            while (effectTime < firingTime)
            {
                yield return new WaitForEndOfFrame();
                effectTime += Time.deltaTime;
                float dissolveProgress = Mathf.SmoothStep(firingProgressFrom, firingProgressTo, effectTime/firingTime);
                renderer.material.SetFloat("Vector1_E09EAC5D",dissolveProgress);
                fireEmbers.transform.localPosition = Vector3.Lerp(Vector3.up, -Vector3.up, effectTime/firingTime);
            }
        }
    }

    [ContextMenu("ResetEffect")]
    public void ResetEffect()
    {
        fireEmbers.transform.localPosition = Vector3.up;
        renderer.material.SetFloat("Vector1_E09EAC5D",firingProgressFrom);
    }
}
