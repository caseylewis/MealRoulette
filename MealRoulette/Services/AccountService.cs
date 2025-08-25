using System;
using MealRoulette.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MealRoulette.Services;

public class AccountService
{
    private readonly MealDatabase _db;
    public AccountService(MealDatabase db) => _db = db;

    public Task<List<HouseholdAccount>> GetHouseholdsAsync() => _db.GetHouseholdsAsync();
    public Task<List<UserProfile>> GetProfilesAsync(int householdId) => _db.GetProfilesForHouseholdAsync(householdId);

    public async Task<int> CreateHouseholdAsync(string email, string password)
    {
        email = email.Trim();
        var households = await _db.GetHouseholdsAsync();
        var existing = households.Find(h => h.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
        {
            await _db.SetCurrentHouseholdAsync(existing.Id);
            await _db.SetCurrentProfileAsync(null);
            return 0;
        }
        var account = new HouseholdAccount
        {
            Email = email,
            PasswordHash = Hash(password)
        };
        var result = await _db.SaveHouseholdAsync(account);
        await _db.SetCurrentHouseholdAsync(account.Id);
        await _db.SetCurrentProfileAsync(null);
        return result;
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        var households = await _db.GetHouseholdsAsync();
        var acct = households.Find(h => h.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
        if (acct == null) return false;
        if (acct.PasswordHash != Hash(password)) return false;
        await _db.SetCurrentHouseholdAsync(acct.Id);
        await _db.SetCurrentProfileAsync(null);
        return true;
    }

    public Task<int> CreateProfileAsync(int householdId, string displayName)
        => _db.SaveUserProfileAsync(new UserProfile { HouseholdId = householdId, DisplayName = displayName.Trim() });

    public Task SetCurrentProfileAsync(int? id) => _db.SetCurrentProfileAsync(id);
    public Task<int?> GetCurrentHouseholdIdAsync() => _db.GetCurrentHouseholdAsync();
    public Task<int?> GetCurrentProfileIdAsync() => _db.GetCurrentProfileAsync();

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
