using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    Quaternion initialRotation;

    public Transform playerTransform;

    Vector3 initialPosition;

    float offsetX;
    float initialoffsetX;
    float offsetY;
    float initialoffsetY;

    Vector3 correctPosition;



    void Start()
    {
        initialRotation = transform.rotation;

        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        transform.rotation = initialRotation;
        float x = playerTransform.position.x;
        float y = playerTransform.position.y + 1.5f;
        transform.position = new Vector3(x, y, 0);

        //correctPosition.y = playerTransform.position.y - transform.position.y;
    }
}
