using UnityEngine;
using System.Collections.Generic;
using UI.Effects;

namespace Managers.Premium_Shop
{
    public class PsTabManager : MonoBehaviour
    {
        public PsUiManager uiManager;
        public List<UITabButtonEffect> tabButtons = new List<UITabButtonEffect>();
        public int currentSelection;

        private void Start()
        {
            // Set the default tab on launch
            SelectTab(currentSelection);
        }

        public void SelectTab(int index)
        {
            currentSelection = index;

            // 1. Tell the UI Manager to swap the panels
            if (uiManager != null) 
            {
                uiManager.OnCategoryClickedInt(index);
            }

            // 2. Tell each tab whether it is the Selected one
            for (var i = 0; i < tabButtons.Count; i++)
            {
                tabButtons[i].SetSelectionState(i == index);
            }
        }
    }
}