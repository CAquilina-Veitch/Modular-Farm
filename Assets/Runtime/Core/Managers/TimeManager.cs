using R3;
using UnityEngine;
using PromptHarvest.Data;

namespace PromptHarvest.Core.Managers
{
    public class TimeManager : SingletonBehaviour<TimeManager>
    {
        [SerializeField] private int weeksPerSeason = 4;

        public ReadOnlyReactiveProperty<TimePhase> CurrentPhase => currentPhase;
        private readonly ReactiveProperty<TimePhase> currentPhase = new(TimePhase.Morning);

        public ReadOnlyReactiveProperty<int> CurrentDay => currentDay;
        private readonly ReactiveProperty<int> currentDay = new(1);

        public ReadOnlyReactiveProperty<int> CurrentWeek => currentWeek;
        private readonly ReactiveProperty<int> currentWeek = new(1);

        public ReadOnlyReactiveProperty<Season> CurrentSeason => currentSeason;
        private readonly ReactiveProperty<Season> currentSeason = new(Season.Spring);

        public ReadOnlyReactiveProperty<int> CurrentYear => currentYear;
        private readonly ReactiveProperty<int> currentYear = new(1);

        public void AdvancePhase()
        {
            switch (currentPhase.Value)
            {
                case TimePhase.Morning:
                    currentPhase.Value = TimePhase.Day;
                    break;
                case TimePhase.Day:
                    currentPhase.Value = TimePhase.Night;
                    break;
                case TimePhase.Night:
                    currentPhase.Value = TimePhase.Morning;
                    AdvanceDay();
                    break;
            }
        }

        private void AdvanceDay()
        {
            if (currentDay.Value >= 7)
            {
                currentDay.Value = 1;
                AdvanceWeek();
            }
            else
            {
                currentDay.Value++;
            }
        }

        private void AdvanceWeek()
        {
            currentWeek.Value++;
            UpdateSeasonAndYear();
        }

        private void UpdateSeasonAndYear()
        {
            int weekIndex = currentWeek.Value - 1;
            int seasonIndex = (weekIndex / weeksPerSeason) % 4;
            currentSeason.Value = (Season)seasonIndex;
            currentYear.Value = (weekIndex / (weeksPerSeason * 4)) + 1;
        }
    }
}
