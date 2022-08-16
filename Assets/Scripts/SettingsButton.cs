using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : ButtonClicked
{
    public override void TaskOnClick()
    {
        SceneManager.LoadScene("Settings");
    }   
}
