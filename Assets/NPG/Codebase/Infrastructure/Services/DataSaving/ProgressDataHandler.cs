using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NPG.Codebase.Infrastructure.JsonData;
using UnityEngine;

namespace NPG.Codebase.Infrastructure.Services.DataSaving
{
    public class ProgressDataHandler
    {
        private GameData _gameData;
        
        private readonly List<IDataReader> _dataOperators = new();

        private const string DataPath = "Assets/NPG/Resources/ProgressData/GameProgressData.json";
        private const string ResourcesPath = "ProgressData/GameProgressData";
        private const string InitialConfigPath = "ProgressData/InitialConfiguration";

        public ProgressDataHandler()
        {
            _gameData = GatherGameData();
        }

        public void RegisterObserver(IDataReader dataOperator)
        {
            if (_dataOperators.Contains(dataOperator))
            {
                Debug.LogError("Data operator already registered");
                return;
            }
            _dataOperators.Add(dataOperator);
            dataOperator.Load(_gameData);
        }
        public void UnregisterObserver(IDataReader dataOperator)
        {
            if (_dataOperators.Contains(dataOperator))
            {
                _dataOperators.Remove(dataOperator);
            }
            else
            {
                Debug.LogError("Data operator doesn't register or not exist");
            }
        }

        public void SaveProgress(IDataWriter dataOperator)
        {
            if (!_dataOperators.Contains(dataOperator))
            {
                Debug.LogError("Data operator doesn't register or not exist");
                return;
            }

            dataOperator.Save(ref _gameData);

            string json = JsonConvert.SerializeObject(_gameData, Formatting.Indented);
            File.WriteAllText(DataPath, json);
            
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
                    Debug.Log($"Saving data for: {_dataOperators[i].GetType().Name}");
                    SaveProgress(dataWriter);
                }
            }
        }

        private GameData GatherGameData()
        {
            TextAsset jsonAsset = Resources.Load<TextAsset>(ResourcesPath);
            if (jsonAsset == null)
            {
                jsonAsset = GetInitialConfiguration();
            }

            var data = _gameData ?? JsonConvert.DeserializeObject<GameData>(jsonAsset.text);

            return data;
        }

        private TextAsset GetInitialConfiguration()
        {
            TextAsset initialConfig = Resources.Load<TextAsset>(InitialConfigPath);
            if (initialConfig == null)
            {
                Debug.LogError("InitialConfiguration not found in Resources.");
                return null;
            }

            File.WriteAllText(DataPath, initialConfig.text);

            return initialConfig;
        }
    }
}