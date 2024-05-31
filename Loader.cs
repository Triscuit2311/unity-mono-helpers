using UnityEngine;

namespace Menu
{
    public class Loader
    {
        public static GameObject Load;

        public static void Init()
        {
            Load = new GameObject();
            Load.AddComponent<Main.Main>();
            Object.DontDestroyOnLoad(Load);
        }

        public static void Unload()
        {
            Object.Destroy(Load);
        }
    }
}
