using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static bool s_init = false;

    private ResourceManager _resourceManager = new ResourceManager();
    private ObjectManager _objectManager = new ObjectManager();
    private PoolManager _poolManager = new PoolManager(); 
    private UIManager _uiManager = new UIManager();
    private GameManager _gameManager = new GameManager();
    private CurrencyManager _currecncyManager = new CurrencyManager();
    private SoundManager _soundManager = new SoundManager();
    private DataManager _dataManager = new DataManager();

    public static ResourceManager ResourceManager { get { return Instance?._resourceManager; } }
    public static ObjectManager ObjectManager { get { return Instance?._objectManager; } }
    public static PoolManager PoolManager { get { return Instance?._poolManager; } }
    public static UIManager UIManager { get { return Instance?._uiManager; } }
    public static GameManager GameManager { get { return Instance?._gameManager; } }
    public static CurrencyManager CurrencyManager { get { return Instance?._currecncyManager; } }
    public static SoundManager SoundManager { get { return Instance?._soundManager; } }
    public static DataManager DataManager { get { return Instance?._dataManager; } }

    public static Managers Instance
    {
        get
        {
            if (s_init == false)
            {
                s_init = true;

                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject() { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();

                s_instance._currecncyManager.Init(); 
            }
            return s_instance;
        }
    }

    public static void DestroyManagers()
    {
        if (s_instance == null) return;

        s_instance._resourceManager.Destroy();
        s_instance._objectManager.Destroy();
        s_instance._poolManager.Destroy();
        s_instance._uiManager.Destroy();
        s_instance._currecncyManager.Destroy();
        s_instance._soundManager.Destroy();

        s_instance = null;
        s_init = false;
    }
}

