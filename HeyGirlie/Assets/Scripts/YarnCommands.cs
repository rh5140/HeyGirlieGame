using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;
    public GameObject gameObject; // Vague naming -- currently only used for Date Select in the Cassandra scene
    [SerializeField] private Character _character;
    private LoveInterest _loveInterest;

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

        dialogueRunner.AddCommandHandler(
            "next_week",
            NextWeek
        );
    }

    void Start()
    {
        _loveInterest = GameManager.Instance.SetUpScene(_character);
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
        _loveInterest.AddPoints(num);
    }

    private void IncrementDateCount()
    {
        _loveInterest.IncrementDateCount();
    }

    private void NextWeek()
    {
        GameManager.Instance.IncreaseWeek();
    }

    [YarnFunction("get_week")]
    public static int GetWeek()
    {
        return GameManager.Instance.GetWeek();
    }

}
