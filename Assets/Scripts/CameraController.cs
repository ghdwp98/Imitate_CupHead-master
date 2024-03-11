using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    Transform targetPos;
    Vector3 cameraPos = new Vector3(0, 0, -10);
    [SerializeField] float cameraSpeed = 5f;
    void Start()
    {
        targetPos = player.transform;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos.position + cameraPos,
                                  Time.deltaTime * cameraSpeed);


    }
}
