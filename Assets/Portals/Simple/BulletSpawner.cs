using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private float rate;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float spawnOffset;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > rate)
        {
            timer = 0;
            if (Input.GetMouseButton(0))
            {
                Rigidbody bulletRB = Instantiate(bullet.gameObject,transform.position + transform.forward*spawnOffset, Quaternion.identity).GetComponent<Rigidbody>();
                bulletRB.AddForce(transform.transform.forward * forwardSpeed, ForceMode.VelocityChange);
            }
        }
    }
}
