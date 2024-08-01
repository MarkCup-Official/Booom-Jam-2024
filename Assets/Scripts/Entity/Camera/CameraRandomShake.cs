using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRandomShake : MonoBehaviour
{
    public float ShakeSpeed;
    public float ShakeStrength;
    private Vector3 Center;
    private void Awake()
    {
        Center = transform.position;
    }
    void Update()
    {
     
            float t = (Mathf.PerlinNoise(0, Time.time * ShakeSpeed) - 0.5f) * ShakeStrength;
            float t2 = (Mathf.PerlinNoise(1, Time.time * ShakeSpeed) - 0.5f) * ShakeStrength;
            transform.position = Center + new Vector3(t, t2, 0);
        
    }
}
