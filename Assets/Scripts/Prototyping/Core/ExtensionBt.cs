using System;
using System.Collections.Generic;

namespace BeauTambour.Prototyping
{
    public static class ExtensionBt
    {
        public static int Count<T>(this T enumValue) where T : Enum
        {
            var count = 0;
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                var castedValue = (T)value;
                if (Convert.ToInt32(castedValue) == 0) continue;
                
                if (enumValue.HasFlag(castedValue)) count++;
            }

            return count;
        }
        
        public static T[] Split<T>(this T enumValue) where T : Enum
        {
            var results = new List<T>();
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                var castedValue = (T)value;
                if (Convert.ToInt32(castedValue) == 0) continue;
                
                if (enumValue.HasFlag(castedValue)) results.Add(castedValue);
            }

            return results.ToArray();
        }

        public static bool IsActingPhase(this PhaseType phaseType) => phaseType == PhaseType.Placement || phaseType == PhaseType.Setup;
    }
}