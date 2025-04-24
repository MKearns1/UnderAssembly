using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SprayGunScript : MonoBehaviour
{

    public InputActionProperty triggerAction;


    private XRGrabInteractable grabInteractable;
    public bool isHeld = false;
    bool inFrontOfGun = false;
    GameObject TargetObj;
    public Color targetColor;
    float colorChangeSpeed = .5f;
    ParticleSystem particles;
    public float Charge;
    float ConsumptionRate = 5;
    GameObject ChargeObj;
    public Transform ChargeInsertionPoint;

    private void Awake()
    {
        grabInteractable = transform.parent.GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        particles = transform.parent.transform.Find("Particle System").GetComponent<ParticleSystem>();
        ChargeObj = transform.parent.Find("Charge").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var main = particles.main;
        main.startColor = targetColor;
        var emission = particles.emission;
        emission.enabled = false;

        float triggerValue = triggerAction.action.ReadValue<float>();
        if (isHeld && inFrontOfGun)
        {
            //Material mat = TargetObj.GetComponent<ObjectBaseScript>().ObjectColourToChange.GetComponent<MeshRenderer>().materials[0];

            if (triggerValue > 0.5f)
            {
                //// Gradually change to the target color while the trigger is pulled
                ////Color currentColor = mat.color;
                //Color currentColor=TargetObj.GetComponent<ObjectBaseScript>().CurrentColour;
                //Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorChangeSpeed);
                //TargetObj.GetComponent<ObjectBaseScript>().CurrentColour = newColor;
                ////  mat.color = newColor;
                //// emission.enabled = true;
                
                
            }

        }

        if (triggerValue > 0.5f && isHeld && Charge > 0)
        {
            emission.enabled = true;
            Charge -= Time.deltaTime *ConsumptionRate;

            if (inFrontOfGun)
            {
                Color currentColor = TargetObj.GetComponent<ObjectBaseScript>().CurrentColour;
                Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorChangeSpeed);
                TargetObj.GetComponent<ObjectBaseScript>().CurrentColour = newColor;
            }
        }
        else
        {
            emission.enabled = false;
        }
        //  Debug.Log(isHeld && inFrontOfGun);

        ChargeObj.transform.localScale = new Vector3(ChargeObj.transform.localScale.x,Charge/2000, ChargeObj.transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.GetComponent<ObjectBaseScript>() != null)
            {
                inFrontOfGun = true;
                TargetObj = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.GetComponent<ObjectBaseScript>() != null)
            {
                inFrontOfGun = false;
                TargetObj = null;
            }
        }
    }
    private void OnDestroy()
    {
        // Prevent memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        Debug.Log("Spray can is now held.");
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        Debug.Log("Spray can is now released.");
    }


    public void Refill(Color newColor)
    {
        ChargeObj.GetComponent<Renderer>().materials[0].color = newColor;
        targetColor = newColor;
        Charge = 100;
    }
}

