using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup = null;
        Coroutine currentActiveFade=null;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
        }
        public void FadeoutImmediate()
        {
            //canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
        }
        IEnumerator FadeOutIn()
        {
            yield return FadeOut(1F);
            yield return FadeIn(1F);
        }
        public Coroutine FadeOut(float time)
        {
            return Fade(time, 1f);

        }
        private IEnumerator FadeRoutine(float time, float target)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,target, Time.deltaTime / time) ;
                yield return null;
            }
        }
        public Coroutine FadeIn(float time)
        {
            return Fade(time, 0f);
        }
        public Coroutine Fade(float time,float target)
        {
            if (currentActiveFade != null)
                StopCoroutine(currentActiveFade);
            currentActiveFade = StartCoroutine(FadeRoutine(time, target));
            return currentActiveFade;
        }
    }

}