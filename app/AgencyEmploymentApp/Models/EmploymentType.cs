using System;
using System.Collections.Generic;

namespace AgencyEmploymentApp.Models;

/// <summary>
/// Справочник типов занятости
/// </summary>
public partial class EmploymentType
{
    public int EmploymentTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
