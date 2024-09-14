using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        if(target != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = target.position.x;
            transform.position = newPosition;
        }
    }
}
