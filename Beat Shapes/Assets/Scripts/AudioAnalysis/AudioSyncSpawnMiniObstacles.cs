using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncSpawnMiniObstacles : AudioSyncer
{
    private int n_miniObstacles;

    private List<GameObject> newGO;

    private void Start()
    {
        newGO = new List<GameObject>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // If there is already a beat, just do nothing
        if (isBeat)
        {
            return;
        }
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StartCoroutine("SpawnMiniObstacles", 1f);
    }

    IEnumerator SpawnMiniObstacles(float targetValue)
    {
        // Lerp to the target scale because a beat has been detected
        float currValue = 0f;
        float initialValue = currValue;
        float timer = 0;

        while (currValue != targetValue)
        {
            currValue = Mathf.Lerp(initialValue, targetValue, timer / timeToBeat);
            timer += Time.deltaTime;

            yield return null;
        }
        //Debug.Log("beat");

        // Higher is the value of the beat, higher is the number of spawned mini obstacles
        if (name == "Charachter")
            n_miniObstacles = (int)AudioSpectrum.spectrumValue / 4 + 1;
        else
            n_miniObstacles = (int)AudioSpectrum.spectrumValue / 10 + 1;
        //Debug.Log(n_miniObstacles);

        // Create the mini obstacles
        for (int i = 0; i < n_miniObstacles; i++)
        {
            //Debug.Log(transform.childCount);
            newGO.Add(Instantiate(transform.GetChild(0).gameObject, transform.GetChild(0).position, transform.GetChild(0).rotation, gameObject.transform));
            newGO[i].transform.localScale = new Vector2(AudioSpectrum.spectrumValue / 10, AudioSpectrum.spectrumValue / 10);
            newGO[i].GetComponent<AudioSyncMoveMiniObstacle>().isOriginal = false;

            // If it derives from the character, it is a projectile, so detach from it
            if (newGO[i].name == "Projectile(Clone)")
                newGO[i].transform.parent = null;
        }

        newGO.Clear();

        // At this point there is no beat and it can wait for others
        isBeat = false;
    }
}
