using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Premium_Shop
{
    public enum PSCategories { All, Packs, Obols, Drachma, Skips, Boosts }

    public class PsUiManager : MonoBehaviour
    {
        public static event Action OnShopOpened;
        public static event Action OnShopClosed;

        [Header("Main References")]
        [Tooltip("The dark background panel (Background Shift)")]
        public GameObject backgroundOverlay; 

        [Serializable]
        public class CategoryData {
            public PSCategories categoryEnum;
            public GameObject categoryObject;
        }
    
        public List<CategoryData> categoriesList;
        
        public void OpenShop()
        {
            if (backgroundOverlay != null) backgroundOverlay.SetActive(true);
            OnShopOpened?.Invoke();
        }
        
        public void CloseShop()
        {
            OnShopClosed?.Invoke();
        }

        public void OnCategoryClickedInt(int index)
        {
            OnCategoryClicked((PSCategories)index);
        }

        public void OnCategoryClicked(PSCategories selected)
        {
            foreach (var item in categoriesList)
            {
                if (item.categoryObject == null) continue;
                var isActive = (item.categoryEnum == selected);
                item.categoryObject.SetActive(isActive);
            }
        }
    }
}