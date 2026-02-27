using System.Collections;
using Managers.Premium_Shop;
using UnityEngine;

namespace UI.Animations.Premium_Shop
{
    public class PsPopAnimation : MonoBehaviour
    {
        [Header("Animation Settings")]
        public float duration = 0.25f;
        public AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            PsUiManager.OnShopOpened += PlayOpenAnimation;
            PsUiManager.OnShopClosed += PlayCloseAnimation;
        }

        private void OnDisable()
        {
            PsUiManager.OnShopOpened -= PlayOpenAnimation;
            PsUiManager.OnShopClosed -= PlayCloseAnimation;
        }

        private void PlayOpenAnimation()
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            
            gameObject.SetActive(true);

            animationCoroutine = StartCoroutine(Animate(Vector3.zero, Vector3.one));
        }

        private void PlayCloseAnimation()
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(Animate(transform.localScale, Vector3.zero, true));
        }

        private IEnumerator Animate(Vector3 from, Vector3 to, bool disableOnEnd = false)
        {
            var time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / duration);
                transform.localScale = Vector3.Lerp(from, to, easingCurve.Evaluate(t));
                yield return null;
            }

            transform.localScale = to;

            if (disableOnEnd)
                gameObject.SetActive(false);
        }
    }
}