using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public enum State { Menu, Game, Highscores, GameOver };
    public State state = State.Game;
    public Transform[] canvasStates;

    public GameObject playerPrefab;
    private Fish playerFish;
    private ScoreHandler score;

    public DepthCameraEffects dcE;
    public FishSpawner fishSpawner;

    private void Start()
    {
        canvasStates[(int)state].gameObject.SetActive(true);
    }

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
            if (state == State.Game)
            {
                inputPoint = Camera.main.ScreenToWorldPoint(inputPoint);
                inputPoint.z = 0;
                playerFish.GoTo(inputPoint);
            }
        }

    }

    public void StartGame()
    {
        GameObject player = Instantiate(playerPrefab);
        player.name = "Player"; //prevent it setting to Player(Clone)
        player.transform.position = new Vector3(0, 0, 0);
        ChangeToState(State.Game);
        fishSpawner.DeleteAllFish();
        fishSpawner.playerFish = playerFish = player.GetComponent<Fish>();
        dcE.target = player.transform;
    }

    public void StopGame()
    {
        ChangeToState(State.GameOver);
        fishSpawner.DeleteAllFish();

    }

    public void ChangeToMenu()
    {
        ChangeToState(State.Menu);
    }

    public void ChangeToHighscores()
    {
        ChangeToState(State.Highscores);
    }

    public void ChangeToState(State newState)
    {
        canvasStates[(int)state].gameObject.SetActive(false);
        state = dcE.state = fishSpawner.state = newState;
        canvasStates[(int)state].gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
