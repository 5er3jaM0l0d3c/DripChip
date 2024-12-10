using System;
using System.Collections.Generic;

namespace Entities;

public partial class Animal
{
    public long Id { get; set; }

    public float Weight { get; set; }

    public float Length { get; set; }

    public float Height { get; set; }

    public string Gender { get; set; } = null!;

    public int ChipperId { get; set; }

    public long ChippingLocationId { get; set; }

    public virtual ICollection<AnimalAnimalType> AnimalAnimalTypes { get; set; } = new List<AnimalAnimalType>();

    public virtual Account Chipper { get; set; } = null!;

    public virtual Location ChippingLocation { get; set; } = null!;
}
