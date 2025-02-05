using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LiquidPlaneCutoff : MonoBehaviour
{
    [SerializeField] private Renderer target;
    [SerializeField] private Transform targetRoot;
    
    private Plane plane;
    [SerializeField] private Mesh previewPlane;
    [SerializeField] private float localTop, localLeftTop;
    [SerializeField] private float localBottom, localLeftBottom;
    [SerializeField] private bool simulatePhysics;
    [SerializeField] private float maxAccumulation = 10f;
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float noiseSpeed;
    
    [SerializeField] private float facingUp;
    [SerializeField] private AnimationCurve angleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    [SerializeField, Range(0f, 1f)] private float fulness = .8f;

    private Vector4 planeVector = Vector4.zero;
    private float bottom, top;
    [SerializeField] private float accumulatedRootRotation;
    private float previousRootRotation;
    private float totalMovement = 0f;
    
    private Material material => Application.isPlaying ? target.material : target.sharedMaterial;
    
    private void Awake()
    {
        if (!target)
        {
            target = GetComponent<Renderer>();
        }
    }

    void Update()
    {
        if (simulatePhysics)
        {
            var current = Mathf.Abs(targetRoot.localEulerAngles.x)
                          + Mathf.Abs(targetRoot.localEulerAngles.y)
                          + Mathf.Abs(targetRoot.localEulerAngles.z);
            accumulatedRootRotation += Mathf.Abs(current - previousRootRotation);
            totalMovement += Mathf.Abs(current - previousRootRotation);
            previousRootRotation = current;
            
            accumulatedRootRotation = Mathf.Clamp(accumulatedRootRotation, 0f, maxAccumulation);
            //Calculate bottom and top
            facingUp = Vector3.Dot(target.transform.up, Vector3.up);
            if (facingUp < 0f)
            {
                bottom = Mathf.Lerp( localLeftBottom, -localTop, -facingUp);
                top = Mathf.Lerp(localLeftTop, -localBottom, -facingUp);
            }
            else
            {
                bottom = Mathf.Lerp( localLeftBottom, localBottom, facingUp);
                top = Mathf.Lerp(localLeftTop, localTop, facingUp);
            }
                
            //Calculate plane position based on fulness
            float newY = Mathf.Lerp(top, bottom, 1f - fulness);
            transform.position = new Vector3(targetRoot.position.x, targetRoot.position.y + newY, targetRoot.position.z);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            float totalTime = Time.time * noiseSpeed;
            if (Application.isPlaying)
            {
                transform.eulerAngles = transform.eulerAngles + new Vector3(
                        Mathf.PerlinNoise(25f + totalTime, 86 + totalTime) * sensitivity *
                        (accumulatedRootRotation / maxAccumulation), 
                        0f,
                        Mathf.PerlinNoise(14f - totalTime, 55 - totalTime)) * sensitivity *
                    (accumulatedRootRotation / maxAccumulation);
            }

            accumulatedRootRotation -= Time.deltaTime;
            accumulatedRootRotation = Mathf.Clamp(accumulatedRootRotation, 0f, maxAccumulation);
            material.SetFloat("_TotalMovement", totalMovement);
            material.SetFloat("_CurrentMovement", accumulatedRootRotation);
        }
        
        plane = new Plane(transform.up, transform.position);
        planeVector.Set(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
        
        material.SetVector("_Plane", planeVector);
        
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireMesh(previewPlane, transform.position, transform.rotation);
        //Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    public void SetFulness(float value)
    {
        fulness = Mathf.Clamp(value, 0f, 1f);
    }
}

/*
0.492
0.155
0.018
*/