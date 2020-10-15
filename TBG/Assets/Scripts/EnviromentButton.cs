using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentButton : MonoBehaviour
{
    public Environment newEnvironment;

    public void UpdateEnviroment()
    {
        GameObject worldController = GameObject.FindGameObjectWithTag("GameController"); //finds the world controller
        worldController.GetComponent<WorldManager>().currentEnviroment = newEnvironment;

    }
}
