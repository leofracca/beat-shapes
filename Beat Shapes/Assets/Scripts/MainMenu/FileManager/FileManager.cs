using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using System.Windows.Forms;

public class FileManager : MonoBehaviour
{
    private static FileManager instance;

    public string path;

    private void Awake()
    {
        instance = this;
    }

    public static FileManager GetInstance()
    {
        return instance;
    }

    public void OpenExplorer()
    {
        //path = EditorUtility.OpenFilePanel("Select the song you want to play", "", "mp3");
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Title = "Select the song you want to play";
        openFileDialog.Filter = "MP3 Files|*mp3|All files (*.*)|*.*";

        //if (path != null)
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            //UnityWebRequest userSong = new UnityWebRequest("file:///" + path);
            UnityWebRequest userSong = new UnityWebRequest("file:///" + openFileDialog.FileName);
            PlayerPrefs.SetString("selectedSong", userSong.url);
        }
    }
}
