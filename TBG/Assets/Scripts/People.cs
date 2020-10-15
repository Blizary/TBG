using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPerson", menuName = "Person")]
public class People : ScriptableObject
{
    [Tooltip("Information of the person being interrogated")]
    [TextArea]
    public string personInfo;

    [Tooltip("Description of the person being Interrogated")]
    [TextArea]
    public string description;

    [Tooltip("Information the player is after")]
    [TextArea]
    public string information;

    public Page startPage;

}

