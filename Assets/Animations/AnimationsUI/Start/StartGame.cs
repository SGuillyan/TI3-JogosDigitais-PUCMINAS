using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] bool active = false;

    void FixedUpdate()
    {
        if (active)
        {
            SceneGoTo(scene);
        }
    }

    public void SceneGoTo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
