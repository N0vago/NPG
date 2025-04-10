using Codebase.Game.Data;
using Codebase.Game.Weapon;
using UnityEditor;
using UnityEngine;

namespace Codebase.Editor
{
    [CustomEditor(typeof(WeaponData))]
    public class WeaponDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _weaponTypeProp;
        //Pistol
        private SerializedProperty _twoHandedProp;
        //Shotgun
        private SerializedProperty _spreadRadiusProp;
        //Grenade launcher
        private SerializedProperty _explosionRadiusProp;
        private SerializedProperty _explosionDelayProp;
        private SerializedProperty _projectileSpeedProp;
        //Flamethrower
        private SerializedProperty _sphereRadiusProp;
        //Energy gun
        private SerializedProperty _freezingTimeProp;

        private WeaponType _previousWeaponType;
        void OnEnable()
        {
            _weaponTypeProp = serializedObject.FindProperty("weaponType");

            _twoHandedProp = serializedObject.FindProperty("twoHanded");

            _spreadRadiusProp = serializedObject.FindProperty("spreadRadius");

            _explosionRadiusProp = serializedObject.FindProperty("explosionRadius");
            _explosionDelayProp = serializedObject.FindProperty("explosionDelay");
            _projectileSpeedProp = serializedObject.FindProperty("projectileSpeed");
            
            _sphereRadiusProp = serializedObject.FindProperty("sphereRadius");

            _freezingTimeProp = serializedObject.FindProperty("freezingTime");

            _previousWeaponType = (WeaponType)_weaponTypeProp.enumValueIndex;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((WeaponData)target), typeof(MonoScript), false);
            
            EditorGUILayout.PropertyField(_weaponTypeProp);
            WeaponType weaponType = (WeaponType)_weaponTypeProp.enumValueIndex;
            
            if (weaponType != _previousWeaponType)
            {
                ClearUnusedFields(_previousWeaponType);
                _previousWeaponType = weaponType;
            }

            EditorGUILayout.Space();
            
            switch (weaponType)
            {
                case WeaponType.Gun:
                    EditorGUILayout.PropertyField(_twoHandedProp);
                    break;

                case WeaponType.Shotgun:
                    EditorGUILayout.PropertyField(_spreadRadiusProp);
                    break;
                case WeaponType.MachineGun:
                    break;
                case WeaponType.GrenadeLauncher:
                    EditorGUILayout.PropertyField(_explosionRadiusProp);
                    EditorGUILayout.PropertyField(_explosionDelayProp);
                    EditorGUILayout.PropertyField(_projectileSpeedProp);
                    break;
                case WeaponType.Flamethrower:
                    EditorGUILayout.PropertyField(_sphereRadiusProp);
                    break;
                case WeaponType.EnergyGun:
                    EditorGUILayout.PropertyField(_freezingTimeProp);
                    break;
            }
            
            EditorGUILayout.Space();
            
            DrawPropertiesExcluding(serializedObject,
                "m_Script",
                "weaponType",
                "twoHanded",
                "spreadRadius",
                "explosionRadius",
                "explosionDelay",
                "projectileSpeed",
                "sphereRadius",
                "freezingTime"
            );

            serializedObject.ApplyModifiedProperties();
        }

        private void ClearUnusedFields(WeaponType previousWeaponType)
        {
            switch (previousWeaponType)
            {
                case WeaponType.Gun:
                    _twoHandedProp.boolValue = false;
                    break;
                case WeaponType.Shotgun:
                    _sphereRadiusProp.floatValue = 0f;
                    break;
                case WeaponType.MachineGun:
                    break;
                case WeaponType.GrenadeLauncher:
                    _explosionRadiusProp.floatValue = 0f;
                    _explosionDelayProp.floatValue = 0f;
                    _projectileSpeedProp.floatValue = 0f;
                    break;
                case WeaponType.Flamethrower:
                    _sphereRadiusProp.floatValue = 0f;
                    break;
                case WeaponType.EnergyGun:
                    _freezingTimeProp.floatValue = 0f;
                    break;
            }
        }
    }
}