using UnityEngine;
using MEC;
using System.Collections.Generic;
using TMPro;


public class MazeDoor : MonoBehaviour
{
    public TextMeshProUGUI timerUI;

    public MazeGoal goal { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null)
            return;

        var p = other.GetComponent<PlayerMovement>();
        if (p.hasKey && !GameValidator.instance.gameOver)
            GameValidator.instance.gameOver = true;
    }

    void StartTimer()
    {
        Timing.RunCoroutine(CheckTimer().CancelWith(gameObject));
    }
    IEnumerator<float> CheckTimer()
    {
        float time = 120f;
        while (!GameValidator.instance.gameOver)
        {
            if(GameValidator.instance.gameStarted)
            {
                time -= .2f;
                int t = Mathf.CeilToInt(time);
                int min = t / 60;
                int sec = t % 60;
                timerUI.text = $"Time left:\n{min}:{(sec > 9 ? sec : $"0{sec}")}";

                if (time <= 0.0f)
                    GameValidator.instance.gameOver = true;
            }
            yield return Timing.WaitForSeconds(.2f);
        }
        goal.ShowGameOverMenu(time);
    }

    void Start()
    {
        StartTimer();
    }

}
