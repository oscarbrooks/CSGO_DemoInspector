namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class RoundInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private Text _roundNumber;

        [SerializeField]
        private Round _currentRound;

        private void Start()
        {
            StartCoroutine(UpdateVisuals());
        }

        private void Update()
        {

        }

        private IEnumerator UpdateVisuals()
        {
            while (true)
            {
                if (PlaybackManager.Instance.Frames.Count > 0)
                {
                    var currentTick = PlaybackManager.Instance.Frames[PlaybackManager.Instance.CurrentFrame].Tick;

                    _currentRound = MatchInfoManager.Instance.Rounds.LastOrDefault(r => currentTick > r.StartTick);

                    _roundNumber.text = $"{_currentRound.Number}/{MatchInfoManager.Instance.Rounds.Count}";
                }

                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}
