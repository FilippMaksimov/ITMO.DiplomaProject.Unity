using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private Text greetingText;

    public string user_name;
    
    private void Awake()
    {
        user_name = PlayerDataHolder.user_name;
        Cursor.lockState = CursorLockMode.None;
    }
    void Start()
    {
        Cursor.visible = true;
        greetingText.text = "Welcome, " + user_name;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
