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
    float ConsumptionRate = 5;
    SprayChargeScript ChargeObject;
    public Transform ChargeInsertionPoint;
    GameObject Sound;
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
    }

    // Update is called once per frame
    void Update()
    {
        var main = particles.main;
        main.startColor = targetColor;
        var emission = particles.emission;
        emission.enabled = false;

        float triggerValue = triggerAction.action.ReadValue<float>();

        if (triggerValue > 0.5f && isHeld)
        {
            if (ChargeObject != null)
                if (ChargeObject.ChargeLeft > 0)
                {
                    emission.enabled = true;
                    ChargeObject.ChargeLeft -= Time.deltaTime * ConsumptionRate;

                    if (inFrontOfGun)
                    {
                        Color currentColor = TargetObj.GetComponent<ObjectBaseScript>().CurrentColour;
                        Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorChangeSpeed);
                        TargetObj.GetComponent<ObjectBaseScript>().CurrentColour = newColor;
                    }

                    if (Sound == null)
                        Sound = SoundManagerScript.Instance.PlaySound("SprayGunSound", gameObject, true, .75f);

                }
                else
                {
                    Destroy(Sound);
                }
        }
        else
        {
            emission.enabled = false;
            Destroy(Sound);
        }
        //  Debug.Log(isHeld && inFrontOfGun);

        //ChargeObj.transform.localScale = new Vector3(ChargeObj.transform.localScale.x,Charge/2000, ChargeObj.transform.localScale.z);
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


    public void AddNewCharge(XRSocketInteractor InsertionPoint)
    {
        GameObject InsertedCharge = InsertionPoint.selectTarget.gameObject;
        Color newColor = Color.black;
        if (InsertedCharge != null)
        {
            newColor = InsertedCharge.GetComponent<SprayChargeScript>().colour;
            ChargeObject = InsertedCharge.GetComponent<SprayChargeScript>();

        }
        targetColor = newColor;
        Physics.IgnoreCollision(InsertedCharge.GetComponent<Collider>(),transform.parent.GetComponent<Collider>());
        SoundManagerScript.Instance.PlaySound("AttachSound", gameObject, false, .75f);
    }
    public void RemoveCharge(XRSocketInteractor InsertionPoint)
    {
        ChargeObject = null;
        targetColor = Color.clear;
    }
}

