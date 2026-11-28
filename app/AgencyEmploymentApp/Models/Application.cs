using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Отклики пользователей на вакансии
/// </summary>
public partial class Application
{
    public int ApplicationId { get; set; }

    public int ResumeId { get; set; }

    public int VacancyId { get; set; }

    /// <summary>
    /// Дата отклика
    /// </summary>
    public DateOnly ResponseDate { get; set; }

    public int ApplicationStatusId { get; set; }

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual Resume Resume { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
