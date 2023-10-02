using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject player;
    public string playerFunc;

    public string[] tasks;

    int index = 0;
    string message = "";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerFunc = player.GetComponent<Player>().message;

        tasks = new string[6];
        tasks[0] = "Moving";
        tasks[1] = "Floating";
        tasks[2] = "Descending";
        tasks[3] = "Glowing";
        tasks[4] = "Lighting";
        tasks[5] = "Levitating";

        message = "What's going on? Am I a ghost? (Press WASD to move)";
    }

    // Update is called once per frame
    void Update()
    {
        playerFunc = player.GetComponent<Player>().message;

        if(index < tasks.Length)
        {
            if (playerFunc == tasks[index])
            {
                if (index <= tasks.Length)
                {
                    index++;
                }
            }
        }

        //Decide what to say
        if(index == 1)
        {
            message = "I can move as normal, but I can't seem to touch solid matter anymore. I also feel a little lighter..." + '\n' +  "(Press and hold T)";
        }

        if(index == 2)
        {
            message = "Oh my, I can fly as well! I'm not so comfortable with something like this!" + '\n' + "(Press and hold G)";
        }

        if(index == 3)
        {
            message = "I'm coming back down onto the ground. Oh good, that's a relief." + '\n' + "(Press and hold X)";
        }

        if(index == 4)
        {
            message = "I can glow as well. This is a shocking development, but what's this in my hands?" + '\n' +  "(Press Left Ctrl)";
        }

        if(index == 5)
        {
            message = "Will-o-the-wisps! I can create and leave wisps bright enough for others to see! " + '\n' + "What else can I do? I wonder what happens if I click on that barrel...";
        }

        if(index == 6)
        {
            message = "I can levitate objects as I wish now as well. Being a ghost is fascinating, but I have a mission to accomplish." + '\n' + "(Press Esc to leave)";
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        style.fontSize = 30;

        int width = 200;
        int height = 100;
        float x = (Screen.width - width) / 2;
        float y = (Screen.height - height) / 1.05f;

        GUI.Box(new Rect(x,y, width, height), message, style);
    }
}
