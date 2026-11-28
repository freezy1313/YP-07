using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Вакансии, размещённые пользователями
/// </summary>
public partial class Vacancy
{
    public int VacancyId { get; set; }

    /// <summary>
    /// Пользователь, разместивший вакансию
    /// </summary>
    public int UserId { get; set; }

    public decimal Salary { get; set; }

    public int EmploymentTypeId { get; set; }

    public int WorkModeId { get; set; }

    public int PositionTitleId { get; set; }

    public DateOnly OpenedAt { get; set; }

    /// <summary>
    /// Дата закрытия вакансии; NULL — вакансия открыта
    /// </summary>
    public DateOnly? ClosedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual EmploymentType EmploymentType { get; set; } = null!;

    public virtual PositionTitle PositionTitle { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual WorkMode WorkMode { get; set; } = null!;
}
