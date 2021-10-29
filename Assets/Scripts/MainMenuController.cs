using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void on_start_game()
    {
        SceneManager.LoadScene("CreateOrJoinRoom");
    }

    public void on_exit()
    {
        Application.Quit();
    }

}
