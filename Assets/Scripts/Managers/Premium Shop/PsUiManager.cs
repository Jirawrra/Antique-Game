using System.Collections.Generic;
using UnityEngine;

namespace Managers.Premium_Shop
{
    /// <summary>
    /// Defines the specific tabs available in the Premium Shop.
    /// </summary>
    public enum PSCategories { All, Packs, Obols, Drachma, Skips, Boosts }

    public class PsUiManager : MonoBehaviour
    {
        [System.Serializable]
        public class CategoryData {
            public PSCategories categoryEnum;
            public GameObject categoryObject;
        }
    
        public List<CategoryData> categoriesList;   
        
        /// <summary>
        /// Wrapper for Unity Buttons. 
        /// 0:All, 1:Packs, 2:Obols, 3:Drachma, 4:Skips, 5:Boosts
        /// </summary>
        public void OnCategoryClickedInt(int index) 
        {
            OnCategoryClicked((PSCategories)index);
        }

        /// <summary>
        /// Toggles the visibility of shop panels based on the selected category.
        /// </summary>
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