using SQLite;

namespace MealRoulette.Models;

[Table("UserProfiles")]
public class UserProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int HouseholdId { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    // For future preferences and dietary restrictions
    public string PreferencesJson { get; set; } = string.Empty;
}
