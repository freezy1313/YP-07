using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Справочник принципов работы (офис, удалённо, гибрид)
/// </summary>
public partial class WorkMode
{
    public int WorkModeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
