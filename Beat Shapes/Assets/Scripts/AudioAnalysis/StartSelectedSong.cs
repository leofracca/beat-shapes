using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartSelectedSong : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        string songName = PlayerPrefs.GetString("selectedSong", "A Himitsu - Adventures");

        // With Resources
        if (songName.StartsWith("file:///"))
        {
            WWW audioLoader = new WWW(songName);
            while (!audioLoader.isDone)
                System.Threading.Thread.Sleep(100);

            audioSource.clip = audioLoader.GetAudioClip();
        }
        else
            audioSource.clip = Resources.Load("Music/" + songName) as AudioClip;

        audioSource.Play();

        // With Addressables
        //AsyncOperationHandle<AudioClip> ac = Addressables.LoadAssetAsync<AudioClip>(PlayerPrefs.GetString("selectedSong", "A Himitsu - Adventures") + ".mp3");
        //ac.Completed += OnLoadDone;
    }

    //private void OnLoadDone(AsyncOperationHandle<AudioClip> handle)
    //{
    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        audioSource.clip = handle.Result;
    //        audioSource.Play();
    //        Addressables.Release(handle);
    //    }
    //}
}
