using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Пользователи системы
/// </summary>
public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    /// <summary>
    /// Наличие медицинской книжки
    /// </summary>
    public bool HasMedicalBook { get; set; }

    /// <summary>
    /// Ссылка на учётные данные (связь 1:1)
    /// </summary>
    public int AuthorizationId { get; set; }

    public virtual Authorization Authorization { get; set; } = null!;

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
