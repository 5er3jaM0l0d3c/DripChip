using System;
using System.Collections.Generic;

namespace Entities;

public partial class AnimalAnimalType
{
    public long Id { get; set; }

    public long AnimalTypeId { get; set; }

    public long AnimalId { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual AnimalType AnimalType { get; set; } = null!;
}
