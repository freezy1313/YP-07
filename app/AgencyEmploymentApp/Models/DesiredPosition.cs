using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Справочник желаемых должностей и специализаций
/// </summary>
public partial class DesiredPosition
{
    public int DesiredPositionId { get; set; }

    public string Specialization { get; set; } = null!;

    public string? Description { get; set; }

    /// <summary>
    /// Ожидаемая заработная плата, руб.
    /// </summary>
    public decimal? Salary { get; set; }

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}
