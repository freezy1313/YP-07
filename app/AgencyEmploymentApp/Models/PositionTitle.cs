using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Справочник названий позиций
/// </summary>
public partial class PositionTitle
{
    public int PositionTitleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
