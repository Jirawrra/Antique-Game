using UnityEngine;
using UnityEngine.UI;
using Managers.Premium_Shop;

namespace UI.Effects
{
    [RequireComponent(typeof(Button))]
    public class UITabButtonEffect : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private PsTabManager tabManager;
        public int categoryIndex;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnTabClicked);
        }

        private void OnTabClicked()
        {
            if (tabManager != null)
            {
                tabManager.SelectTab(categoryIndex);
            }
        }

        public void SetSelectionState(bool active)
        {
        }
    }
}