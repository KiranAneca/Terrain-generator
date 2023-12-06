using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public void OpenWindow()
    {
        transform.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void CloseWindow()
    {
        transform.GetComponent<CanvasGroup>().alpha = 0;
    }


    public void OnCloseButtonPressed()
    {
        CloseWindow();
    }
}
