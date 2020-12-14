using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private KeyCode LEFT = KeyCode.LeftArrow;
    private KeyCode RIGHT = KeyCode.RightArrow;
    private KeyCode UP = KeyCode.UpArrow;
    private KeyCode DOWN = KeyCode.DownArrow;

    private bool left = false;
    private bool right = false;
    private bool up = false;
    private bool down = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(LEFT))
        {
            left = true;
        }
        if(Input.GetKeyUp(LEFT))
        {
            left = false;
        }

        if (Input.GetKeyDown(RIGHT))
        {
            right = true;
        }
        if (Input.GetKeyUp(RIGHT))
        {
            right = false;
        }

        if (Input.GetKeyDown(DOWN))
        {
            down = true;
        }
        if (Input.GetKeyUp(DOWN))
        {
            down = false;
        }

        if (Input.GetKeyDown(UP))
        {
            up = true;
        }
        if (Input.GetKeyUp(UP))
        {
            up = false;
        }
    }
}
