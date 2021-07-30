using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AudioSyncChromaticAberration : AudioSyncer
{
	// Chromatic aberration properties
	private Volume volume;
	private ChromaticAberration chromaticAberration;

	private void Start()
	{
		// Initialization
		volume = GetComponent<Volume>();
		volume.profile.TryGet(out chromaticAberration);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		// If there is already a beat, just do nothing
		if (isBeat) return;

		// Otherwise return to the original chromatic aberration value
		chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0f, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		// Start the change of the color
		//StopCoroutine("MoveToChromaticAberration");
		StartCoroutine("MoveToChromaticAberration", 1f);
	}

	private IEnumerator MoveToChromaticAberration(float chromaticAberrationTargetValue)
	{
		// Lerp to the target value because a beat has been detected
		float curr = chromaticAberration.intensity.value;
		float initial = curr;
		float timer = 0;

		while (curr != chromaticAberrationTargetValue)
		{
			curr = Mathf.Lerp(initial, chromaticAberrationTargetValue, timer / timeToBeat);
			timer += Time.deltaTime;

			chromaticAberration.intensity.value = curr;

			yield return null;
		}

		// At this point there is no beat and it can wait for others
		isBeat = false;
	}
}