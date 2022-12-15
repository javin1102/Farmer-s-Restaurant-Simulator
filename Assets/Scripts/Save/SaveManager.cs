using SimpleJSON;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get => m_Instance; }
    public UnityAction OnSave { get; set; } //params: filename in json
    private static SaveManager m_Instance;
    private readonly float m_DefaultAutoSaveTime = 10;
    private float m_AutoSaveTime = 0;
    private readonly bool m_IsDebugging = true;
    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(this);

        // DontDestroyOnLoad(this);
        m_AutoSaveTime = m_DefaultAutoSaveTime;
    }

    // private void Update() => AutoSave_Seconds(m_DefaultAutoSaveTime);

    public async Task SaveData(string jsonText, string filename)
    {
        string localPath = SavePath() + filename;
        await Task.Run(() =>
        {
            File.WriteAllText(localPath, jsonText);
        });
    }
    private void AutoSave_Seconds(float timeSeconds)
    {
        if (m_AutoSaveTime <= 0)
        {
            m_AutoSaveTime = timeSeconds;
            OnSave?.Invoke();
            return;
        }

        m_AutoSaveTime -= Time.deltaTime;
    }


    public void LoadData(string filename, UnityAction<JSONNode> OnLoadSucceeded, UnityAction OnLoadFailed)
    {
        string localPath = SavePath() + filename;
        if (!File.Exists(localPath))
        {
            OnLoadFailed?.Invoke();
        }
        else
        {
            string jsonString = File.ReadAllText(localPath);
            JSONNode node = JSONNode.Parse(jsonString);
            OnLoadSucceeded?.Invoke(node);
        }
    }

    private string SavePath() => m_IsDebugging ? Application.dataPath + Path.AltDirectorySeparatorChar : Application.persistentDataPath + Path.AltDirectorySeparatorChar;
}
