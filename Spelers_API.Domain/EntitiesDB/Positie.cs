using System;
using System.Collections.Generic;

namespace Spelers_API.Domain.EntitiesDB;

public partial class Positie
{
    public int Id { get; set; }

    public string Naam { get; set; } = null!;

    public virtual ICollection<Speler> Spelers { get; set; } = new List<Speler>();
}
