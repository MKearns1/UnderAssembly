using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourMixerScript : MonoBehaviour
{
    GameObject Slider;
    float DyePullAmount;
    public float MaxDyePullAmount;
    MixerLeverScript Lever;
    ParticleSystem dyeParticles;
    Color DyeColor = Color.red;
    public enum Dye { red, yellow, blue };
    public Dye CurrentDye;
    int SelectedDyeIndex =0;
    float dyeUnitTimer = 0f;
    bool dyeUnitDispensedThisPull = false;

    // Start is called before the first frame update
    void Start()
    {
       GetComponent<Renderer>().material.color= MixRYB(0, 5, 1);
        Lever = transform.Find("PaintMixerLever").GetChild(0).GetComponent<MixerLeverScript>();
        Slider = transform.GetChild(1).transform.Find("Slider").gameObject;
        dyeParticles = transform.Find("Particle System").GetComponent<ParticleSystem>();

        DyePullAmount = MaxDyePullAmount;
    }

    // Update is called once per frame
    void Update()
    {
        var main = dyeParticles.main;
        main.startColor = DyeColor;
        var emission = dyeParticles.emission;
        emission.enabled = false;
        Slider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = DyeColor;
        ColorBlock colorblock =
        Slider.GetComponent<Slider>().colors;
        colorblock.normalColor = DyeColor;
        Slider.GetComponent<Slider>().colors= colorblock;

        if (Lever.LeverDown && DyePullAmount > 0)
        {
            dyeUnitTimer += Time.deltaTime;
            DyePullAmount -= Time.deltaTime;
            emission.enabled =true;

            if (dyeUnitTimer >= 1f && !dyeUnitDispensedThisPull)
            {
                dyeUnitDispensedThisPull = true;

                switch (CurrentDye)
                {
                    case Dye.red:
                        Raycast(1, 0, 0);
                        DyeColor = Color.red;
                        break;
                    case Dye.yellow:
                        Raycast(0, 1, 0);
                        DyeColor = Color.yellow;

                        break;
                    case Dye.blue:
                        Raycast(0, 0, 1);
                        DyeColor = Color.blue;

                        break;
                    default:
                        break;

                }
            }
        }
        else
        {
            dyeUnitTimer = 0;
            dyeUnitDispensedThisPull = false;
            emission.enabled = false;
        }
        Slider.GetComponent<Slider>().value = DyePullAmount;

        GetComponent<Renderer>().material.color = MixRYB(0, 1, 1);

    }


    Color MixRYB(float r, float y, float b)
    {
        // Define RGB equivalents of RYB primaries
        Color red = new Color(1f, 0f, 0f);
        Color yellow = new Color(1f, 1f, 0f);
        Color blue = new Color(0f, 0f, 1f);

        float total = r + y + b;
        if (total == 0) return Color.white;

        // Weighted average of RGB colors based on RYB amounts
        Color result = (red * r + yellow * y + blue * b) / total;
        return result;
    }


        void Raycast(int r, int y, int b)
    {
        Ray ray = new Ray(dyeParticles.transform.position, dyeParticles.transform.forward); // Cast from this object's position forward
        RaycastHit hit;
        Debug.DrawRay(dyeParticles.transform.position, dyeParticles.transform.forward);

        if (Physics.Raycast(ray, out hit, 100f)) // Cast up to 10 units
        {
            if (hit.collider.gameObject.tag == "ColourChargeCap")
            {
                SprayChargeScript sprayScript = hit.collider.transform.parent.GetComponent<SprayChargeScript>();

                if (!sprayScript.isMixed)
                {
                    sprayScript.AddNewDyeSegment(r, y, b);
                    // sprayScript.redAmount += r;
                    //  sprayScript.yellowAmount += y;
                    // sprayScript.blueAmount += b;

                    Debug.Log("Ray hit the target object!");
                    //hit.collider.transform.parent.GetComponent<SprayChargeScript>().colour = ;
                    //sprayScript.colour = MixRYB(sprayScript.redAmount, sprayScript.yellowAmount, sprayScript.blueAmount);
                }
            }

            Debug.Log(hit.collider.gameObject);
        }
    }

    public void ChangeDye()
    {
        switch(SelectedDyeIndex)
        {
            case 0:
                CurrentDye = Dye.red;
                DyeColor = Color.red;
                SelectedDyeIndex++;
                break;

            case 1:
                CurrentDye = Dye.yellow;
                DyeColor = Color.yellow;
                SelectedDyeIndex++;

                break;

            case 2:
                CurrentDye = Dye.blue;
                DyeColor = Color.blue;
                SelectedDyeIndex = 0;

                break;
        }
        
    }



    public Dictionary<string, Color> dyeMixLookup = new Dictionary<string, Color>()
    {
    { "400", new Color(255, 0, 0) }, // Crimson
    { "040", new Color(1.0f, 0.96f, 0.31f) }, // Lemon
    { "004", new Color(0.0f, 0.28f, 0.67f) }, // Cobalt
    { "220", new Color(1.0f, 0.65f, 0.0f) },  // Orange
    { "202", new Color(0.54f, 0.17f, 0.89f) }, // Violet
    { "022", new Color(0.31f, 0.78f, 0.47f) }, // Emerald
    { "112", new Color(0.42f, 0.35f, 0.80f) }, // Slate Blue
    { "121", new Color(0.5f, 0.5f, 0.0f) },    // Olive
    { "211", new Color(0.55f, 0.27f, 0.07f) }, // Brown
    { "103", new Color(0.29f, 0.0f, 0.51f) },  // Indigo
    { "130", new Color(1.0f, 0.76f, 0.14f) },  // Marigold
    { "310", new Color(0.89f, 0.26f, 0.20f) }, // Vermilion
    { "101", new Color(0.5f, 0.0f, 0.13f) },   // Burgundy
    { "111", new Color(0.5f, 0.5f, 0.5f) },    // Neutral Grey
    { "210", new Color(1.0f, 0.75f, 0.8f) }    // Pink
    };

    //void CalculateFinalColor()
    //{
    //    string key = $"{redUnits}{yellowUnits}{blueUnits}";
    //    if (dyeMixLookup.ContainsKey(key))
    //        finalColor = dyeMixLookup[key];
    //    else
    //        finalColor = Color.white;
    //}

    public Color LookUpColour(int r, int y, int b)
    {
        string key = r.ToString() + y.ToString() + b.ToString();
        if (dyeMixLookup.TryGetValue(key, out Color colour))
        {
            return colour;
        }
        else { return MixRYB(r, y, b); }
    }
}
