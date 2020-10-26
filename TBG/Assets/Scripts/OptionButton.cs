using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [HideInInspector]
    public Page nextPage;

    private PageOption option;

    /// <summary>
    /// Called on button pressed
    /// </summary>
    public void NextPage()
    {
        GameObject worldController = GameObject.FindGameObjectWithTag("GameController"); //finds the world controller
        worldController.GetComponent<WorldManager>().UpdatePage(nextPage,option); //calls the update page function


    }

    public void UpdateResources(PageOption _option)
    {
        option = _option;
    }
}
