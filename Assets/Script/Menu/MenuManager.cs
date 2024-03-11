using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    ///[Tooltip(?Describe the tip?)]

    public GameObject menuLoading;
    // varibale bound to class not to an object
    public static MenuManager instance;

    public Menu[] menus;
    private void Awake()
    {
        instance = this;
    }
    public void OpenMenu(Menu menu)
    {

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].IsOpen)
                menus[i].Close();
        }
        menu.Open();
    }
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void loading(bool state)
    {
        menuLoading.SetActive(state);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
