using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System; 

public class LoadingManager : MonoBehaviour
{

    private static LoadingManager instance;
    //Ensure this GameObject is not destroyed on load
    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set this instance and make it persistent
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // Function to load a scene asynchronously based on an integer value
    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    // Coroutine to handle the asynchronous loading
    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
