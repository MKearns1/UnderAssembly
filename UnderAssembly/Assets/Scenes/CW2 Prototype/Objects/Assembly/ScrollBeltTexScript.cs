using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBeltTexScript : MonoBehaviour
{
    AssemblyScript Assembly;

    // Start is called before the first frame update
    void Start()
    {
        Assembly = GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ScrollTexture(GetComponent<Renderer>(), Assembly.AssemblySpeed/20, Vector2.right);

    }

    void ScrollTexture(Renderer renderer, float speed, Vector2 direction)
    {
        Material mat = renderer.material; // This creates an instance so each object is unique
        Vector2 offset = mat.GetTextureOffset("_BaseMap");
        offset += direction.normalized * speed * Time.deltaTime;
        mat.SetTextureOffset("_BaseMap", offset);
    }
}
