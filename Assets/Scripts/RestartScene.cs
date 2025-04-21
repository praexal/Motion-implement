using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public string triggerTag = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}
