using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum AnimalGender
    {
        [EnumMember(Value = "MALE")]
        MALE,
        [EnumMember(Value = "FEMALE")]
        FEMALE,
        [EnumMember(Value = "OTHER")]
        OTHER
    }
}
