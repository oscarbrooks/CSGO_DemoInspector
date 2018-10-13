namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DemoInfo;

    public class TeamSidePanel : MonoBehaviour {

        [SerializeField]
        public TeamSide TeamSide;
        [SerializeField]
        public Team CurrentFaction;

        [SerializeField]
        private Text _clanText;

        [SerializeField]
        private GameObject _playerInfoPanelPrefab;

        private List<PlayerInfoPanel> _playerPanels { get; set; } = new List<PlayerInfoPanel>();

        private void Start()
        {
            _clanText.text = CurrentFaction.ToString();
        }

        public void AddPlayer(PartialPlayer player)
        {
            var playerInfoPanel = Instantiate(_playerInfoPanelPrefab, transform).GetComponent<PlayerInfoPanel>();

            playerInfoPanel.Initialize(player, transform);

            _playerPanels.Add(playerInfoPanel);
        }
    }
}

