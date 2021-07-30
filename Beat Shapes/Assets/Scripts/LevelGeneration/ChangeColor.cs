using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangeColor : MonoBehaviour
{
    // Ceiling, floor and obstacles
    SpriteRenderer sp;
    Light2D l;

    // Colors
    static bool firstRandomColorAlreadyAssigned = false;
    static Color firstRandomColor;
    Color tmpColor;
    float h, s, v;

    // Bloom properties
    private Volume volume;
    private Bloom bloom;

    [SerializeField] [Range(0f, 1f)] float lerpTime; // The time between a color and the next
    [SerializeField] Color[] myColors; // The colors

    static int colorIndex = 0;
    static float tmp = 0f;

    void Start()
    {
        // Choose the first random color
        if (!firstRandomColorAlreadyAssigned)
        {
            firstRandomColor = new Color(Random.value, Random.value, Random.value);

            Color.RGBToHSV(firstRandomColor, out h, out s, out v);
            firstRandomColor = Color.HSVToRGB(h, 1f, 1f);
            firstRandomColorAlreadyAssigned = true;
        }

        // Assign it
        switch (name)
        {
            case "Global Volume":
                volume = GetComponent<Volume>();
                volume.profile.TryGet(out bloom);

                bloom.tint.value = firstRandomColor;

                //Debug.Log("global volume");
                break;
            case "Ceiling":
            case "Floor":
                sp = transform.GetComponent<SpriteRenderer>();
                l = transform.GetChild(0).GetComponent<Light2D>();

                sp.color = firstRandomColor;
                l.color = sp.color;

                //Debug.Log("floor ceiling");
                break;
            case "Triangle Obstacle": // Triangle obstacles (they are a little bit different from the other obstacles)
            case "Triangle Obstacle(Clone)":
                sp = transform.GetChild(0).GetComponent<SpriteRenderer>();
                l = transform.GetChild(0).GetChild(0).GetComponent<Light2D>();

                break;
            default: // The other obstacles but triangles
                sp = transform.GetComponent<SpriteRenderer>();
                l = transform.GetChild(0).GetComponent<Light2D>();

                //Debug.Log("obstacle");
                break;
        }
    }

    void Update()
    {
        // Lerp ONLY on ceiling and floor
        if (name.Equals("Ceiling") || name.Equals("Floor"))
        {
            LerpColors();
        }
        // The obstacles simply take the color from the ceiling at every frame (without lerping)
        else
        {
            sp.color = GameObject.Find("Ceiling").GetComponent<SpriteRenderer>().color; // Ceiling and floor have the same color; there are no different choosing from ceiling or floor
            l.color = sp.color;
        }
    }

    private void LerpColors()
    {
        // Lerp the colors
        switch (name)
        {
            case "Global Volume":
                //LerpBloomColors();
                break;
            default:
                LerpObjectColors();
                break;
        }

        tmp = Mathf.Lerp(tmp, 1f, lerpTime * Time.deltaTime);

        if (tmp > 0.9f)
        {
            tmp = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }

    //private void LerpBloomColors()
    //{
    //    tmpColor = Color.Lerp(bloom.tint.value, myColors[colorIndex], lerpTime * Time.deltaTime);
    //    Color.RGBToHSV(tmpColor, out h, out s, out v);
    //    bloom.tint.value = Color.HSVToRGB(h, 1f, 1f);
    //}

    private void LerpObjectColors()
    {
        tmpColor = Color.Lerp(sp.color, myColors[colorIndex], lerpTime * Time.deltaTime);
        Color.RGBToHSV(tmpColor, out h, out s, out v);
        sp.color = Color.HSVToRGB(h, 1f, 1f);

        tmpColor = Color.Lerp(l.color, myColors[colorIndex], lerpTime * Time.deltaTime);
        Color.RGBToHSV(tmpColor, out h, out s, out v);
        l.color = Color.HSVToRGB(h, 1f, 1f);
    }

    private void OnDestroy()
    {
        // Ceiling and floor are destroyed when the scene is destroyed; there are no different choosing from ceiling or floor
        if (name.Equals("Ceiling"))
        {
            firstRandomColorAlreadyAssigned = false;
        }
    }
}
