using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    MENU,
    VICTORY,
    INSTRUCT,
}
public class Menu : MonoBehaviour
{
    public State state;
    public GameObject guiWin;
    public GameObject guiMenu;
    public GameObject guiInstruct;

    static public StateMachine instance;

    

    private void Start()
    {
        state=State.MENU;
    }

    private void Update()
    {
        guiMenu.SetActive(state==State.MENU);
        guiWin.SetActive(state == State.VICTORY);
        guiInstruct.SetActive(state==State.INSTRUCT);
    }

    public void SetState(State newState)
    {
        state=newState;
    }
    public void OnClickPlay()
    {
       SetState(State.INSTRUCT );
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
