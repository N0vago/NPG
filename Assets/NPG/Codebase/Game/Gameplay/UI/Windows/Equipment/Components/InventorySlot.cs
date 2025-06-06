using System;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        
        private readonly Color _defaultColor = Color.clear;
        public RectTransform RectTransform { get; private set; }
        public bool IsOccupied { get; private set; }
        
        private readonly ReactiveProperty<InventoryItem> _currentItem = new();
        public ReadOnlyReactiveProperty<InventoryItem> CurrentItem => _currentItem;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
        
        public void SetOccupied(bool value, InventoryItem item)
        {
            IsOccupied = value;
            _currentItem.Value = item;
        }
        public void ClearOccupied()
        {
            IsOccupied = false;
            _currentItem.Value = null;
        }

        public void Highlight(bool valid)
        {
            image.color = valid ? Color.green : Color.red;
        }

        public void ClearHighlight()
        {
            image.color = _defaultColor;
        }
    }
}