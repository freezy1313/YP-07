using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Резюме пользователей
/// </summary>
public partial class Resume
{
    public int ResumeId { get; set; }

    public int UserId { get; set; }

    public int DesiredPositionId { get; set; }

    /// <summary>
    /// Описание опыта работы
    /// </summary>
    public string? Experience { get; set; }

    public int ResumeStatusId { get; set; }

    /// <summary>
    /// Согласие на обработку персональных данных
    /// </summary>
    public bool PersonalDataConsent { get; set; }

    public DateOnly PublishedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual DesiredPosition DesiredPosition { get; set; } = null!;

    public virtual ResumeStatus ResumeStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
