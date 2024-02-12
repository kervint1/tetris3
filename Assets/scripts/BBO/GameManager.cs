using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BBOGameManager : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void Reset()
    {
        SceneManager.LoadScene(2);
    }
}