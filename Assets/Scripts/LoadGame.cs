using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public void LoadLow()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMiddle()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadHigh()
    {
        SceneManager.LoadScene(3);
    }
}
