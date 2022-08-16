using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : ButtonClicked
{
    public override void TaskOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
