using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public Scene scene;


    // Start is called before the first frame update
    void Start()
    {
        scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if(Input.GetMouseButton(0))
        {
            if(scene.name == "Title")
            {
                StartGame();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(scene.name == "Tutorial")
            {
                BackToStart();
            }
        }
    }

    void Exit()
    {
        Application.Quit();
    }

    void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    void BackToStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
