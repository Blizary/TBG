using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPage",menuName ="Page")]
public class Page : ScriptableObject
{

    [TextArea]
    public string mainText;

    [HideInInspector]
    public int numOfPages;
    public List<PageOption> options;

 
    //public AudioClip pageMusic;
}
