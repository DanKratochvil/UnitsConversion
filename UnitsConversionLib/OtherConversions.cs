using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitsConversion
{
    internal static class OtherConversions
    {
        static Dictionary<Prefix, double> BytePrefixes { get; } = new Dictionary<Prefix, double>()
        {
            {Prefix.yotta, Math.Pow(1024,8) },
            {Prefix.zetta, Math.Pow(1024,7) },
            {Prefix.exa,  Math.Pow(1024,6) },
            {Prefix.peta, Math.Pow(1024,5) },
            {Prefix.tera, Math.Pow(1024,4) },
            {Prefix.giga, Math.Pow(1024,3) },
            {Prefix.mega, Math.Pow(1024,2) },
            {Prefix.kilo, 1024   }
        };

        public static double ConvertTemperature(Unit sourceUnit, Unit destUnit, double amount)
        {
            if (sourceUnit == Unit.celsius && destUnit == Unit.fahrenheit)
                return amount * 1.8 + 32;
            else if (sourceUnit == Unit.fahrenheit && destUnit == Unit.celsius)
                return (amount - 32) / 1.8;
            else if (sourceUnit == destUnit)
                return amount;
            else
                throw new ArgumentException($"Can't convert {sourceUnit} to {destUnit}");
        }

        public static double ConvertByte(Unit sourceUnit, Unit destUnit, Prefix sourcePrefix, Prefix destPrefix, double amount)
        {
            double sourcePrefixByteMult = (sourcePrefix != Prefix.none) ? BytePrefixes[sourcePrefix] : 1;
            double destPrefixByteMult = (destPrefix != Prefix.none) ? BytePrefixes[destPrefix] : 1;

            amount *= sourcePrefixByteMult / destPrefixByteMult;
            if (sourceUnit == Unit.@byte && destUnit == Unit.bit)
                return 8 * amount;
            else if (sourceUnit == Unit.bit && destUnit == Unit.@byte)
                return amount / 8;
            else if (sourceUnit == destUnit)
                return amount;
            else
                throw new ArgumentException($"Can't convert {sourceUnit} to {destUnit}");
        }
    }
}
