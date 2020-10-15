using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    public People startPerson;
    public Page currentPage;
    public float typeTextWaitTime;

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

    [Header("Prefabs")]
    public GameObject optionPrefab;


    private int currentTextPage;
    private bool typing;
    private string newText;
    private AudioSource typewriteSource;

    // Start is called before the first frame update
    void Start()
    {
        gameUIHost.SetActive(false);
        startInfo.SetActive(true);

        personMainText.GetComponent<TextMeshProUGUI>().text = startPerson.description;
        personInfoText.GetComponent<TextMeshProUGUI>().text = startPerson.personInfo;
        interrogationInfo.GetComponent<TextMeshProUGUI>().text = startPerson.information;
        currentPage = startPerson.startPage;
        //Add img when available
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void UpdatePage(Page _newPage)
    {
        currentPage = _newPage;
        ReadPage();

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
        currentPage.numOfPages = currentPage.texts.Count;
        

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

            }
        }
        else
        {
            if (currentPage.ending)
            {
                //endText.SetActive(true);
            }
            else
            {
                Debug.Log("THERE ARE NO OPTIONS FOR THIS PAGE");
            }

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
        yield return new WaitForSeconds(1.5f);
        gameUIHost.SetActive(true);
        
    }
}
