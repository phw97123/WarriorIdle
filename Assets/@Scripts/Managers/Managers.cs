using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static bool s_init = false;

    private ResourceManager _resourceManager = new ResourceManager();
    private ObjectManager _objectManager = new ObjectManager();
    private PoolManager _poolManager = new PoolManager(); 

    public static ResourceManager ResourceManager { get { return Instance?._resourceManager; } }
    public static ObjectManager ObjectManager { get { return Instance?._objectManager; } }
    public static PoolManager PoolManager { get { return Instance?._poolManager; } }

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
            }
            return s_instance;
        }
    }
}
