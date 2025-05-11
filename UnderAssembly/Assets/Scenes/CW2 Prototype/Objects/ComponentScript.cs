using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ComponentScript : MonoBehaviour
{
    public string ObjectName;
    public GameObject Model;
    public MeshRenderer MeshRenderer;
    public int MaterialIndex;
    public ObjectBaseScript baseObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(baseObject != null)
        {
            SetComponentColour(baseObject);
        }
    }

    public void SetComponentColour(ObjectBaseScript BaseObject)
    {
        if (Model != null)
        {
            MeshRenderer = Model.GetComponent<MeshRenderer>();
            MeshRenderer.materials[MaterialIndex].color = BaseObject.CurrentColour;
        }
    }
}
