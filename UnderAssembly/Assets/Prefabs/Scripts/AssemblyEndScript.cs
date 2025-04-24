using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyEndScript : MonoBehaviour
{
    public GameObject AssemblySpawn;
    public GameObject[] Items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
         Destroy(other.gameObject);

        GameObject CurrentItem = Instantiate(Items[Random.Range(0, Items.Length)], AssemblySpawn.transform.position, Quaternion.identity);

        GameObject.Find("DisplayTitleText").GetComponent<Text>().text = "CURRENT ITEM: " + CurrentItem.name; 
    }
}
