using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Transform player;
    private bool isFrozen = false;
    private bool gameOver = false;
    private float freezeTimer = 0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (gameOver) return;

        // Freeze 상태일 때 타이머 감소
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                Unfreeze();
            }
            return;
        }

        // 플레이어 따라가기
        if (player != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    public void Freeze(float duration)
    {
        if (gameOver) return;
        isFrozen = true;
        freezeTimer = duration;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true; // 멈출 때 물리 영향 끊기
        }
    }

    private void Unfreeze()
    {
        isFrozen = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOver = true;
        isFrozen = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        Debug.Log("Game Over! 적과 부딪힘!");
    }
}
