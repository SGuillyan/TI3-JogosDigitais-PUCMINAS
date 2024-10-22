using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class EditMode : MonoBehaviour
{
    public static bool activeMode = false;

    public static void ChangeActive()
    {
        activeMode = !activeMode;
    }

    public static void SetActive(bool value)
    {
        activeMode = value;
    }
}
