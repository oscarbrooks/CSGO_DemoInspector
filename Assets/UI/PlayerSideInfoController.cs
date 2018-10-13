namespace UI {
    using DemoInfo;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerSideInfoController : MonoBehaviour {

        [SerializeField]
        private TeamSidePanel[] _teamPanels;

        private void Start() {

        }

        private void Update() {

        }

        public void AddPlayer(PartialPlayer player)
        {
            if (player.IsBot || player.Team == Team.Spectate) return;

            var teamPanel = _teamPanels.First(t => t.CurrentFaction == player.Team);

            teamPanel.AddPlayer(player);
        }
    }

    public enum TeamSide
    {
        Left,
        Right
    }

}
