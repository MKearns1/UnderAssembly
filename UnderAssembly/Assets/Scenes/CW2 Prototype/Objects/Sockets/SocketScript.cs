using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomSocketInteractor : XRSocketInteractor
{
    public List<string> EnabledObjects;
    
    [Header("Custom Hover Materials")]
    public Material AllowedHoverMaterial;
    public Material ForbiddenHoverMaterial;
    public LayerMask AllowedLayers;
    //[SerializeField] private Material allowedHoverMaterialInstance;
    //[SerializeField] private Material forbiddenHoverMaterialInstance;

    private static bool initialized = false;
    
    protected override void Awake()
    {
        base.Awake();

        // Initialize static materials only once
        //if (!initialized)
        //{
        //    // Ensure that you only assign the static variables the first time
        //    if (allowedHoverMaterialInstance != null)
        //        AllowedHoverMaterial = allowedHoverMaterialInstance;

        //    if (forbiddenHoverMaterialInstance != null)
        //        ForbiddenHoverMaterial = forbiddenHoverMaterialInstance;

        //    initialized = true;
        //}
    }

    
    // Your allowed check
    private bool IsInteractableAllowed(IXRInteractable interactable)
    {
        var obj = interactable.transform.gameObject;
        if (obj.GetComponent<ComponentScript>() != null && (AllowedLayers.value & (1 << obj.layer)) != 0)
        {
            foreach (string name in EnabledObjects)
            {
                if (obj.GetComponent<ComponentScript>().ObjectName == name)
                { return true; }
            }
        }
        if (obj.GetComponent<SprayChargeScript>() != null && (AllowedLayers.value & (1 << obj.layer)) != 0)
        { return true;}

            // Example: allow based on Tag
            return false;
    }

    protected override Material GetHoveredInteractableMaterial(IXRHoverInteractable interactable)
    {
        if (!IsInteractableAllowed(interactable))
            return ForbiddenHoverMaterial;

        return hasSelection ? ForbiddenHoverMaterial : AllowedHoverMaterial;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return IsInteractableAllowed(interactable) &&
               base.CanSelect(interactable) &&
               ((!hasSelection && !interactable.isSelected) ||
                (IsSelecting(interactable) && interactable.interactorsSelecting.Count == 1));
    }


    public override bool CanHover(IXRHoverInteractable interactable)
    {
        var selectable = interactable as IXRSelectInteractable;
        if (selectable != null)
        {
            var go = selectable.transform.gameObject;

            if ((AllowedLayers.value & (1 << go.layer)) == 0)
                return false;

            Debug.Log(go.layer);
            foreach (var interactor in selectable.interactorsSelecting)
            {
                // If another socket is selecting it, don't allow hover
                if (interactor is XRSocketInteractor && interactor != this)
                    return false;
            }
            
        }

        return base.CanHover(interactable);
    }




}
