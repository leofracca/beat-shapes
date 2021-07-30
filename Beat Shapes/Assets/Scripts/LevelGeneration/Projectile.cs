using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject destroyEffect;

    private void Start()
    {
        // Activate the simulation of the rigid body for registering collisions
        if (!GetComponent<AudioSyncMoveMiniObstacle>().isOriginal)
        {
            GetComponent<Rigidbody2D>().simulated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the projectile hits an obstacle, then they both explode
        if (IsObstacle(collision.transform.name) || IsTriangle(collision.transform.name))
        {
            //Debug.Log("collisione con " + collision.name);

            //Instantiate(destroyEffect, transform.position, Quaternion.identity);
            //Destroy(gameObject);

            Instantiate(destroyEffect, collision.transform.position, Quaternion.identity);
            if (IsTriangle(collision.transform.name))
            {
                if (collision.transform.parent.name.EndsWith("(Clone)"))
                    Destroy(collision.transform.parent.gameObject);
            } 
            else
                Destroy(collision.gameObject);
        }

        // If the projectile hits the ceiling or the floor, only the projectile explodes
        else if (IsWall(collision.transform.name))
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private bool IsObstacle(string obstacleName)
    {
        return obstacleName.EndsWith("Obstacle(Clone)");
    }

    private bool IsTriangle(string obstacleName)
    {
        return obstacleName == "Triangle";
    }

    private bool IsWall(string wallName)
    {
        return wallName == "Ceiling" || wallName == "Floor";
    }
}
