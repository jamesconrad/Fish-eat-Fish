using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private Fish player;
    private InputHandler ih;
    private float startMassX100;

    public UnityEngine.UI.Text text;
    public float maxMass;
    [Tooltip("Ecosystem destroyed when mass > max and time > cutoff")]
    public float cutoffTime;

    public bool victory { get; private set; }
    public float gameTime { get; private set; }
    public float score { get; private set; }


    private void Start()
    {
        ih = FindObjectOfType<InputHandler>();
        text = GetComponent<UnityEngine.UI.Text>();
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fish>();
        startMassX100 = player.m_mass * 100;
        victory = false;
    }

    void Update()
    {
        score = player.m_mass * 100 - startMassX100;
        gameTime += Time.deltaTime;
        text.text = "Score : " + (int)score;
        if (player.m_mass > 15 && gameTime > cutoffTime)
        {
            victory = true;
            ih.StopGame();
        }
    }
}
