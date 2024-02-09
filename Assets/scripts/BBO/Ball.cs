using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{

    float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        if (position.y > -500) 
        { 
            position.y -= speed;
        }
        transform.position = position;
    }
}
