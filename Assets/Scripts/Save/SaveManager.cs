using Newtonsoft.Json.Schema;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    //private string m_PublicPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

    private static SaveManager m_Instance;
    public static SaveManager Instance { get => m_Instance; }
    public UnityAction OnSave { get; set; } //params: filename in json
    private readonly float m_DefaultAutoSaveTime = 10;
    private float m_AutoSaveTime = 0;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( this );

        DontDestroyOnLoad( this );
        m_AutoSaveTime = m_DefaultAutoSaveTime;
    }

    private void Start()
    {
        //string localPath = Application.dataPath + Path.AltDirectorySeparatorChar + Utils.FARM_OBJECTS_FILENAME;
        //string jsonString = File.ReadAllText( localPath );
        //FarmObjectData[] farmObjects = JsonHelper.FromJson<FarmObjectData>( jsonString );
        //Debug.Log( farmObjects[0].worldPos );

        //FarmObjectData[] farmObjects = LoadData<FarmObjectData>( "asd.json" );
        //Debug.Log( farmObjects );
    }
    private void Update()
    {
        if ( m_AutoSaveTime <= 0 )
        {
            m_AutoSaveTime = m_DefaultAutoSaveTime;
            print( "Save" );
            OnSave?.Invoke();
            return;
        }

        m_AutoSaveTime -= Time.deltaTime;
    }
    //public void CreateNewDataFile( string filename )
    //{
    //    m_LocalPath += filename;
    //    m_PublicPath += filename;
    //}

    //public async void SaveData<T>( T serializableData )
    //{
    //    string localPath = Application.dataPath + Path.AltDirectorySeparatorChar + "test.json";
    //    Debug.Log( "Saving at " + localPath );
    //    await Task.Run( () =>
    //    {
    //        string json = JsonUtility.ToJson( serializableData );
    //        using StreamWriter writer = new( localPath );
    //        writer.Write( json );
    //    } );
    //}

    public async Task SaveData<T>( T[] serializableData, string filename )
    {
        string localPath = Application.dataPath + Path.AltDirectorySeparatorChar + filename;
        Debug.Log( "Saving!" );
        await Task.Run( () =>
        {
            string json = JsonHelper.ToJson( serializableData );
            File.WriteAllText( localPath, json );
        } );
        Debug.Log( "Save Done!" );
    }


    public bool LoadData<T>( string fileName, out T[] values )
    {
        string localPath = Application.dataPath + Path.AltDirectorySeparatorChar + fileName;
        if ( !File.Exists( localPath ) )
        {
            values = null;
            return false;
        }
        string jsonString = File.ReadAllText( localPath );
        values = JsonHelper.FromJson<T>( jsonString );
        return true;
    }
}
