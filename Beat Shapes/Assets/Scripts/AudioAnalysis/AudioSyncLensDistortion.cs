using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AudioSyncLensDistortion : AudioSyncer
{
	// Chromatic aberration properties
	private Volume volume;
	private LensDistortion lensDistortion;

	private void Start()
	{
		// Initialization
		volume = GetComponent<Volume>();
		volume.profile.TryGet(out lensDistortion);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		// If there is already a beat, just do nothing
		if (isBeat) return;

		// Otherwise return to the original chromatic aberration value
		lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, 0f, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		// Start the change of the color
		//StopCoroutine("MoveToChromaticAberration");
		StartCoroutine("MoveToChromaticAberration", .5f);
	}

	private IEnumerator MoveToChromaticAberration(float lensDistortionTargetValue)
	{
		// Lerp to the target value because a beat has been detected
		float curr = lensDistortion.intensity.value;
		float initial = curr;
		float timer = 0;

		while (curr != lensDistortionTargetValue)
		{
			curr = Mathf.Lerp(initial, lensDistortionTargetValue, timer / timeToBeat);
			timer += Time.deltaTime;

			lensDistortion.intensity.value = curr;

			yield return null;
		}

		// At this point there is no beat and it can wait for others
		isBeat = false;
	}
}
