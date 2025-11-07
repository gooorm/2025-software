using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_simple : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                            0, Input.GetAxis("Vertical") * Time.deltaTime * speed);
    }
}
