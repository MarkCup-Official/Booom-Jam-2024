using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Light2D))]
public class BreathLight : MonoBehaviour
{
    Light2D _light;
    public float Frequency = 1;
    public Color startColor;
    public Color endColor;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.color = startColor + (Mathf.Sin(Time.time) + 1) / 2f * (endColor - startColor);
    }
}
