using UnityEngine;

namespace Managers.Core
{
    public class UIPrefabManager : MonoBehaviour
    {
        [Header("Shop Prefabs")] 
        public GameObject upgradeItemPrefab;

        public static UIPrefabManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}