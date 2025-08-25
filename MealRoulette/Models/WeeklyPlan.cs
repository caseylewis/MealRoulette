using System;
using System.Collections.Generic;

namespace MealRoulette.Models
{
    public class WeeklyPlan
    {
        public List<WeeklyPlanEntry> Entries { get; set; } = new();
        public List<string> SelectedDays { get; set; } = new();
        public List<string> SelectedMealTypes { get; set; } = new();
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }

    public class WeeklyPlanEntry
    {
        public string Day { get; set; } = string.Empty;
        public string MealType { get; set; } = string.Empty;
        public string MealName { get; set; } = string.Empty;
    }
}
