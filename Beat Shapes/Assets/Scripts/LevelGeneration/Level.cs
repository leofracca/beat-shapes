using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    private const float CAMERA_SIZE = 50f;
    private const float SPEED = 30f;
    private const float LEFT_EDGE = -150f; // The threshold for destroying obstacles
    private const float RIGHT_EDGE = 150f; // The threshold for instantiating obstacles

    //private /*const*/ int HOW_MANY_OBSTACLES = Enum.GetNames(typeof(ObstaclesTypes)).Length;
    private List<Transform> obstacles; // All the obstacles in the scene
    private Transform obstacleToSpawn;
    private Array values;
    private System.Random random;
    public static /*int*/ObstaclesTypes randomObj;

    private float obstacleSpawnTimer;
    private float obstacleSpawnTimerMax; // The seconds elapsed between the generation of two obstacles

    public static int obstaclesPassed;
    private int spawnedObstacles; // The total number of obstacles spawned from the start

    public static State state; // The state of the player

    void Awake()
    {
        obstacles = new List<Transform>();

        obstacleSpawnTimerMax = 2f;

        state = State.WaitingToStart;
    }

    private void Start()
    {
        obstaclesPassed = 0;
        spawnedObstacles = 0;

        values = Enum.GetValues(typeof(ObstaclesTypes));
        random = new System.Random();

        ChooseObstacleToSpawn();
    }

    void Update()
    {
        if (state == State.Playing)
        {
            MoveObstaclesLeft();
            SpawnObstaclesRight();
        }
    }

    private void ChooseObstacleToSpawn()
    {
        //randomObj = Random.Range(0, HOW_MANY_OBSTACLES);

        randomObj = (ObstaclesTypes)values.GetValue(random.Next(values.Length));

        switch (randomObj)
        {
            case ObstaclesTypes.Triangle:
                obstacleToSpawn = GameAssets.GetInstance().triangleObstacle;
                break;
            case ObstaclesTypes.Rectangle:
                obstacleToSpawn = GameAssets.GetInstance().squareObstacle;
                break;
            case ObstaclesTypes.Circle:
                obstacleToSpawn = GameAssets.GetInstance().circleObstacle;
                break;
            case ObstaclesTypes.Hexagon:
                obstacleToSpawn = GameAssets.GetInstance().hexagonObstacle;
                break;
            default:
                return;
        }
    }

    public void MoveObstaclesLeft()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            bool isToTheRightOfPlayer = obstacles[i].position.x > 0;

            // Move the obstacle
            obstacles[i].position += new Vector3(-1, 0, 0) * SPEED * Time.deltaTime;

            if (obstacles[i].name == "Hexagon Obstacle(Clone)")
                obstacles[i].rotation = Quaternion.Euler(0f, 0f, Time.unscaledTime * 7f);

            // If it was to the right and now it is to the left, increment the counter
            if (isToTheRightOfPlayer && obstacles[i].position.x <= 0)
            {
                // If it is not a triangle, add 2
                if (/*randomObj != ObstaclesTypes.Triangle && */!obstacles[i].GetChild(0).name.Equals("Triangle Obstacle"))
                    obstaclesPassed += 2;
                // If it is a triangle, add 1 (because each space is formed by 2 triangles)
                else
                    obstaclesPassed++;
            }

            // If the obstacle is out of the camera view, destroy it
            if (obstacles[i].position.x < LEFT_EDGE)
            {
                Destroy(obstacles[i].gameObject);
                obstacles.Remove(obstacles[i]);
                i--;
            }
        }
    }

    public void SpawnObstaclesRight()
    {
        obstacleSpawnTimer -= Time.deltaTime;

        if (obstacleSpawnTimer < 0)
        {
            obstacleSpawnTimer += obstacleSpawnTimerMax;

            CreateObstacle(RIGHT_EDGE);
            spawnedObstacles++;

            // Every Random.Range(3, 10) obstacles change the type of obstacle
            if (spawnedObstacles != 0 && spawnedObstacles % Random.Range(3, 10) == 0)
            {
                ChooseObstacleToSpawn();
            }
        }
    }

    public void CreateObstacle(float xPos)
    {
        float obstacleWidth;
        float obstacleHeight;

        switch (randomObj)
        {
            case ObstaclesTypes.Triangle:
                //obstacleWidth = Random.Range(30f, 50f);
                //obstacleHeight = Random.Range(30f, 50f);
                //CreateTriangle(obstacleWidth, obstacleHeight, xPos, true);
                //CreateTriangle(obstacleWidth, CAMERA_SIZE * 2 - obstacleHeight, xPos, false);
                obstacleWidth = Random.Range(20f, 35f);
                obstacleHeight = Random.Range(20f, 35f);
                CreateTriangle(obstacleWidth, obstacleHeight, xPos, true);

                obstacleWidth = Random.Range(20f, 35f);
                obstacleHeight = Random.Range(20f, 35f);
                CreateTriangle(obstacleWidth, obstacleHeight, xPos, false);
                break;
            case ObstaclesTypes.Rectangle:
                //obstacleWidth = Random.Range(50f, 80f);
                //obstacleHeight = Random.Range(5f, 10f);
                obstacleWidth = Random.Range(10f, 30f);
                obstacleHeight = Random.Range(5f, 10f);

                CreateRectangle(obstacleWidth, obstacleHeight, xPos);
                break;
            case ObstaclesTypes.Circle:
                //float radius = Random.Range(20f, 50f);
                float radius = Random.Range(10f, 25f);

                CreateCircle(radius, xPos);
                break;
            case ObstaclesTypes.Hexagon:
                //float size = Random.Range(20f, 50f);
                float size = Random.Range(10f, 25f);

                CreateHexagon(size, xPos);
                break;
        }
    }

    public void CreateTriangle(float width, float height, float xPos, bool bottom)
    {
        // Insantiate the obstacle with the position and the size
        Transform obstacle = Instantiate(obstacleToSpawn);

        float obstacleYPosition;
        if (bottom)
        {
            obstacleYPosition = -CAMERA_SIZE;
        }
        else
        {
            obstacleYPosition = CAMERA_SIZE;
            obstacle.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        obstacle.position = new Vector3(xPos, obstacleYPosition);
        obstacle.GetChild(0).localScale = new Vector2(width, height);

        obstacles.Add(obstacle);
    }

    public void CreateRectangle(float width, float height, float xPos)
    {
        // Insantiate the obstacle with the position and the size
        Transform obstacle = Instantiate(obstacleToSpawn);

        obstacle.position = new Vector3(xPos, Random.Range(CAMERA_SIZE, -CAMERA_SIZE));
        obstacle.GetChild(0).localScale = new Vector2(width, height);

        obstacles.Add(obstacle);
    }

    public void CreateCircle(float radius, float xPos)
    {
        // Insantiate the obstacle with the position and the size
        Transform obstacle = Instantiate(obstacleToSpawn);

        obstacle.position = new Vector3(xPos, Random.Range(CAMERA_SIZE, -CAMERA_SIZE));
        obstacle.GetChild(0).localScale = new Vector2(radius, radius);

        obstacles.Add(obstacle);
    }

    public void CreateHexagon(float size, float xPos)
    {
        // Insantiate the obstacle with the position and the size
        Transform obstacle = Instantiate(obstacleToSpawn);

        obstacle.position = new Vector3(xPos, Random.Range(CAMERA_SIZE, -CAMERA_SIZE));
        obstacle.GetChild(0).localScale = new Vector2(size, size);

        obstacles.Add(obstacle);
    }
}
