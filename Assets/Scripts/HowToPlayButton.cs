using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayButton : ButtonClicked
{
    public Canvas instructionsTab;
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
        instructionsTab.gameObject.SetActive(true);
    }

    public void ExitTabTaskOnClick()
    {
        instructionsTab.gameObject.SetActive(false);
    }
}
