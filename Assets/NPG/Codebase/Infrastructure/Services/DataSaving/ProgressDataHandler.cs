using System.Collections.Generic;
using System.IO;
using NPG.Codebase.Infrastructure.JsonData;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace NPG.Codebase.Infrastructure.Services.DataSaving
{
    public class ProgressDataHandler
    {
        private GameData _gameData;
        
        private readonly List<IDataReader> _dataOperators = new();

        private const string DataPath = "Assets/Resources/Data/GameProgressData.json";

        public ProgressDataHandler()
        {
            _gameData = GatherGameData();
        }

        public void RegisterObserver(IDataReader dataOperator)
        {
            _dataOperators.Add(dataOperator);
            dataOperator.Load(_gameData);
        }

        public void SaveProgress(IDataWriter dataOperator)
        {
            if (_dataOperators.Contains(dataOperator))
            {
                dataOperator.Save(ref _gameData);

                string json = JsonUtility.ToJson(_gameData, true);
                File.WriteAllText(DataPath, json);
                AssetDatabase.Refresh();

            }
            else
            {
                Debug.LogError("Data operator doesn't register or not exist");
            }
        }

        public void LoadAll()
        {
            for (int i = 0; i < _dataOperators.Count; i++)
            {
                _dataOperators[i].Load(_gameData);
            }
        }

        public void SaveAll()
        {
            for (int i = 0; i < _dataOperators.Count; i++)
            {
                if (_dataOperators[i] is IDataWriter dataWriter){
                    SaveProgress(dataWriter);
                }
            }
        }

        private GameData GatherGameData()
        {
            TextAsset json = Resources.Load<TextAsset>("Data/GameProgressData");
            if (json.IsUnityNull())
            {
                json = GetInitialConfiguration();
            }

            var data = _gameData.IsUnityNull() ? JsonUtility.FromJson<GameData>(json.text) : _gameData;
            
            return data;
        }

        private TextAsset GetInitialConfiguration()
        {
            FileStream file = File.Create(DataPath);
            TextAsset initialConfig = Resources.Load<TextAsset>("Data/InitialConfiguration");
            file.Write(initialConfig.bytes);

            return initialConfig;
        }
    }
}