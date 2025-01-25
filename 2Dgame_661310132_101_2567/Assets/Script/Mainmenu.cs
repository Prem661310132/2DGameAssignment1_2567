using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void Credit()
    {
        SceneManager.LoadSceneAsync("Credit");
    }

    public void Quit()
    {
        Application.Quit();
    }


}
