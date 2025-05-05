using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RecipeBookScript : MonoBehaviour
{
    Animator animator;
    bool open;
    bool isHeld;
    private XRGrabInteractable grabInteractable;
    ColourMixerScript colourMixerScript;
    public List<GameObject> pages;
    Transform Pages;
    Transform ColourEntry;
    public InputActionProperty triggerAction;
    public bool isTurningPage;
    bool TriggerPressed;
    public class Page
    {
        string title;
        string mainText;
    }

    // Start is called before the first frame update
    void Start()
    {
        Pages = transform.Find("Pages");
        colourMixerScript = GameObject.Find("ColourMixer").GetComponent<ColourMixerScript>();
        animator = GetComponent<Animator>();

        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        ColourEntry = GameObject.Find("ColourEntry").transform;
        string pagetext = "";

        for (int i = 0;i< colourMixerScript.MixableColours.Count; i++)
        {
            string name = colourMixerScript.MixableColours[i].name.PadRight(20);
            int red = int.Parse(colourMixerScript.MixableColours[i].code[0].ToString());
            int yellow = int.Parse(colourMixerScript.MixableColours[i].code[1].ToString());
            int blue = int.Parse(colourMixerScript.MixableColours[i].code[2].ToString());

            float EntrySpacing = .011f;
            float DropSpacing = .0125f;

            Transform NewEntry = Instantiate(ColourEntry, ColourEntry.parent, true);
            NewEntry.localPosition = ColourEntry.transform.localPosition + Vector3.down * EntrySpacing * i;
            NewEntry.GetChild(0).GetComponent<TextMeshPro>().text = name;

            Transform Drop = NewEntry.GetChild(1);
            //pagetext = pagetext + colourMixerScript.MixableColours[i].name;
            //pagetext += name + "| ";
            int Dropindex = 0;
            for (int j = 0; j < red; j++)
            {
                Transform NewDrop = Instantiate(Drop, NewEntry, true);
                NewDrop.localPosition += Vector3.right * DropSpacing * Dropindex;
                NewDrop.GetComponent<Renderer>().material.color = UnityEngine.ColorUtility.TryParseHtmlString("#A60010", out var color) ? color : Color.white;
                Dropindex++;
                //pagetext += "<b><color=#FF0000> * </color></b>";
            }
            for (int j = 0; j < yellow; j++)
            {
                Transform NewDrop = Instantiate(Drop, NewEntry, true);
                NewDrop.localPosition += Vector3.right * DropSpacing *Dropindex;
                NewDrop.GetComponent<Renderer>().material.color = UnityEngine.ColorUtility.TryParseHtmlString("#FFF800", out var color) ? color : Color.white;
                Dropindex++;

                //  pagetext += "<b><color=#FFFF00> * </color></b>";
            }
            for (int j = 0; j < blue; j++)
            {
                Transform NewDrop = Instantiate(Drop, NewEntry, true);
                NewDrop.localPosition += Vector3.right * DropSpacing * Dropindex;
                NewDrop.GetComponent<Renderer>().material.color = UnityEngine.ColorUtility.TryParseHtmlString("#003A99", out var color) ? color : Color.white; 
                Dropindex++;

                //pagetext += "<b><color=#0000FF> * </color></b>";
            }

            Destroy(Drop.gameObject);

           // ColourEntry = NewEntry;
           // pagetext += "\n";
        }
        Destroy(ColourEntry.gameObject);

        //GameObject.Find("PageText").GetComponent<TextMeshPro>().text = pagetext;
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        if(triggerValue > .5f && !TriggerPressed)
        { 
            if(isHeld)
            {
                Debug.Log(isTurningPage);

                if (Pages.GetComponent<Animator>().GetInteger("Side") == -1 && !isTurningPage)
                {
                    isTurningPage = true;
                    TurnPage(1);
                }
                else if (!isTurningPage)
                {
                    isTurningPage = true;
                    TurnPage(-1);
                }
            }
            Debug.Log(Pages.GetComponent<Animator>().GetInteger("Side"));
            Debug.Log(isTurningPage);
                TriggerPressed = true; 
        }
        else if (triggerValue <.5f && TriggerPressed)
        {
            TriggerPressed = false;
        }

        if (isHeld)
        {
            if (!open)
            {
                animator.SetBool("Open", true);
                open = true;
                Debug.Log("OPpen");
            }
            else
            {
                if (TriggerPressed)
                {
                 
                   
                }
            }
            Pages.gameObject.SetActive(true);

        }
        else
        {
            if (open)
            {
                animator.SetBool("Open", false);
                open = false;
                Debug.Log("OPpen");
            }
            Pages.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TurnPage(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TurnPage(1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Pages.GetComponent<Animator>().GetInteger("Side") == -1)
            {
                TurnPage(-1);
            }
            else
            {
                TurnPage(1);
            }
        }
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

    void TurnPage(int Dir)
    {
        if (Dir == 1)
        {
          //  Pages.GetComponent<Animator>().ResetTrigger("TurnRight");
            Pages.GetComponent<Animator>().SetTrigger("TurnRight");
            Pages.GetComponent<Animator>().ResetTrigger("TurnLeft");
            Pages.GetComponent<Animator>().SetInteger("Side",-1);
            //Pages.GetChild(2).gameObject.active = false;

        }
        else if (Dir == -1)
        {
          //  Pages.GetComponent<Animator>().ResetTrigger("TurnLeft");
            Pages.GetComponent<Animator>().SetTrigger("TurnLeft");
            Pages.GetComponent<Animator>().ResetTrigger("TurnRight");
            Pages.GetComponent<Animator>().SetInteger("Side", 1);

           // Pages.GetChild(0).gameObject.SetActive(true);

        }
    }

    public void TurnPageComplete()
    {
        Debug.Log("SDFKJSHDJFHSKJDFHS");
        isTurningPage = false;
    }
    void HidePageText(int Direction)
    {
        if ((Direction == 1))
        {
            //Pages.GetChild(0).gameObject.SetActive(false);
        }
        else if ((Direction == -1))
        {
          //  Pages.GetChild(2).gameObject.SetActive(false);
        }
    }
}
