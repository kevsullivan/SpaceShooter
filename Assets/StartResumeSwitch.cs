using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartResumeSwitch : MonoBehaviour
{

    public GameObject button;
    public GameObject mainMenu, startScreenButton;
    private int sceneToSet;
    private LoadSceneOnClick sceneSwitcher;

    public void Start()
    {
        sceneSwitcher = new LoadSceneOnClick();
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void StartContext()
    {
        button.GetComponentInChildren<Text>().text = "Start";
    }
   
    public void InGameContext()
    {
        button.GetComponentInChildren<Text>().text = "Resume";
    }

    public void TaskOnClick()
    {
        mainMenu.SetActive(false);
        if (GameManager.instance.inGame)
        {
            GameManager.instance.ContinueGame();
        }
        else
        {
            InGameContext();
            startScreenButton.SetActive(true);
            sceneSwitcher.LoadByIndex(1);
        }
    }
}
