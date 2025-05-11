using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesScript : MonoBehaviour
{
    public bool OnLeftSide;
    public bool OnRightSide;
    Animator animator;
    RecipeBookScript bookScript;
    bool TriggerPressed;

    // Start is called before the first frame update
    void Start()
    {
        OnRightSide = true;
        animator = GetComponent<Animator>();
        bookScript = transform.parent.GetComponent<RecipeBookScript>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = bookScript.triggerAction.action.ReadValue<float>();
        if (triggerValue > .5f && !TriggerPressed)
        {
            if (bookScript.isHeld)
            {
                Debug.Log("PressedTrigger");
                TurnPage();

            }

            TriggerPressed = true;
        }
        else if (triggerValue < .5f && TriggerPressed)
        {
            TriggerPressed = false;
        }

        if (!bookScript.isHeld)
        {
            if (OnLeftSide)
            {
                OnLeftSide = false;
                animator.Play("TurnPageLeft",0,0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Debug.Log("TurningPage");
            TurnPage();
        }
    }

    public void TurnPage()
    {
        if (OnLeftSide)
        {
            OnLeftSide = false;
            animator.SetTrigger("TurnLeft");
            SoundManagerScript.Instance.PlaySound("TurnPage", gameObject, false, 1f);
        }
        if (OnRightSide)
        {
            OnRightSide = false;
            animator.SetTrigger("TurnRight");
            SoundManagerScript.Instance.PlaySound("TurnPage", gameObject, false, 1f);
        }
    }
    public void SetOnLeftSide()
    {
        OnLeftSide = true;
        animator.SetInteger("Side", -1);

    }
    public void SetOnRightSide()
    {
        OnRightSide = true;
        animator.SetInteger("Side", 1);

    }
}
