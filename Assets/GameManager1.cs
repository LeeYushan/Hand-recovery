using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public void OnStartGame(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
