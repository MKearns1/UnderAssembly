using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourMixerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GetComponent<Renderer>().material.color= MixRYB(0, 5, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Color MixRYB(float r, float y, float b)
    {
        // Approximate RYB to RGB mix
        float red = r + y * 0.5f;
        float green = y + b * 0.5f;
        float blue = b + r * 0.25f;

        // Normalize
        float max = Mathf.Max(red, green, blue, 1f);
        return new Color(red / max, green / max, blue / max);
    }



}
