using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceholderUI : MonoBehaviour
{
    public GameObject configMenu;
    public GameObject creditsMenu;
    public GameObject idsMenu;

    public void ShowConfig()
    {
        if (configMenu.activeSelf)
            configMenu.SetActive(false);
        else
            configMenu.SetActive(true);
    }

    public void ShowCredits()
    {
        if (creditsMenu.activeSelf)
            creditsMenu.SetActive(false);
        else
            creditsMenu.SetActive(true);
    }

    public void ShowIDS()
    {
        if (idsMenu.activeSelf)
            idsMenu.SetActive(false);
        else
            idsMenu.SetActive(true);
    }
}
