using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public abstract class ButtonClicked : MonoBehaviour
{
    public Button currentButton;
        
    //    PlayButton;
    //public Button QuitButton;
    //public Button SettingsButton;

    void Start()
    {
        currentButton.onClick.AddListener(TaskOnClick);
        //Button btn = PlayButton.GetComponent<Button>();
        //btn.onClick.AddListener(PlayTaskOnClick);
        //Button quitbtn = QuitButton.GetComponent<Button>();
        //quitbtn.onClick.AddListener(QuitTaskOnClick);
        //Button settingsbtn = SettingsButton.GetComponent<Button>();
        //settingsbtn.onClick.AddListener(ChangeSettingsOnClick);
    }

    public abstract void TaskOnClick();
    //void PlayTaskOnClick()
    //{
    //    SceneManager.LoadScene("Game");
    //}

    //void ChangeSettingsOnClick()
    //{
    //    SceneManager.LoadScene("Settings");
    //}

    //void QuitTaskOnClick()
    //{
    //    Application.Quit();
    //}

}