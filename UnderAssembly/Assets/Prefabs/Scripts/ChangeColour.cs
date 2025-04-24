using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hue = Mathf.PingPong(Time.time * speed, 1.0f);
        Color color = Color.HSVToRGB(hue, 1, 1);

        GetComponent<Renderer>().material.color = color;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color ); ;
    }
}
