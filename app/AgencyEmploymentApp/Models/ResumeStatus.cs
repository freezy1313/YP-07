using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Справочник статусов резюме
/// </summary>
public partial class ResumeStatus
{
    public int ResumeStatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}
