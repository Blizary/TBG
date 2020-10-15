using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Environment
{
    Environment1,
    Environment2,
    Environment3
}

public class WorldManager : MonoBehaviour
{

    public People startPerson;
    public Page currentPage;
    public float typeTextWaitTime;
    public Environment currentEnviroment;
    public GameObject enviromentInfo;

    [Header("UI person Description")]
    public GameObject startInfo;
    public GameObject personMainText;
    public GameObject personImg;
    public GameObject personInfoText;
    public GameObject interrogationInfo;

    [Header("UI Game")]
    public GameObject gameUIHost;
    public GameObject optionHost;
    public GameObject pageDescription;
    public GameObject healtBar;
    public GameObject infoBar;
    public GameObject failScreen;
    public GameObject successScreen;

    [Header("Prefabs")]
    public GameObject optionPrefab;


    [Header("Resources")]
    public float maxHealth;
    public float currentHealth;
    public float requiredInformation;
    public float currentInformation;

    private int currentTextPage;
    private bool typing;
    private string newText;
    private AudioSource typewriteSource;
    



    // Start is called before the first frame update
    void Start()
    {
        currentEnviroment = Environment.Environment1;
        gameUIHost.SetActive(false);
        startInfo.SetActive(true);
        failScreen.SetActive(false);
        successScreen.SetActive(false);

        personMainText.GetComponent<TextMeshProUGUI>().text = startPerson.description;
        personInfoText.GetComponent<TextMeshProUGUI>().text = startPerson.personInfo;
        interrogationInfo.GetComponent<TextMeshProUGUI>().text = startPerson.information;

        //start
        currentPage = startPerson.startPage;
        currentHealth = maxHealth;
        currentInformation = 0;
        healtBar.GetComponent<Image>().fillAmount = 1;
        infoBar.GetComponent<Image>().fillAmount = 0;


        //Add img when available
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnviroment();
    }


    /// <summary>
    /// Typewriter effect for the main text
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeText()
    {
        typing = true;
        //typewriteSource.Play(); //start sound effect for typing
        pageDescription.GetComponent<TextMeshProUGUI>().text = "";

        for (int i = 0; i < currentPage.mainText.Length; i++)
        {
            pageDescription.GetComponent<TextMeshProUGUI>().text += currentPage.mainText[i];
            yield return new WaitForSeconds(typeTextWaitTime);
        }

        typing = false;
        //typewriteSource.Stop(); //stop sound effect
    }




    /// <summary>
    /// Updates the current page and runs the read page function
    /// Called outside
    /// </summary>
    public void UpdatePage(Page _newPage,PageOption _option)
    {
        currentPage = _newPage;
        ReadResources(_option);
        ReadPage();

    }

    void ReadResources(PageOption _option)
    {
        
        //Health
        if(_option.healthEffects!=0)
        {
            float health = currentHealth + _option.healthEffects;
            if(health> maxHealth)
            {
                currentHealth = maxHealth;
                healtBar.GetComponent<Image>().fillAmount =1;
            }
            else if( health <=0)
            {
                //TRIGGER DEATH 
                currentHealth = 0;
                healtBar.GetComponent<Image>().fillAmount = 0;
                failScreen.SetActive(true);
            }
            else
            {
                currentHealth = health;
                healtBar.GetComponent<Image>().fillAmount = currentHealth / maxHealth;
            }

            
        }

        //Information
        if (_option.infoEffects != 0)
        {
            float info = currentInformation + _option.infoEffects;

            if(info >= requiredInformation)
            {
                //TRIGGER WIN 
                info = requiredInformation;
                infoBar.GetComponent<Image>().fillAmount = 1;
                successScreen.SetActive(true);
            }
            else if( info<0)
            {
                currentInformation = 0;
                infoBar.GetComponent<Image>().fillAmount = 0;
            }
            else
            {
                currentInformation = info;
                infoBar.GetComponent<Image>().fillAmount = currentInformation / requiredInformation;
            }

            
        }
        


    }



    /// <summary>
    /// Reads the information on the page and displays it on screen 
    /// </summary>
    public void ReadPage()
    {
        //Trigger fade effect
        //fadePanel.GetComponent<Animator>().SetBool("FadeIn", true);
       // options.SetActive(false);
       // currentTextPage = 0;
        typing = false;
        StartCoroutine(IE_ReadPage());

    }

    /// <summary>
    /// Sets the page number of the text back to 1
    /// </summary>
    void ResetPageOfText()
    {
        pageDescription.GetComponent<TextMeshProUGUI>().pageToDisplay = 1;
    }

    IEnumerator IE_ReadPage()
    {
        yield return new WaitForSeconds(1.5f);

        ResetPageOfText();
        //Change music
        /*
        if ((currentPage.pageMusic != null && backgroundSound != currentPage.pageMusic)) //if sound changed on the page
        {
            backgroundSound = currentPage.pageMusic;
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = backgroundSound;
            //test
            GetComponent<AudioSource>().Play();
        }
        */

        //Change image
        /*
        backgroundImage.GetComponent<Image>().sprite = currentPage.backgroundImage;
        if (currentPage.backgroundImage != null)
        {
            Debug.Log("THERE IS NO BACKGROUND IMAGE FOR THIS PAGE");
        }
        */

        //Change maintext
        StopCoroutine("TypeText");
        currentTextPage = 0;
        StartCoroutine("TypeText");
       
        

        //Clear old options
        foreach (Transform child in optionHost.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //add new options
        if (currentPage.options.Count != 0)
        {
            for (int i = 0; i < currentPage.options.Count; i++)
            {
                GameObject newOption = Instantiate(optionPrefab, optionHost.transform);
                newOption.GetComponent<OptionButton>().nextPage = currentPage.options[i].nextPage;
                newOption.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentPage.options[i].optionText;
                newOption.GetComponent<OptionButton>().UpdateResources(currentPage.options[i]);

            }
        }
        else
        {
            /*
            if (currentPage.ending)
            {
                //endText.SetActive(true);
            }
            else
            {
                Debug.Log("THERE ARE NO OPTIONS FOR THIS PAGE");
            }
            */

        }


       // fadePanel.GetComponent<Animator>().SetBool("FadeIn", false);
    }





    public void StartInterrogation()
    {
        StartCoroutine(IEStartInterrogation());
    }
    IEnumerator IEStartInterrogation()
    {
        yield return new WaitForSeconds(0.1f);
        startInfo.SetActive(false);
        ReadPage();
        yield return new WaitForSeconds(1.3f);
        gameUIHost.SetActive(true);
        
    }

    public void LoseButton()
    {
        SceneManager.LoadScene("Interrogation");
    }

    public void WinButton()
    {
        SceneManager.LoadScene("Interrogation");
    }

    public void UpdateEnviroment()
    {
     switch(currentEnviroment)
        {
            case Environment.Environment1:
                {
                    enviromentInfo.GetComponent<Image>().color = Color.green;
                    break;
                }
            case Environment.Environment2:
                {
                    enviromentInfo.GetComponent<Image>().color = Color.red;
                    break;
                }
            case Environment.Environment3:
                {
                    enviromentInfo.GetComponent<Image>().color = Color.blue;
                    break;
                }
        }
    }

}
