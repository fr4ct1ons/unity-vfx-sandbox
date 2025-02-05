using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float sensitivity = 5f;
    
    Vector3 buffer = Vector3.zero;
    void Update()
    {
        var xRotation = Input.GetAxis("Vertical");
        var yRotation = Input.GetAxis("Horizontal");
        buffer.Set(xRotation * Time.deltaTime * sensitivity, yRotation * Time.deltaTime * sensitivity, 0f);
        transform.localEulerAngles += buffer;
    }
}
