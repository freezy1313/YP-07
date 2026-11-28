using AgencyEmploymentApp.Data;
using AgencyEmploymentApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgencyEmploymentApp.Services;

public sealed record AuthResult(bool Success, string? Error, User? User, string? Login);

public static class AuthService
{
    /// <summary>
    /// Проверяет логин и пароль средствами PostgreSQL (pgcrypto, bcrypt).
    /// Пароль не хранится и не сравнивается в приложении в открытом виде.
    /// </summary>
    public static async Task<AuthResult> AuthenticateAsync(string login, string password)
    {
        await using var db = new AppDbContext();

        var authorization = await db.Authorizations
            .FromSqlRaw(
                "SELECT * FROM authorizations " +
                "WHERE login = {0} AND password_hash = crypt({1}, password_hash)",
                login,
                password)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (authorization is null)
        {
            return new AuthResult(false, "Неверный логин или пароль.", null, login);
        }

        var user = await db.Users
            .Include(u => u.Role)
            .Include(u => u.Authorization)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.AuthorizationId == authorization.AuthorizationId);

        return user is null
            ? new AuthResult(false, "Для учётной записи не найден пользователь.", null, login)
            : new AuthResult(true, null, user, authorization.Login);
    }
}
