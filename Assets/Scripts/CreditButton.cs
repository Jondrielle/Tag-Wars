using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditButton : ButtonClicked
{
    public Canvas creditsTab;
    //public RawImage instructions;
    public Button exitTabButton;

    void Awake()
    {
        //instructions = instructionsTab.GetComponent<RawImage>();
        exitTabButton.onClick.AddListener(ExitTabTaskOnClick);
        //settingsbtn.onClick.AddListener(ChangeSettingsOnClick);
    }

    public override void TaskOnClick()
    {
        creditsTab.gameObject.SetActive(true);
    }

    public void ExitTabTaskOnClick()
    {
        creditsTab.gameObject.SetActive(false);
    }
}

