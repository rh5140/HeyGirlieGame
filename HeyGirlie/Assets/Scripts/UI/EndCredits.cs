using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndCredits : Credits
{
    void Start()
    {
        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return StartCoroutine(RunCredits(2.5f));
        SceneManager.LoadScene("Main Menu");
    }
}
