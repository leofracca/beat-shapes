using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AudioSyncLightIntensity : AudioSyncer
{
    private float originalIntensity;

    private Light2D light;

    void Start()
    {
		// Initialization
        light = GetComponent<Light2D>();
		originalIntensity = light.intensity;
    }

	public override void OnUpdate()
	{
		base.OnUpdate();

		// If there is already a beat, just do nothing
		if (isBeat) return;

		// Otherwise return to the original intensity
		light.intensity = Mathf.Lerp(light.intensity, originalIntensity, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		// Start the change of the intensity
		//StopCoroutine("MoveToIntensity");
		StartCoroutine("MoveToIntensity", 2f);
	}

	private IEnumerator MoveToIntensity(float target)
	{
		// Lerp to the target intensity because a beat has been detected
		float curr = light.intensity;
		float initial = curr;
		float timer = 0;

		while (curr != target)
		{
			curr = Mathf.Lerp(initial, target, timer / timeToBeat);
			timer += Time.deltaTime;

			light.intensity = curr;

			yield return null;
		}

		// At this point there is no beat and it can wait for others
		isBeat = false;
	}
}
