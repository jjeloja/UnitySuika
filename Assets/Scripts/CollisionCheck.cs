using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public bool collided;

    void Start()
    {
        collided = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        collided = true;
    }
}
