using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [SerializeField] GameObject teamMembers, partners, teamMembersTitle, partnersTitle, btn_Next, btn_Previous;

    public void Next()
    {
        partners.SetActive(true);
        partnersTitle.SetActive(true);
        btn_Previous.SetActive(true);

        teamMembers.SetActive(false);
        teamMembersTitle.SetActive(false);
        btn_Next.SetActive(false);
    }

    public void Previous()
    {
        teamMembers.SetActive(true);
        teamMembersTitle.SetActive(true);
        btn_Next.SetActive(true);

        partners.SetActive(false);
        partnersTitle.SetActive(false);
        btn_Previous.SetActive(false);

    }
}
