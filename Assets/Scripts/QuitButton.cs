using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : ButtonClicked
{
    public override void TaskOnClick()
    {
        Application.Quit();
    }
}
