namespace UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LoadingController : MonoBehaviour
    {
        [SerializeField]
        private Text _roundsText;

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
                _roundsText.text = $"{MatchInfoManager.Instance.Rounds.Count} Rounds loaded";

                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        public void OnParsingComplete()
        {
            var items = GetComponentsInChildren<MaskableGraphic>();

            var itemsFaded = 0;

            foreach (var item in items) StartCoroutine(FadeItem(item, () => itemsFaded++));

            StartCoroutine(WaitUntilTrue(() => itemsFaded == items.Length, () => gameObject.SetActive(false)));
        }

        private IEnumerator FadeItem(MaskableGraphic item, Action onCompleted)
        {
            for (float f = 1f; f >= 0; f -= 0.02f)
            {
                var color = item.color;
                color.a = f;
                item.color = color;
                yield return null;
            }
            item.gameObject.SetActive(false);
            onCompleted();
        }

        private IEnumerator WaitUntilTrue(Func<bool> isTrue, Action onCompleted)
        {
            while (!isTrue())
            {
                yield return null;
            }
            onCompleted();
        }
    }

}