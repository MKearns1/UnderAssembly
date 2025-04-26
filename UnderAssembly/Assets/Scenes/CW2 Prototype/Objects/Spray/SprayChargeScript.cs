using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SprayChargeScript : MonoBehaviour
{
    GameObject SprayGun;
    ColourMixerScript Mixer;
    SprayGunScript gunScript;
    GameObject Fluid;
    GameObject MixedFluid;
    ColourMixerScript MixerScript;
    Transform AttatchPoint;
    public Color colour;
    public int redAmount, yellowAmount, blueAmount;
    public List<GameObject> DyeSegments;
    public int DyeAmount;

    public float shakeThreshold = 2.0f; // How "hard" a shake needs to be
    public float mixProgress;
    public float mixRequired; // How much shake progress needed to mix
    private Vector3 lastVelocity;
    public bool isMixed = false;

    private float lastShakeTime;
    private Vector3 lastPosition;

    public XRGrabInteractable grabInteractable;
    public XRBaseInteractor interactor; // Set this when grabbed
    private Vector3 lastControllerPosition;

    // Start is called before the first frame update
    void Start()
    {
        SprayGun = GameObject.FindGameObjectWithTag("SprayGun");
        gunScript = SprayGun.transform.Find("TriggerArea").GetComponent<SprayGunScript>();
        Fluid = transform.GetChild(0).gameObject;
        Mixer = GameObject.Find("ColourMixer").GetComponent<ColourMixerScript>();
        AttatchPoint = transform.Find("AttachPoint");
        MixedFluid = transform.Find("MixedCharge").gameObject;
        MixedFluid.SetActive(false);
        MixerScript = GameObject.Find("ColourMixer").GetComponent<ColourMixerScript>();

        foreach (Transform t in transform.Find("DyeSegments").transform)
        {
            DyeSegments.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Fluid.GetComponent<Renderer>().material.color = colour;

        if (isMixed)
        {
            if (Vector3.Distance(AttatchPoint.position, gunScript.ChargeInsertionPoint.transform.position) < 0.05)
            {
                gunScript.Refill(colour);
                Destroy(gameObject);
            };
            ScrollTexture(MixedFluid.GetComponent<Renderer>(), .25f, Vector2.right+Vector2.down);

        }




        if (interactor == null) return;

        Vector3 currentPos = interactor.transform.position;
        float delta = (currentPos - lastControllerPosition).magnitude;
       // Debug.Log(delta*100);
        //delta *= 100;

        if (delta*100 > shakeThreshold)
        {
            mixProgress += delta;
            lastShakeTime = Time.time;
        }
        Debug.Log(mixProgress);

        lastControllerPosition = currentPos;

        if (mixProgress >= mixRequired && !isMixed)
        {
            MixDye();
            isMixed = true;
        }
    }

    void FixedUpdate()
    {

    }

    public void AddNewDyeSegment(int r, int y, int b)
    {
        Color newColor = Color.red;

        if (r > 0)
        {
            newColor = Color.red;
            redAmount++;
        }
        else if (y > 0)
        {
            newColor = Color.yellow;
            yellowAmount++;
        }
        else if (b > 0)
        {
            newColor = Color.blue;
            blueAmount++;
        }

        DyeSegments[DyeAmount].SetActive(true);
        DyeSegments[DyeAmount].GetComponent<Renderer>().material.color = newColor;
        DyeAmount++;
    }

    public void MixDye()
    {
        MixedFluid.SetActive(true);
        colour = MixerScript.LookUpColour(redAmount, yellowAmount, blueAmount);
        MixedFluid.GetComponent<Renderer>().material.color = colour;
        foreach (GameObject obj in DyeSegments)
        {
            obj.SetActive(false);
        }
        transform.Find("Charge").gameObject.SetActive(false);
        

        isMixed = true;
        Debug.Log(("Mixed"));
    }

    void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
        lastControllerPosition = interactor.transform.position;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        interactor = null;
    }

    void ScrollTexture(Renderer renderer, float speed, Vector2 direction)
    {
        Material mat = renderer.material; // This creates an instance so each object is unique
        Vector2 offset = mat.GetTextureOffset("_BaseMap");
        offset += direction.normalized * speed * Time.deltaTime;
        mat.SetTextureOffset("_BaseMap", offset);
    }
}
