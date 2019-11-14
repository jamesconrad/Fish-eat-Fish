using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameText : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    public ScoreHandler score;

    private void Awake()
    {
        text.text =
            score.victory == true ? "<color=green><size=40>Ecosystem Collapsed</size></color>" : "<color=red><size=40>Game Over</size></color>\n" +
            "\n" +
            "<color=orange><size=30>Score : " + score.score + "</size></color>\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n";
    }
}
