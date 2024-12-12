using System;
using System.Collections.Generic;

namespace Entities;

public partial class Animal
{
    public long Id { get; set; }

    public float Weight { get; set; }

    public float Length { get; set; }

    public float Height { get; set; }

    public int ChipperId { get; set; }

    public long ChippingLocationId { get; set; }

    public DateOnly ChippingDateTime { get; set; }

    public DateOnly? DeathDateTime { get; set; }

    public AnimalLifeStatus LifeStatus { get; set; }

    public AnimalGender Gender { get; set; }

    public virtual Account Chipper { get; set; } = null!;

    public virtual Location ChippingLocation { get; set; } = null!;
}
