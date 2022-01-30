using UnityEngine;
using UnityEngine.UI;
using System;
using Core.Singletons;

namespace CycleManager
{
    public class CycleManager : MonoBehaviourSingleton<CycleManager>
    {
        public Action<Phases> OnCycleShift;

        public Transform ClockTransform;

        public Image healthCircle;

        public Phases CurrentPhase;

        public float timePerPhase = 36f;

        public float currentTime = 0f;

        public int passedPhases;

        public string currentClockTime;

        [SerializeField]
        private TeethControl _player;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            currentTime += Time.deltaTime;

            var previousPassed = passedPhases;
            passedPhases = (int)(currentTime / timePerPhase);
            CurrentPhase = passedPhases % 2 == 0 ? Phases.Day : Phases.Night;

            if (passedPhases > previousPassed)
                OnCycleShift.Invoke(CurrentPhase);

            currentClockTime = GetCurrentClockTime();

            var restForRotation = (currentTime % (timePerPhase * 2));
            var clockRotation = ((restForRotation / (timePerPhase * 2)) * 360) - 90;

            ClockTransform.rotation = Quaternion.Euler(0, 0, clockRotation);

            if (_player != null)
            {
                healthCircle.fillAmount = _player.Hunger / 100f;
            }
        }

        private string GetCurrentClockTime()
        {
            var time = (6 + (currentTime / timePerPhase * 12)) % 24;
            var hours = (int)time;
            var minutes = (int)((time % 1) * 100 * 0.6);
            string hoursString = hours < 10 ? "0" + hours : hours.ToString();
            string minutesString = minutes < 10 ? "0" + minutes : minutes.ToString();
            return $"{hoursString}:{minutesString}";
        }
    }

    public enum Phases { Day, Night };
}
