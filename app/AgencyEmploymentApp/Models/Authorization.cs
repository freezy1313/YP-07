using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Учётные данные для входа в систему
/// </summary>
public partial class Authorization
{
    public int AuthorizationId { get; set; }

    /// <summary>
    /// Уникальный логин пользователя
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary>
    /// bcrypt-хеш пароля (pgcrypto, crypt/gen_salt)
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    public virtual User? User { get; set; }
}
