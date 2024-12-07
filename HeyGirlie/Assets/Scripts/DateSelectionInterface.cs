using UnityEngine;
using UnityEngine.SceneManagement;

public class DateSelectionInterface : MonoBehaviour
{
    // TEMPORARY FOR TESTING
    // Later -- parameter is Character enum defined in GameManager.cs -> load relevant scene
    public void ChangeScene()
    {
        SceneManager.LoadScene("Test Route");
    }

    public void CassandraScene()
    {
        SceneManager.LoadScene("Cassandra");
    }
}
