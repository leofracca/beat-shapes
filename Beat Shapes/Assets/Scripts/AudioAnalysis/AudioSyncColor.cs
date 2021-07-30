using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncColor : AudioSyncer
{
	private Color originalColor;

	private SpriteRenderer sp;

	private void Start()
	{
		// Initialization
		sp = GetComponent<SpriteRenderer>();
		originalColor = sp.color;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		// If there is already a beat, just do nothing
		if (isBeat) return;

		// Otherwise return to the original color
		sp.color = Color.Lerp(sp.color, originalColor, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		// Choose a random color
		Color color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 1f, 1f);

		// Start the change of the color
		//StopCoroutine("MoveToColor");
		StartCoroutine("MoveToColor", color);
	}

	private IEnumerator MoveToColor(Color target)
	{
		// Lerp to the target color because a beat has been detected
		Color curr = sp.color;
		Color initial = curr;
		float timer = 0;

		while (curr != target)
		{
			curr = Color.Lerp(initial, target, timer / timeToBeat);
			timer += Time.deltaTime;

			sp.color = curr;

			yield return null;
		}

		// At this point there is no beat and it can wait for others
		isBeat = false;
	}
}