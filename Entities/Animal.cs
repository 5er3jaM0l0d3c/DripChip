using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Entities;

public partial class Animal
{
    public long Id { get; set; }

    public float Weight { get; set; }

    public float Length { get; set; }

    public float Height { get; set; }

    public int ChipperId { get; set; }

    public long ChippingLocationId { get; set; }

    public DateTime? ChippingDateTime { get; set; }

    public DateTime? DeathDateTime { get; set; }

    public string LifeStatus { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public virtual Account Chipper { get; set; } = null!;

    public virtual Location ChippingLocation { get; set; } = null!;
}
public partial class Animal
{
    public long[] animalTypes { get; set; } = null!;
    public long[] visitedLocations { get; set; } = null!;
}
