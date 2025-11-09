using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 7f;

    [SerializeField] float gravity = -4.9f;
    [SerializeField] float jumpHeight = 1.2f;

    [SerializeField] float mouseSpeed = 1.5f; //마우스회전속도

    Vector3 velo; //이동방향 저장
    Transform camTr; //카메라 트랜스폼

    CharacterController cc;
    float xRot; //x의 회전값 저장

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        camTr = Camera.main.transform;
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); //좌우입력값
        float v = Input.GetAxis("Vertical"); //상하입력값

        float curSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 movDir = transform.right * h + transform.forward * v; //이동방향 계산

        cc.Move(movDir * curSpeed * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;

        xRot -= mouseY; // 상하회전값
        xRot = Mathf.Clamp(xRot, -90f, 90f); //상하 회전의 각도를 90도로 제한

        camTr.localRotation = Quaternion.Euler(xRot, 0f, 0f); //카메라의 상하회전
        transform.Rotate(Vector3.up * mouseX); //플레이어 좌우 회전
    }
}
