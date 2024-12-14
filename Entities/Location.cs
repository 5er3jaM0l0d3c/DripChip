using System;
using System.Collections.Generic;

namespace Entities;

public partial class Location
{
    public long Id { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
