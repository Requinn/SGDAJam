using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject {
    public class FadeInScene: MonoBehaviour {
        public CanvasGroup fadeCanvas;
        public bool FadeOnAwake = false;
        public float FadeAwakeDuration = 1.5f;

        void Awake() {
            if (FadeOnAwake) {
                StartFadeOut(FadeAwakeDuration);
            }
        }

        void Start() {
            //StartFadeOut(FadeAwakeDuration);
        }

        private void StartFadeIn(float duration) {
            InitiateFade(0f, 1f, duration);
        }

        private void StartFadeOut(float duration) {
            InitiateFade(1f, 0f, duration);
        }

        private void InitiateFade(float from, float to, float duration) {
            fadeCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeFromTo(from, to, duration));
        }

        /// <summary>
        /// Lerp the canvas from visible to not
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator FadeFromTo(float from, float to, float duration) {
            float timer = 0;
            float alphaValue = from;

            while (timer < duration) {
                timer += Time.deltaTime;
                alphaValue = Mathf.Lerp(from, to, timer / duration); //get a value from a to b @ time t in relation to how much time we have remaining
                fadeCanvas.alpha = alphaValue;
                yield return null;
            }

            if (alphaValue == 0f) {
                fadeCanvas.gameObject.SetActive(false);
            }
        }
    }
}