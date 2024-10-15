using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    [SerializeField] private Ambient mainAmbient;
    [SerializeField] private Ambient[] othersAmbients;

    private void ModifyAmbients()
    {
        for (int i = 0; i < othersAmbients.Length; i ++)
        {
            othersAmbients[i].ModifyAmbient(mainAmbient);
        }
    }
}
