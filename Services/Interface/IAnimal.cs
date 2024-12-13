﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAnimal
    {
        public object? GetAnimalInfo(long? animalId);
        public object? SearchAnimal(DateTime? startDateTime,
                                    DateTime? endDateTime,
                                    int? chipperId,
                                    long? chippingLocationId,
                                    string? lifeStatus,
                                    string? gender,
                                    int from = 0,
                                    int size = 10);
    }
}
