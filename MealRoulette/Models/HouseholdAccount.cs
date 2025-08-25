using SQLite;

namespace MealRoulette.Models;

[Table("Households")]
public class HouseholdAccount
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Unique = true)]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}
