using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceScript : MonoBehaviour
{
    Light[] FireLights;
    public float flickerSpeed = 2;
    public float LowestLightIntensity;
    public List<GameObject> ObjectsInFurnace = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        FireLights = new Light[transform.Find("FireLights").transform.childCount];

       for (int t =0; t < transform.Find("FireLights").transform.childCount;t++)
        {
            FireLights[t] = transform.Find("FireLights").GetChild(t).GetComponent<Light>();
        }
            
      // flickerSpeed = flickerSpeed = Random.Range(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Light light in FireLights)
        {
            light.intensity = (Mathf.Sin(Time.time * flickerSpeed)*LowestLightIntensity) + LowestLightIntensity*2;
            //light.intensity += 2;
        }
        //Debug.Log(Mathf.Sin(Time.time * FireFlareSpeed));

        if (Time.frameCount % 60 == 0) // Every ~5 seconds at 60fps
        {
           // flickerSpeed = Random.Range(FireFlareSpeed, FireFlareSpeed + 2f);
        }

        for (int i = 0; i < ObjectsInFurnace.Count; i++)
        {
            if (ObjectsInFurnace[i] == null)ObjectsInFurnace.RemoveAt(i);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!ObjectsInFurnace.Contains(other.gameObject))
        {
            ObjectsInFurnace.Add(other.gameObject);
            StartCoroutine(DestroyAfterDelay(other.gameObject,10f));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ObjectsInFurnace.Remove(other.gameObject);

    }
    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (ObjectsInFurnace.Contains(obj))
        {
            ObjectsInFurnace.Remove(obj);
            Destroy(obj);
        }
    }
}
