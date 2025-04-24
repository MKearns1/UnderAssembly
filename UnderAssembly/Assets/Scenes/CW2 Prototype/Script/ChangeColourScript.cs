using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourScript : MonoBehaviour
{
    public int MaterialIndex;
    public Material MainMat;

    // Start is called before the first frame update
    void Start()
    {
        MainMat = GetComponent<Renderer>().materials[MaterialIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColour(Color newColour)
    {
        MainMat.color = newColour;
    }
}
