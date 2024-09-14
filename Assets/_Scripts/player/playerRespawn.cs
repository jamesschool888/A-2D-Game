using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

   public void respawnTimer()
    {
        StartCoroutine(Respawn(0f));
    }

    IEnumerator Respawn(float duration)
    {
       yield return new WaitForSeconds(duration);
        transform.position = respawnPoint.position;
        playerHealth.Respawn();
    }
}
