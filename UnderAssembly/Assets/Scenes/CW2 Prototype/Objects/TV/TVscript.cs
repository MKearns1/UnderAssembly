using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TVscript : MonoBehaviour
{
    GameObject Canvas;
    GameObject ProductName;
    GameObject ProductIcon;
    GameObject ProductColour;
    GameObject Quota;
    string productType;
    public Texture2D[] Icons;
    public Texture2D DisplayIcon;
    public Dictionary<string, Texture2D> IconsDict = new Dictionary<string, Texture2D>();

    GameObject Timer;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = transform.Find("Canvas").gameObject;
        ProductName = Canvas.transform.Find("BG1").Find("ProductName").gameObject;
        ProductIcon = Canvas.transform.Find("BG1").Find("ProductIcon").gameObject;
        ProductColour = Canvas.transform.Find("BG1").Find("ProductColour").gameObject;
        Timer = Canvas.transform.Find("BG2").Find("TimerMain").Find("Text").gameObject;
        Quota = Canvas.transform.Find("BG2").Find("QuotaMain").Find("Text").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ProductName.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = GeneralScript.Instance.NewProductTemplate.TemplateTitle;
        ProductColour.transform.Find("Image").Find("Text").GetComponent<TextMeshProUGUI>().text = GeneralScript.Instance.NewProductTemplate.ColourName;
        ProductColour.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().color = GeneralScript.Instance.NewProductTemplate.Colour;
        productType = GeneralScript.Instance.NewProductTemplate.BaseType;
        

        switch (productType)
        {
            case "Tricycle":
                DisplayIcon = Icons[0];
                break;

            case "MonsterTruck":
                DisplayIcon = Icons[1];
                break;

            case "Robot":
                DisplayIcon = Icons[2];
                break;

            case "Jackhammer":
                DisplayIcon = Icons[3];
                break;

            case "Chair":
                DisplayIcon = Icons[4];
                break;

        }


        Texture2D texture = DisplayIcon; // assuming Icons[2] is a Texture2D
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        ProductIcon.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = sprite;


        time -= Time.deltaTime;
        time = Mathf.Max(time, 0);

        Timer.GetComponent<UnityEngine.UI.Text>().text = FormattedTime(time);

        Quota.GetComponent<UnityEngine.UI.Text>().text = 
            GeneralScript.Instance.ProductsSuccessfullyMade + " of " + GeneralScript.Instance.Quota + "\n<size=29>UNITS COMPLETE</size>";
    }


    string FormattedTime(float time)
    {
        
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formattedTime;
    }
}
