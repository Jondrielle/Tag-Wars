using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button MainMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        Button menuBtn = MainMenuButton.GetComponent<Button>();
        menuBtn.onClick.AddListener(MainMenuOnClick);
    }

    public void MainMenuOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
