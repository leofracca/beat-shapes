using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuBGObjects : MonoBehaviour
{
    private Dictionary<Transform, Vector3> objects; // The object with his target position
    private Transform tmpObj;
    private SpriteRenderer sp;

    public List<float> speedMovements; // The speed of movement for each object in the scene
    public List<float> speedRotations; // The speed of rotation for each object in the scene

    private Array values;
    private System.Random random;

    const int initialNumber = 7;

    void Start()
    {
        objects = new Dictionary<Transform, Vector3>();
        speedMovements = new List<float>();
        speedRotations = new List<float>();

        values = Enum.GetValues(typeof(ObstaclesTypes));
        random = new System.Random();

        for (int i = 0; i < initialNumber; i++)
        {
            ChooseShape();

            objects.Add(Instantiate(tmpObj, new Vector3(Random.Range(-100f, 100f), Random.Range(-50f, 50f), 0f), Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-360f, 360f)))), new Vector3(Random.Range(-100f, 100f) * 1000f, Random.Range(-50f, 50f) * 1000f, 0f));
            
            speedMovements.Add(Random.Range(2f, 7f));
            speedRotations.Add(Random.Range(2f, 7f));
        }

        // Make them bigger
        foreach (KeyValuePair<Transform, Vector3> o in objects)
        {
            float scale = Random.Range(5f, 10f);

            o.Key.transform.localScale = new Vector2(tmpObj.localScale.x * scale, tmpObj.localScale.y * scale);
        }
    }

    void Update()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            KeyValuePair<Transform, Vector3> obj = objects.ElementAt(i);

            obj.Key.position = Vector3.MoveTowards(obj.Key.position, obj.Value, speedMovements[i] * Time.deltaTime);
            obj.Key.rotation = Quaternion.Euler(0f, 0f, Time.unscaledTime * speedRotations[i]);

            // If the object goes out, create another one and then destroy it
            if ((obj.Key.position.x > 150f || obj.Key.position.x < -150f) ||
                (obj.Key.position.y > 100f || obj.Key.position.y < -100f))
            {
                DestroyOldObject(obj.Key, i);
                CreateNewObject(i);
            }
        }
    }

    private void ChooseShape()
    {
        ObstaclesTypes objectToSpawn = (ObstaclesTypes)values.GetValue(random.Next(values.Length));

        switch (objectToSpawn)
        {
            case ObstaclesTypes.Triangle:
                tmpObj = GameAssets.GetInstance().triangle;
                break;
            case ObstaclesTypes.Rectangle:
                tmpObj = GameAssets.GetInstance().square;
                break;
            case ObstaclesTypes.Circle:
                tmpObj = GameAssets.GetInstance().circle;
                break;
            case ObstaclesTypes.Hexagon:
                tmpObj = GameAssets.GetInstance().hexagon;
                break;
        }

        // Generate a random color with the max V value (it makes the bloom effect better)
        sp = tmpObj.GetComponent<SpriteRenderer>();
        sp.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 1f, 1f);
    }

    private void CreateNewObject(int i)
    {
        // Add a new object
        ChooseShape();

        // Choose his spawn position
        int pos = Random.Range(0, 4);
        switch (pos)
        {
            case 0: // Left low
                objects.Add(Instantiate(tmpObj, new Vector3(Random.Range(-150f, -120f), Random.Range(-100f, -70f), 0f), Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-360f, 360f)))), new Vector3(Random.Range(-100f, 100f) * 1000f, Random.Range(-50f, 50f) * 1000f, 0f));
                break;
            case 1: // Left high
                objects.Add(Instantiate(tmpObj, new Vector3(Random.Range(-150f, -120f), Random.Range(70f, 100f), 0f), Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-360f, 360f)))), new Vector3(Random.Range(-100f, 100f) * 1000f, Random.Range(-50f, 50f) * 1000f, 0f));
                break;
            case 2: // Right low
                objects.Add(Instantiate(tmpObj, new Vector3(Random.Range(120f, 150f), Random.Range(-100f, -70f), 0f), Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-360f, 360f)))), new Vector3(Random.Range(-100f, 100f) * 1000f, Random.Range(-50f, 50f) * 1000f, 0f));
                break;
            case 3: // Right high
                objects.Add(Instantiate(tmpObj, new Vector3(Random.Range(120f, 150f), Random.Range(70f, 100f), 0f), Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-360f, 360f)))), new Vector3(Random.Range(-100f, 100f) * 1000f, Random.Range(-50f, 50f) * 1000f, 0f));
                break;
        }

        // Make it bigger
        float scale = Random.Range(5f, 10f);
        objects.ElementAt(i).Key.transform.localScale = new Vector2(tmpObj.localScale.x * scale, tmpObj.localScale.y * scale);

        speedMovements.Add(Random.Range(2f, 7f));
        speedRotations.Add(Random.Range(2f, 7f));
    }

    private void DestroyOldObject(Transform t, int i)
    {
        Destroy(t.gameObject);
        objects.Remove(t);

        speedMovements.RemoveAt(i);
        speedRotations.RemoveAt(i);
    }
}
