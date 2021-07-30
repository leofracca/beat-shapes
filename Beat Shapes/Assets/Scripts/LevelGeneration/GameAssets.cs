using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    private void Awake()
    {
        instance = this;
    }

    public static GameAssets GetInstance()
    {
        return instance;
    }

    public Transform triangleObstacle;
    public Transform squareObstacle;
    public Transform circleObstacle;
    public Transform hexagonObstacle;

    public Transform triangle;
    public Transform square;
    public Transform circle;
    public Transform hexagon;
}
