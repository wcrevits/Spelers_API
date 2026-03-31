using System;
using System.Collections.Generic;

namespace Spelers_API.Domain.EntitiesDB;

public partial class Speler
{
    public int Id { get; set; }

    public string Naam { get; set; } = null!;

    public int Leeftijd { get; set; }

    public int? PositieId { get; set; }

    public int? TeamId { get; set; }

    public virtual Positie? Positie { get; set; }

    public virtual Team? Team { get; set; }
}
