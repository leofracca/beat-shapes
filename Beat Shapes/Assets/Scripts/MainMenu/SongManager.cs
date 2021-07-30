using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SongManager : MonoBehaviour
{
    public Song[] songs;

    void Start()
    {
        int i = 1;

        foreach (Song s in songs)
        {
            Transform songButton = GameObject.Find("Song" + i).transform;

            songButton.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(s.name);
            songButton.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(s.author);

            i++;
        }
    }
}
