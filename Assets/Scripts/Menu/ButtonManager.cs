using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private string NewGame = "Game";
    // Start is called before the first frame update

    public void NewGameButton()
    {
        SceneManager.LoadScene(NewGame);
    }

    public void Stats()
    {

    }

    public void Credits()
    {

    }

    public void Exit()
    {

    }
}
