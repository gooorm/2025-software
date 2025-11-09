using UnityEngine;

public class LaserHit : MonoBehaviour
{
    public float freezeTime = 5f; // 멈추는 시간 (초)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TestEnemy enemy = other.GetComponent<TestEnemy>();
            if (enemy != null)
            {
                enemy.Freeze(freezeTime);
                Debug.Log("적이 레이저에 맞음! 5초 동안 멈춤");
            }
        }
    }
}
