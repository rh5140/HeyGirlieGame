using UnityEngine;
using UnityEngine.SceneManagement;

public class DateSelectionInterface : MonoBehaviour
{
    // TEMPORARY FOR TESTING
    public void ChangeScene()
    {
        SceneManager.LoadScene("Test Route");
    }

    public void CassandraScene()
    {
        SceneManager.LoadScene("Cassandra");
    }
}
