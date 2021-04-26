using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float speed = 0.02f;
    [SerializeField] private bool reverseLeftRight;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0f, 0f, speed);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(0f, 0f, -speed);
        }
    }
}
