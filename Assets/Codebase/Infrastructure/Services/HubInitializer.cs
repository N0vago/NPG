using Codebase.Game;
using Codebase.Game.Player;
using Codebase.Infrastructure.Data;
using Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using Zenject;

namespace Codebase.Infrastructure.Services
{
    public class HubInitializer : MonoBehaviour, IDataReader
    {
        [SerializeField] private PlayerController playerController;

        private ProgressDataHandler _progressData;

        private Vector3 _playerPosition = new();

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            _progressData = progressDataHandler;
            _progressData.RegisterObserver(this);
        }

        private void Start()
        {
            InitHub();
        }

        private void InitHub()
        {
            playerController.transform.position = _playerPosition;
        }

        public void Load(GameData data)
        {
            _playerPosition = data.playerData.playerPosition;
        }
    }
}