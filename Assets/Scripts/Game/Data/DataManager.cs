using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameData gameData = new GameData();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.F))
        {
            SaveData();
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            LoadData();
        }
#endif
    }

    public void SaveData()
    {
        BinaryFormatter binary = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "game.save");

        try
        {
            using (FileStream file = File.Create(path))
            {
                binary.Serialize(file, gameData);
            }
            Debug.Log("Saved!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save data: " + e.Message);
        }
    }

    public void LoadData()
    {
        BinaryFormatter binary = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "game.save");

        if (File.Exists(path))
        {
            try
            {
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    GameData loadData = (GameData)binary.Deserialize(file);
                    gameData = loadData;
                    Debug.Log("Loaded!");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load data: " + e.Message);
            }
        }
    }
}
