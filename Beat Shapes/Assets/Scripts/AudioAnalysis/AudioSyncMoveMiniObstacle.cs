using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncMoveMiniObstacle : AudioSyncer
{
    private float upForce = 100f;
    private float sideForce = 100f;
    private Vector2 finalPosition;
    private float finalRotation;

    public bool isOriginal = true;

    private void Start()
    {
        if (!isOriginal)
        {
            CalculatePosition();

            // Start the repositioning
            StartCoroutine("MoveToPosition");
            StartCoroutine("MoveToRotation");
        }
    }

    private void CalculatePosition()
    {
        float randomX = Random.Range(-sideForce, sideForce);
        float xPosition = randomX >= 0 ? randomX + 100f : randomX - 100f;
        float randomY = Random.Range(-upForce, upForce);
        float yPosition = randomY >= 0 ? randomY + 100f : randomY - 100f;

        finalRotation = Random.Range(-360f, 360f);

        finalPosition = new Vector2(xPosition, yPosition);
    }

    IEnumerator MoveToPosition()
    {
        // Lerp to the target position because a beat has been detected
        Vector2 currPosition = transform.localPosition;
        Vector2 initialPosition = currPosition;
        float timer = 0;

        while (currPosition != finalPosition)
        {
            currPosition = Vector2.Lerp(initialPosition, finalPosition, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localPosition = currPosition;

            yield return null;
        }

        // At this point there is no beat and it can wait for others
        isBeat = false;

        //Destroy(gameObject);
    }

    IEnumerator MoveToRotation()
    {
        // Lerp to the target rotation because a beat has been detected
        float currRotation = transform.rotation.z;
        float initialRotation = currRotation;
        float timer = 0;

        while (currRotation != finalRotation)
        {
            currRotation = Mathf.Lerp(initialRotation, finalRotation, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.rotation = Quaternion.Euler(0f, 0f, currRotation);

            yield return null;
        }

        // At this point there is no beat and it can wait for others
        isBeat = false;
    }
}
