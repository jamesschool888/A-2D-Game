using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBoundary : MonoBehaviour
{
    public Transform player;
    public float killOffset = 1f;
    public Health Player;
    public bool offBounds;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = Camera.main.WorldToViewportPoint(player.position);

            if (playerPosition.y < 0f - killOffset)
            {
                // Player is out of bounds, trigger death or game over here
                // You can call a method to handle player death or game over
                Player.Die();
            }
        }
    
    }
}
