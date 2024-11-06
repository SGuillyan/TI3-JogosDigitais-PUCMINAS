using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ToggleList : MonoBehaviour
{
    [SerializeField] private Toggle[] list;

    private bool active = false;

    /*private void Start()
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].onValueChanged.AddListener(() => ChangeSelect(list[i]));
        }
    }*/

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
                    }                   
                }
                active = true;
            }
            else
            {
                active = true;
            }
        }
        else
        {
            active = false;
        }
        
    }
}
