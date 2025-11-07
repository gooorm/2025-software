using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NewBehaviourScript : MonoBehaviour
{
    GameObject timerText;
    GameObject timer2min;
    GameObject gameOver1;
    GameObject gameOver2;

    float time1 = 0.0f;
    float time2 = 120.0f;

    bool isOver1 = false;
    bool isOver2 = false;

    // Start is called before the first frame update
    void Start()
    {
        this.timerText = GameObject.Find("Time1");
        this.timer2min = GameObject.Find("Time2");
        this.gameOver1 = GameObject.Find("GameOver1");
        this.gameOver2 = GameObject.Find("GameOver2");

        this.gameOver1.SetActive(false);
        this.gameOver2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 0 -> 120
        if (!isOver1)
        {
            if (this.time1 < 120.0f)
            {
                this.time1 += Time.deltaTime;
            }
            else
            {
                this.time1 = 120.0f;
                isOver1 = true;

                this.timerText.SetActive(false);
                this.gameOver1.SetActive(true);
                this.gameOver1.GetComponent<TextMeshProUGUI>().text = "GAME OVER1";
            }

            int min1 = Mathf.FloorToInt(this.time1 / 60);
            int sec1 = Mathf.FloorToInt(this.time1 % 60);
            this.timerText.GetComponent<TextMeshProUGUI>().text = "Time1: " + string.Format("{0:00}:{1:00}", min1, sec1);
        }

        // 120 -> 0
        if (!isOver2)
        {
            if (this.time2 > 0.0f)
            {
                this.time2 -= Time.deltaTime;
            }
            else
            {
                this.time2 = 0.0f;
                isOver2 = true;

                this.timer2min.SetActive(false);
                this.gameOver2.SetActive(true);
                this.gameOver2.GetComponent<TextMeshProUGUI>().text = "GAME OVER2";
            }

            int min2 = Mathf.FloorToInt(this.time2 / 60);
            int sec2 = Mathf.FloorToInt(this.time2 % 60);
            this.timer2min.GetComponent<TextMeshProUGUI>().text = "Time2: " + string.Format("{0:00}:{1:00}", min2, sec2);

        }


    }
}
