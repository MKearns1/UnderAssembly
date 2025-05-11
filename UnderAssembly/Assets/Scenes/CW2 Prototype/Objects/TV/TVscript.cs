using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TVscript : MonoBehaviour
{
    GameObject Canvas;
    GameObject ProductName;
    GameObject ProductIcon;
    GameObject ProductColour;
    GameObject Quota;
    GameObject MainMenu;
    GameObject EvalScreen;
    string productType;
    public Texture2D[] Icons;
    public Texture2D DisplayIcon;
    public Dictionary<string, Texture2D> IconsDict = new Dictionary<string, Texture2D>();

    ObjectTemplateScript Template;
    private ObjectTemplateScript lastTemplate;
    private string lastProductType;
    private float lastTime = -1;

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
        MainMenu = Canvas.transform.Find("MainMenu").gameObject;
        EvalScreen = Canvas.transform.Find("EvaluationScreen").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GeneralScript.Instance.NewProductTemplate != null &&
    GeneralScript.Instance.NewProductTemplate != lastTemplate)
        {
            lastTemplate = GeneralScript.Instance.NewProductTemplate;
            lastTemplate.Colour = GeneralScript.Instance.NewProductTemplate.Colour;
            Template = lastTemplate;

            ProductName.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = Template.TemplateTitle;
            ProductColour.transform.Find("Image").Find("Text").GetComponent<TextMeshProUGUI>().text = Template.ColourName;
            ProductColour.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().color = Template.Colour;
            Debug.Log(Template.Colour);

            if (Template.BaseType != lastProductType)
            {
                lastProductType = Template.BaseType;
                switch (Template.BaseType)
                {
                    case "Tricycle": DisplayIcon = Icons[0]; break;
                    case "MonsterTruck": DisplayIcon = Icons[1]; break;
                    case "Robot": DisplayIcon = Icons[2]; break;
                    case "Jackhammer": DisplayIcon = Icons[3]; break;
                    case "Chair": DisplayIcon = Icons[4]; break;
                }

                Sprite sprite = Sprite.Create(DisplayIcon, new Rect(0, 0, DisplayIcon.width, DisplayIcon.height), new Vector2(0.5f, 0.5f));
                ProductIcon.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = sprite;
            }
        }

        if (GeneralScript.Instance.GameStarted)
        {
            // Timer update — still per frame, but cheap
            time -= Time.deltaTime;
            time = Mathf.Max(time, 0);
            if (Mathf.Abs(time - lastTime) > 0.1f)
            {
                Timer.GetComponent<UnityEngine.UI.Text>().text = FormattedTime(time);
                lastTime = time;
            }
        }
        // Quota update — only if count changes
        string quotaText = GeneralScript.Instance.ProductsMade + " of " + GeneralScript.Instance.Quota;
        if (Quota.GetComponent<UnityEngine.UI.Text>().text != quotaText)
        {
            Quota.GetComponent<UnityEngine.UI.Text>().text = quotaText + "\n<size=29>UNITS COMPLETE</size>";
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            StartGame();
        }

        if(time <= 0)
        {
            GeneralScript.Instance.EndGame();
        }
    }


    string FormattedTime(float time)
    {
        
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formattedTime;
    }

    public void StartGame()
    {
        Debug.Log("Template at StartGame: " + GeneralScript.Instance.NewProductTemplate);
        Debug.Log("Colour: " + GeneralScript.Instance.NewProductTemplate?.Colour);
        Debug.Log("ColourName: " + GeneralScript.Instance.NewProductTemplate?.ColourName);

        MainMenu.SetActive(false);
        Canvas.transform.Find("BG1").gameObject.SetActive(true);
        Canvas.transform.Find("BG2").gameObject.SetActive(true);
        GeneralScript.Instance.StartGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndSession()
    {
        Canvas.transform.Find("BG1").gameObject.SetActive(false) ;
        Canvas.transform.Find("BG2").gameObject.SetActive(false) ;
        EvalScreen.SetActive(true);

        EvalScreen.transform.Find("Performance").Find("Rating").GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GeneralScript.Instance.CalculatePerformance();
        EvalScreen.transform.Find("Errors").Find("Rating").GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GeneralScript.Instance.ErrorsMade.ToString();
        EvalScreen.transform.Find("ComponentsUsed").Find("Rating").GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GeneralScript.Instance.ComponentsUsed.ToString();
        EvalScreen.transform.Find("Cleanliness").Find("Rating").GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GeneralScript.Instance.CalculateCleanliness();
    }
}
