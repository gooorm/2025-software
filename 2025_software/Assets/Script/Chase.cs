using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    [SerializeField] Transform Target;

    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        flying();
    }
    void flying()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        targetPosition = Target.transform.position;
    }
}
