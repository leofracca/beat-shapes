using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    public Vector2 beatScale;
    private Vector2 shrinkScale;

    private void Start()
    {
        // Initialize the scales
        shrinkScale = transform.localScale;

        if (name.Equals("Ceiling") || name.Equals("Floor"))
        {
            beatScale = new Vector2(transform.localScale.x, transform.localScale.y + 5);

            return;
        }

        // If the object is not a circle or a hexagon
        if (/*Level.randomObj != ObstaclesTypes.Circle && Level.randomObj != ObstaclesTypes.Hexagon || */name.Equals("Triangle Obstacle(Clone)") || name.Equals("Square Obstacle(Clone)"))
        {
            // It decides what dimension is the one to increase (height or width)
            float increaseDimension = Random.value;

            if (increaseDimension < 0.5f)
            {
                beatScale = new Vector2(transform.localScale.x + 9f, transform.localScale.y);
            }
            else
            {
                beatScale = new Vector2(transform.localScale.x, transform.localScale.y + 5f);
            }
        }
        else if (name.Equals("Character"))
        {
            beatScale = new Vector2(transform.localScale.x + 1f, transform.localScale.y + 1f);
        }
        else
        {
            // It is a circle or a hexagon
            beatScale = new Vector2(transform.localScale.x + 5f, transform.localScale.y + 5f);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // If there is already a beat, just do nothing
        if (isBeat)
        {
            return;
        }

        // Otherwise return to the shrink scale
        transform.localScale = Vector3.Lerp(transform.localScale, shrinkScale, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        // Start the resizing
        //StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);
    }

    IEnumerator MoveToScale(Vector2 targetScale)
    {
        // Lerp to the target scale because a beat has been detected
        Vector2 currScale = transform.localScale;
        Vector2 initialScale = currScale;
        float timer = 0;

        while(currScale != targetScale)
        {
            currScale = Vector2.Lerp(initialScale, targetScale, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = currScale;

            yield return null;
        }

        // At this point there is no beat and it can wait for others
        isBeat = false;
    }
}
