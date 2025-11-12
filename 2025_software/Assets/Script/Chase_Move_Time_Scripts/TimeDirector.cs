using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NewBehaviourScript : MonoBehaviour
{
     GameObject timerText;

    float time = 300.0f;

    bool isOver = false;

    // Start is called before the first frame update
    void Start()
    {
        this.timerText = GameObject.Find("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        {
            if (this.time > 0.0f)
            {
                this.time -= Time.deltaTime;
            }
            else
            {
                this.time = 0.0f;
                isOver = true;

                this.timerText.SetActive(false);

                SceneManager.LoadScene("GameOverScene");
            }

            int min = Mathf.FloorToInt(this.time / 60);
            int sec = Mathf.FloorToInt(this.time % 60);
            this.timerText.GetComponent<TextMeshProUGUI>().text = "Time: " + string.Format("{0:00}:{1:00}", min, sec);
        }
    }
}
