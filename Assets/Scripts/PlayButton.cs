using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : ButtonClicked
{
    public override void TaskOnClick()
    {
        SceneManager.LoadScene("Game");
    }
}
