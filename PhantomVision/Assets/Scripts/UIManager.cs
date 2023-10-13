using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public SceneManager sceneManager;
    public bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        sceneManager = GameObject.Find("SceneManager").GetComponent <SceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialManager != null)
        {
            if(tutorialManager.endTime <= 0.0f)
            {
                end = true; //End the tutorial
            }
        }

        //When the game has reached end, the end text appears on the screen and the game is paused
        if(end)
        {
            sceneManager.EndGame();
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = Color.white;
        style.fontSize = 22;

        int width = 100;
        int height = 100;
        float x = (Screen.width - width) / 6;
        float y = (Screen.height + height) / 12;

        GUI.Box(new Rect(x, y, width, height), "Hold right mouse button to rotate camera" + '\n' + "Scroll to zoom in/out", style);
    }
}
