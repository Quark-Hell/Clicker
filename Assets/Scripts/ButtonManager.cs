using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class TypeOfMenu {
    [SerializeField]
    private string Name;    //just for developer

    public List<GameObject> Object;
    public Vector3 Displacement;
    [Range(0, 10f)] public float SpeedOfShift;
    public bool IsShift;
    public bool Show;

    [HideInInspector]
    public List<Vector3> ObjectStartPos;
}

public class ButtonManager : MonoBehaviour
{
    public List<TypeOfMenu> MainMenu;

    private void Start()
    {
        //Set start point
        for (int i = 0; i < MainMenu.Count; i++)
        {
            for (int k = 0; k < MainMenu[i].Object.Count; k++)
            {
                MainMenu[i].ObjectStartPos.Add(MainMenu[i].Object[k].transform.localPosition);
            }
        }

        StartMovingPartsOfMenu();
        EnabledPartsOfMenu(true);
    }

    public void NewGame()
    {
        if (Input.touchCount == 1)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void Stats()
    {
        if (Input.touchCount == 1)
        {
            //Main menu
            MainMenu[0].IsShift = true;
            MainMenu[0].Show = false;
            MainMenu[0].Displacement = MainMenu[2].Displacement;

            //Stats
            MainMenu[1].IsShift = true;
            MainMenu[1].Show = true;
        }
    }
    public void BackFromStats()
    {
        if (Input.touchCount == 1)
        {
            //Stats
            MainMenu[1].IsShift = true;
            MainMenu[1].Show = false;

            //Main menu
            MainMenu[0].IsShift = true;
            MainMenu[0].Show = true;
        }
    }

    public void Credits()
    {
        if (Input.touchCount == 1)
        {
            //Main menu
            MainMenu[0].IsShift = true;
            MainMenu[0].Show = false;
            MainMenu[0].Displacement = MainMenu[1].Displacement;

            //Credits
            MainMenu[2].IsShift = true;
            MainMenu[2].Show = true;
        }
    }
    public void BackFromCredits()
    {
        if (Input.touchCount == 1)
        {
            //Credits
            MainMenu[2].IsShift = true;
            MainMenu[2].Show = false;

            //Main menu
            MainMenu[0].IsShift = true;
            MainMenu[0].Show = true;
        }
    }

    public void Exit()
    {
        if (Input.touchCount == 1)
        {
            Application.Quit();
        }
    }

    public void BackToMenu()
    {
        if (Input.touchCount == 1)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    public void Restart()
    {
        if (Input.touchCount == 1)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void MyStatsInGame()
    {
        if (Input.touchCount == 1)
        {
            //Main menu
            MainMenu[0].IsShift = true;
            MainMenu[0].Show = false;

            //Stats
            MainMenu[1].IsShift = true;
            MainMenu[1].Show = true;
        }
    }
    public void BackFromStatsInGame()
    {
        if (Input.touchCount == 1)
        {
            //Stats
            MainMenu[1].IsShift = true;
            MainMenu[1].Show = false;

            //Main menu
            MainMenu[0].IsShift = true;
            MainMenu[0].Show = true;
        }
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
                        MainMenu[i].Object[k].transform.localPosition = Vector3.Lerp(MainMenu[i].Object[k].transform.localPosition, MainMenu[i].ObjectStartPos[k], MainMenu[i].SpeedOfShift * Time.deltaTime);
                        if (Vector3.Distance(MainMenu[i].Object[k].transform.position, MainMenu[i].ObjectStartPos[k]) < 0.001f)
                        {
                            MainMenu[i].IsShift = false;
                            MainMenu[i].Show = true;
                        }
                    }
                    //Hidding
                    else
                    {
                        MainMenu[i].Object[k].transform.localPosition = Vector3.Lerp(MainMenu[i].Object[k].transform.localPosition, MainMenu[i].ObjectStartPos[k] + MainMenu[i].Displacement, MainMenu[i].SpeedOfShift * Time.deltaTime);
                        if (Vector3.Distance(MainMenu[i].Object[k].transform.position, MainMenu[i].ObjectStartPos[k] + MainMenu[i].Displacement) < 0.001f)
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
                        MainMenu[i].Object[k].transform.localPosition = MainMenu[i].ObjectStartPos[k];

                        MainMenu[i].IsShift = false;
                        MainMenu[i].Show = true;
                    }
                    //Hidding
                    else
                    {
                        MainMenu[i].Object[k].transform.localPosition = MainMenu[i].ObjectStartPos[k] + MainMenu[i].Displacement;

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
