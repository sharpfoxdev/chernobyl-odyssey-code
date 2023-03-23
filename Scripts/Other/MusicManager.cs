using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that plays list of songs in random order. It has two modes of operation - normal and persistent. In normal, 
/// is just plays the songs within the scene and upon leaving the scene, the object is destroyed. This is used in menu scenes. 
/// Persistent mode of operation is when the object persists even when the scene is reloaded/another scene is loaded. 
/// This is useful on the map when player dies and the scene is reloaded, so that the music doesn't start all over again. In 
/// that case upon returning to menus (or other scenes, where we do not want this object to persist), this object is destroyed
/// by script GameplayMusicDestroyer. Also in that case this object has to be appropriatelly tagged. 
/// </summary>
public class MusicManager : MonoBehaviour
{
    [SerializeField] 
    private List<AudioClip> soundtracks = new List<AudioClip>();
    [SerializeField]
    private bool destroyOnLoad = true;
    private int indexIntoRandomSequence = 0;
    //list of random permutation of indexes, which gives an order, in which the tracks will be played
    private List<int> randomTrackIndexSequence = new List<int>();
    private AudioSource audioSource;

    // used for persistent mode
    public static MusicManager instance { get; private set; }
    private void Awake() {
        if (!destroyOnLoad) {
            // this is setup for persistent mode
            if (instance == null) {
                instance = this;
                //we don't want to destroy music manager when changing/loading scenes
                DontDestroyOnLoad(instance);
            }
            else {
                //in a case there is already a music manager, we destroy the game objdect
                //so we don't end up with multiple music managers in the same scene
                Destroy(gameObject);
            }
        }
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        CreateRandomTrackIndexSequence();
        if (soundtracks.Count <= 0) {
            return;
        }
        StartCoroutine(PlayNextTrack());
    }

    /// <summary>
    /// Generates the list of indexes and then permutates it.
    /// This list of indexes is then used for indexing into the list of tracks and determines, in what order will
    /// they be played. 
    /// This way all the tracks will be played
    /// before starting all over again and no two same tracks will be played after each other, so the player
    /// can enjoy all of the available soundtracks.
    /// </summary>
    private void CreateRandomTrackIndexSequence() {
        CreateIndexSequence();
        ShuffleIndexSequence();
    }

    /// <summary>
    /// creates list of indexes, not shuffled
    /// </summary>
    private void CreateIndexSequence() {
        for (int i = 0; i < soundtracks.Count; i++) {
            randomTrackIndexSequence.Add(i);
        }
    }

    /// <summary>
    /// shuffles the list of indexes according to the Knuth's shuffle. We traverse the list from the end, 
    /// in each step we generate random index from beginning to the current index and these two numbers we swap
    /// </summary>
    private void ShuffleIndexSequence() {
        System.Random r = new System.Random();
        int n = randomTrackIndexSequence.Count;
        while (n > 1) {
            n--;
            int k = r.Next(n + 1);
            int value = randomTrackIndexSequence[k];
            randomTrackIndexSequence[k] = randomTrackIndexSequence[n];
            randomTrackIndexSequence[n] = value;
        }
    }
    /// <summary>
    /// Picks a random track and plays it. After that is done, rinse and repeat. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayNextTrack() {

        //set up and play sound track
        int currentTrackIndex = randomTrackIndexSequence[indexIntoRandomSequence];
        audioSource.clip = soundtracks[currentTrackIndex];
        audioSource.Play();
        
        //we move to next index
        indexIntoRandomSequence = (indexIntoRandomSequence + 1) % soundtracks.Count;

        //we wait till the song finishes and then play another song
        while (audioSource.isPlaying) {
            yield return null;
        }
        StartCoroutine(PlayNextTrack());
    }
}
