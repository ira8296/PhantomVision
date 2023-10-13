using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject player;
    public string playerFunc;
    public GameObject stairway;
    public GameObject bedroom;
    public GameObject Matt;
    public GameObject Alex;

    public string[] tasks;

    public int index = 0;
    string message = "";

    public float endTime = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Matt = GameObject.Find("Matt");
        Alex = GameObject.Find("Alex");
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

        MCAI Matt_AI = Matt.GetComponent<MCAI>();
        MCAI Alex_AI = Alex.GetComponent<MCAI>();

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
            message = "I can levitate objects as I wish now as well. Being a ghost is fascinating, but I have a mission to accomplish.";
        }

        //Detect when player leaves room -- when they do, play the scream
        string area = stairway.GetComponent<AreaTrigger>().area;
        string sign = stairway.GetComponent<AreaTrigger>().message;

        //Detect when player has entered bedroom
        string bedArea = bedroom.GetComponent<AreaTrigger>().area;
        string bedSign = bedroom.GetComponent <AreaTrigger>().message;

        if(area == "Stairway" && sign == "Player has entered")
        {
            message = "That scream -- it came from upstairs in the bedrooms! I must hurry!";
        }

        if (bedArea == "Bedroom" && bedSign == "Player has entered")
        {
            message = "Alex: Oh my god, Mom has fallen victim to the curse! What are we going to do?" + '\n' + "-They know about the curse, so they're the ones I've been waiting for." + '\n' +  "With my light I should be able to catch their attention.";
        }

        if(Alex_AI.inContact || Matt_AI.inContact)
        {
            message = "Matt: Who or what's that glowing figure? Is she a ghost?" + '\n' + "Alex: This place just keeps getting stranger and stranger.";
            endTime -= Time.deltaTime;
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
