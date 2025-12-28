using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 13.3 - How an NPC behaves in specific weather
    [Serializable]
    public struct WeatherBehaviorEntry
    {
        public Weather Weather;
        public WeatherBehavior Behavior;

        public WeatherBehaviorEntry(Weather weather, WeatherBehavior behavior)
        {
            Weather = weather;
            Behavior = behavior;
        }

        public static WeatherBehaviorEntry StaysIndoorsDuring(Weather weather)
        {
            return new WeatherBehaviorEntry(weather, WeatherBehavior.StaysIndoors);
        }
    }
}
