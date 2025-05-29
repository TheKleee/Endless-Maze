using UnityEngine;
using MEC;
using System.Collections.Generic;
using TMPro;


public class MazeDoor : MonoBehaviour
{
    bool GameOver = false;

    public TextMeshProUGUI timerUI;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null)
            return;

        var p = other.GetComponent<PlayerMovement>();
        if (p.hasKey && !GameOver)
        {
            DBManager.instance.winner();
        }
    }

    void StartTimer()
    {
        Timing.RunCoroutine(CheckTimer().CancelWith(gameObject));
    }
    IEnumerator<float> CheckTimer()
    {
        float time = 120f;
        while (!GameOver)
        {
            time -= .2f;
            timerUI.text = "Time left:\n" + (int)time;
            yield return Timing.WaitForSeconds(.2f);

            if (time <= 0.0f)
                GameOver = true;
        }

        DBManager.instance.loser();
    }

    void Start()
    {
        StartTimer();
    }

}
