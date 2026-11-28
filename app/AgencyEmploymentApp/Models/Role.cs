using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Роли пользователей системы
/// </summary>
public partial class Role
{
    public int RoleId { get; set; }

    /// <summary>
    /// Наименование роли
    /// </summary>
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
