using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play1()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Play2()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void stop()
    {
        SceneManager.LoadSceneAsync(0);
    }

}