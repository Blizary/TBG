using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PageOption
{
    [TextArea]
    public string optionText;
    public Page nextPage;

    [Header("Resources")]
    public int healthEffects;
    public int infoEffects;

    [Header("Animations")]
    public TriggersAni triggerA;
}
