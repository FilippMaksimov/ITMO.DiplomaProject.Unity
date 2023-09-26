using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUIRotation : MonoBehaviour
{
    public Transform _camera;

    void LateUpdate()
    {
        transform.LookAt(_camera);
    }
}
