using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class TypeOfMenu {
    [SerializeField]
    private string Name;    //just for developer

    public List<GameObject> Object;
    public Vector3 Displacement;
    [Range(0, 1)] public float SpeedOfShift;
    public bool IsShift;
    public bool Show;

    [HideInInspector]
    public List<Vector3> ObjectStartPos;
}

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private string Level = "Game";

    public List<TypeOfMenu> MainMenu;

    private void Start()
    {
        //Set start point
        for (int i = 0; i < MainMenu.Count; i++)
        {
            for (int k = 0; k < MainMenu[i].Object.Count; k++)
            {
                MainMenu[i].ObjectStartPos.Add(MainMenu[i].Object[k].transform.position);
            }
        }

        StartMovingPartsOfMenu();
        EnabledPartsOfMenu(true);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(Level);
    }

    public void Stats()
    {

    }
    public void BackFromStats()
    {

    }

    public void Credits()
    {
        //Main menu
        MainMenu[0].IsShift = true;
        MainMenu[0].Show = false;

        //Credits
        MainMenu[2].IsShift = true;
        MainMenu[2].Show = true;
    }
    public void BackFromCredits()
    {
        //Main menu
        MainMenu[2].IsShift = true;
        MainMenu[2].Show = false;

        //Credits
        MainMenu[0].IsShift = true;
        MainMenu[0].Show = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    void MovingPartsOfMenu()
    {
        //Check all types of menu for needed to moving
        for (int i = 0; i < MainMenu.Count; i++)
        {
            if (MainMenu[i].IsShift)
            {
                //Moving all objects in type of menu
                for (int k = 0; k < MainMenu[i].Object.Count; k++)
                {
                    //Showing
                    if (MainMenu[i].Show)
                    {
                        MainMenu[i].Object[k].transform.position = Vector3.Lerp(MainMenu[i].Object[k].transform.position, MainMenu[i].ObjectStartPos[k], MainMenu[i].SpeedOfShift);
                        if (Vector2.Distance(MainMenu[i].Object[k].transform.position, MainMenu[i].ObjectStartPos[k]) < 0.01f)
                        {
                            MainMenu[i].IsShift = false;
                            MainMenu[i].Show = true;
                        }
                    }
                    //Hidding
                    else
                    {
                        MainMenu[i].Object[k].transform.position = Vector3.Lerp(MainMenu[i].Object[k].transform.position, MainMenu[i].ObjectStartPos[k] + MainMenu[i].Displacement, MainMenu[i].SpeedOfShift);
                        if (Vector2.Distance(MainMenu[i].Object[k].transform.position, MainMenu[i].ObjectStartPos[k] + MainMenu[i].Displacement) < 0.01f)
                        {
                            MainMenu[i].IsShift = false;
                            MainMenu[i].Show = false;
                        }
                    }
                }
            }
        }
    }

    //Used only in start
    void StartMovingPartsOfMenu()
    {
        //Check all types of menu for needed to moving
        for (int i = 0; i < MainMenu.Count; i++)
        {
            if (MainMenu[i].IsShift)
            {
                //Moving all objects in type of menu
                for (int k = 0; k < MainMenu[i].Object.Count; k++)
                {
                    //Showing
                    if (MainMenu[i].Show)
                    {
                        MainMenu[i].Object[k].transform.position = MainMenu[i].ObjectStartPos[k];

                        MainMenu[i].IsShift = false;
                        MainMenu[i].Show = true;
                    }
                    //Hidding
                    else
                    {
                        MainMenu[i].Object[k].transform.position = MainMenu[i].ObjectStartPos[k] + MainMenu[i].Displacement;

                        MainMenu[i].IsShift = false;
                        MainMenu[i].Show = false;
                    }
                }
            }
        }
    }

    //Used only in start
    void EnabledPartsOfMenu(bool IsEnabled)
    {
        for (int i = 0; i < MainMenu.Count; i++)
        {
            for (int k = 0; k < MainMenu[i].Object.Count; k++)
            {
                MainMenu[i].Object[k].SetActive(true);
            }
        }
    }

    private void Update()
    {
        MovingPartsOfMenu();
    }
}
