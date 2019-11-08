using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public enum State { Menu, Game };
    public State state = State.Game;

    public Fish playerFish;


    // Update is called once per frame
    void Update()
    {
        //calculate click or touch point
        Vector3 inputPoint = new Vector3();
        bool inputRecv = false;

        if (Input.touchCount > 0)
        {
            inputRecv = true;
            inputPoint = Input.GetTouch(0).position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inputRecv = true;
            inputPoint = Input.mousePosition;
        }

        if (inputRecv)
        {
            inputPoint = Camera.main.ScreenToWorldPoint(inputPoint);
            inputPoint.z = 0;
            playerFish.GoTo(inputPoint);
        }

    }
}
