using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AnimalGenderConverter : ValueConverter<string, string>
    {
        public AnimalGenderConverter()
            : base(
                v => v, // Строка остается строкой
                v => v // Строка остается строкой
            )
        { }
    }
}
