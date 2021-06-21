using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIMenuHandler : MonoBehaviour
{
    public static UIMenuHandler Instance;

    protected string playerName;
    protected int highScore;

    //The buttons on the Main Menu
    [SerializeField] InputField playerNameInput;
    [SerializeField] Text displayPlayerName;
    [SerializeField] Button startGame;
    [SerializeField] Button quitGame;

    public int HighScore { get; set; }
    public string PlayerName { get; set; }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }

    public void Start()
    {
        playerNameInput.onEndEdit.AddListener(delegate { DisplayPlayerName(playerNameInput); });
        startGame.onClick.AddListener(delegate { StartGame(); });
        quitGame.onClick.AddListener(delegate { QuitGame(); });
    }

    void DisplayPlayerName(InputField input)
    {
        PlayerName = input.text;
        displayPlayerName.text = PlayerName;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public int HighScore;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.HighScore = HighScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScore = data.HighScore;
        }
        else
        {
            HighScore = 0;
        }
    }


}
