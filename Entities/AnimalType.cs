﻿using System;
using System.Collections.Generic;

namespace Entities;

public partial class AnimalType
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<AnimalAnimalType> AnimalAnimalTypes { get; set; } = new List<AnimalAnimalType>();
}
