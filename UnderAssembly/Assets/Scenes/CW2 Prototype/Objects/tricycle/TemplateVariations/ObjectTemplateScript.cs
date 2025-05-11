using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectTemplateScript : MonoBehaviour
{
    Transform Trigger;
    public string TemplateTitle;
    public string BaseType;
    public List<string> CorrectSocketObject;
    ColourMixerScript colourMixerScript;
    public Color Colour;
    public string ColourName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        colourMixerScript = GameObject.Find("ColourMixer").GetComponent<ColourMixerScript>();
        if (colourMixerScript.dyeCombinations.Count > 0)
        {
            int randcolour = Random.Range(0, colourMixerScript.dyeCombinations.Count);
            Colour = colourMixerScript.dyeCombinations.ElementAt(randcolour).Value;
            ColourName = colourMixerScript.MixableColours[randcolour].name;
        }
        Trigger = transform.Find("Triggers");
        for (int i = 0; i < Trigger.childCount; i++)
        {
            CorrectSocketObject.Add(Trigger.GetChild(i).name);
        }
    }
}
