using System;
using System.Collections.Generic;

namespace Entities;

public partial class AnimalLocation
{
    public long LocationId { get; set; }

    public long AnimalId { get; set; }

    public long Id { get; set; }

    public DateTime DateTimeOfVisitLocationPoint { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
