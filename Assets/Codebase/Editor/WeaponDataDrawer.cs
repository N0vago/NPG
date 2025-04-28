using Codebase.Game.Data;
using UnityEditor;
using UnityEngine;

namespace Codebase.Editor
{
    [CustomPropertyDrawer(typeof(WeaponSettings))]
    public class WeaponDataDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            WeaponType weaponType = (WeaponType)property.FindPropertyRelative("weaponType").enumValueIndex;
            int lineCount = 1;

            switch (weaponType)
            {
                case WeaponType.Pistol:
                    lineCount += 1;
                    break;
                case WeaponType.MachineGun:
                    lineCount += 1;
                    break;
                case WeaponType.Shotgun:
                    lineCount += 1;
                    break;
                case WeaponType.GrenadeLauncher:
                    lineCount += 3;
                    break;
                case WeaponType.Flamethrower:
                    lineCount += 1;
                    break;
                case WeaponType.EnergyGun:
                    lineCount += 1;
                    break;
            }

            lineCount += 1;
            lineCount += 6;

            return lineCount * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);
            
            EditorGUI.LabelField(currentRect, new GUIContent("Weapon specific data"), EditorStyles.boldLabel);
            currentRect.y += lineHeight + spacing;

            EditorGUI.indentLevel++;
            
            SerializedProperty weaponTypeProp = property.FindPropertyRelative("weaponType");
            EditorGUI.PropertyField(currentRect, weaponTypeProp);
            currentRect.y += lineHeight + spacing;

            WeaponType weaponType = (WeaponType)weaponTypeProp.enumValueIndex;
            
            
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    break;
                case WeaponType.Shotgun:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("spreadRadius"));
                    currentRect.y += lineHeight + spacing;
                    break;
                case WeaponType.MachineGun:
                    break;
                case WeaponType.GrenadeLauncher:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("explosionRadius"));
                    currentRect.y += lineHeight + spacing;
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("explosionDelay"));
                    currentRect.y += lineHeight + spacing;
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("projectileSpeed"));
                    currentRect.y += lineHeight + spacing;
                    break;
                case WeaponType.Flamethrower:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("sphereRadius"));
                    currentRect.y += lineHeight + spacing;
                    break;
                case WeaponType.EnergyGun:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("freezingTime"));
                    currentRect.y += lineHeight + spacing;
                    break;
            }

            EditorGUI.indentLevel--;
            
            EditorGUI.LabelField(currentRect, new GUIContent("General settings"), EditorStyles.boldLabel);
            currentRect.y += lineHeight + spacing;

            EditorGUI.indentLevel++;
            
            EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("weaponPrefab"));
            currentRect.y += lineHeight + spacing;

            EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("damage"));
            currentRect.y += lineHeight + spacing;

            EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("reloadTime"));
            currentRect.y += lineHeight + spacing;

            EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("fireRate"));
            currentRect.y += lineHeight + spacing;

            EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("magSize"));
            currentRect.y += lineHeight + spacing;

            EditorGUI.IntSlider(currentRect, property.FindPropertyRelative("accuracy"), 0, 4,
                new GUIContent("Accuracy"));
            currentRect.y += lineHeight + spacing;

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }
    }

}
