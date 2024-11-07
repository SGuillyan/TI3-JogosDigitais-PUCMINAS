using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ToggleList : MonoBehaviour
{
    [SerializeField] private Toggle[] list;

    private bool active = false;

    private void Start()
    {
        list[0].isOn = true;
        /*for (int i = 0; i < list.Length; i++)
        {
            list[i].onValueChanged.AddListener(() => ChangeSelect(list[i]));
        }*/
    }

    public void VerifySelect(Toggle ignore)
    {
        if (ignore.isOn)
        {
            if (active)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].isOn && list[i] != ignore)
                    {
                        list[i].isOn = false;
                        list[i].interactable = true;
                    }                   
                }
                active = true;
                ignore.interactable = false;
            }
            else
            {
                active = true;
                ignore.interactable = false;
            }
        }
        else
        {
            active = false;
        }        
    }
}
