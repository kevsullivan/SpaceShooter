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
        sceneSwitcher = gameObject.AddComponent<LoadSceneOnClick>();
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
            // Already in game so `resume` is active, just continue game on click
            GameManager.instance.ContinueGame();
        }
        else
        {
            // Button clicked outside of game, i.e. start screen 
            // so set in game context and activate return to start screen button
            InGameContext();
            startScreenButton.SetActive(true);

            // TODO: Loads a specific level, this should be configuratble (for load games etc.)
            sceneSwitcher.LoadByIndex(1);
        }
    }
}
