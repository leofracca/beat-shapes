using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSelectedSong : MonoBehaviour
{
    [SerializeField]
    private string fullName;

    public void SetSong()
    {
        // Create the title of the song with the style "Author - Name"
        fullName = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text + " - " + transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        
        PlayerPrefs.SetString("selectedSong", fullName);
    }
}
