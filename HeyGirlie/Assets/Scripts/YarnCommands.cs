using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;
    public GameObject gameObject;
    [SerializeField] private Character character;
    private LoveInterest loveInterest;

    void Awake()
    {
        dialogueRunner.AddCommandHandler<string>(
            "change_scene",
            ChangeScene
        );
        
        dialogueRunner.AddCommandHandler(
            "enable_date_select",
            EnableDateSelect
        );

        dialogueRunner.AddCommandHandler<int>(
            "add_points",
            AddPoints
        );

        dialogueRunner.AddCommandHandler(
            "increment_date_count",
            IncrementDateCount
        );

        dialogueRunner.AddCommandHandler<int>(
            "next_week",
            NextWeek
        );
    }

    void Start()
    {
        loveInterest = GameManager.Instance.SetUpScene(character);
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EnableDateSelect()
    {
        gameObject.SetActive(true);
    }
    
    // Method for going to next date OR cassandra if no more dates left in list
    private void CassandraScene(int week)
    {
        SceneManager.LoadScene("Cassandra");
    }

    private void AddPoints(int num)
    {
        loveInterest.AddPoints(num);
    }

    private void IncrementDateCount()
    {
        loveInterest.IncrementDateCount();
    }

    private void NextWeek(int num)
    {
        GameManager.Instance.IncreaseWeek();
    }

    [YarnFunction("get_week")]
    public static int GetWeek()
    {
        return GameManager.Instance.GetWeek();
    }

}
