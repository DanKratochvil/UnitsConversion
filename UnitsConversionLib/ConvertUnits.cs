using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnitsConversion
{


    public class ConvertUnits
    {
        Dictionary<Prefix, double> Prefixes { get; } = new Dictionary<Prefix, double>()
        {
            {Prefix.yotta, 1E24},
            {Prefix.zetta, 1E21},
            {Prefix.exa,  1E18 },
            {Prefix.peta, 1E15 },
            {Prefix.tera, 1E12 },
            {Prefix.giga, 1E9  },
            {Prefix.mega, 1E6  },
            {Prefix.kilo, 1E3  },
            {Prefix.hecto,1E2 },
            {Prefix.deca, 1E1  },
            {Prefix.deci, 1E-1 },
            {Prefix.centi,1E-2 },
            {Prefix.mili, 1E-3 },
            {Prefix.micro, 1E-6 },
            {Prefix.nano, 1E-9 },
            {Prefix.pico, 1E-12 },
            {Prefix.femto, 1E-15 },
        };

        Dictionary<(Unit unitImperial, Unit unitSI), double> Conversion { get; } = new Dictionary<(Unit unitImperial, Unit unitSI), double>()
        {
            {(Unit.inch, Unit.meter), 0.0254 },
            {(Unit.foot, Unit.meter),  0.3048 },
            {(Unit.feet,Unit.meter),  0.3048 },
            {(Unit.yard,Unit.meter), 0.9144},

            {(Unit.ounce,Unit.gram), 28.349523125},
            {(Unit.pound,Unit.gram), 453.59237},
            {(Unit.stone,Unit.gram), 6350.29318},
            {(Unit.quarter,Unit.gram), 12700.58636 },

            {(Unit.gill,Unit.liter), 0.1416},
            {(Unit.pint,Unit.liter), 0.5696},
            {(Unit.quart,Unit.liter), 1.1360},
            {(Unit.gallon,Unit.liter), 4.54371 }
        };

        public double Convert(string sourceValue, string destValue)
        {
            sourceValue = sourceValue.ToLower().Trim();
            destValue = destValue.ToLower().Trim();

            (Prefix sourcePrefix, Unit sourceUnit, double amount) = SplitSourceValue(sourceValue);
            (Prefix destPrefix, Unit destUnit) = SplitPrefix(destValue);
            double sourcePrefixMult = (sourcePrefix != Prefix.none) ? Prefixes[sourcePrefix] : 1;
            double destPrefixMult = (destPrefix != Prefix.none) ? Prefixes[destPrefix] : 1;

            if (sourceUnit == Unit.celsius || sourceUnit == Unit.fahrenheit)
                return OtherConversions.ConvertTemperature(sourceUnit, destUnit, amount);
            if (sourceUnit == Unit.@byte || sourceUnit == Unit.bit)
                return OtherConversions.ConvertByte(sourceUnit, destUnit, sourcePrefix, destPrefix, amount);

            //try to find 2 keys of Conversion Dictionary with same Imperial/SI unit as sourceValue/destValue   
            //2 keys found are used to identify type of conversion: Imperial->SI, SI->Imperial, Imperial->Imperial SI->SI
            var sourceImperial = Conversion.Keys.FirstOrDefault(k => k.unitImperial == sourceUnit);
            var destImperial = Conversion.Keys.FirstOrDefault(k => k.unitImperial == destUnit);
            var sourceSI = Conversion.Keys.FirstOrDefault(k => k.unitSI == sourceUnit);
            var destSI = Conversion.Keys.FirstOrDefault(k => k.unitSI == destUnit);

            if (sourceImperial.HasValue() && destSI.HasValue())
                return sourcePrefixMult * ConvertImperialToSI(sourceUnit, destUnit, amount) / destPrefixMult;
            else if (sourceSI.HasValue() && destImperial.HasValue())
                return sourcePrefixMult * ConvertSIToImperial(sourceUnit, destUnit, amount) / destPrefixMult;
            else if (sourceImperial.HasValue() && destImperial.HasValue() && sourceImperial.unitSI == destImperial.unitSI)
                return sourcePrefixMult * ConvertImperialToImperial(sourceUnit, destUnit, sourceImperial.unitSI, amount) / destPrefixMult;
            else if (sourceImperial.HasValue() && destImperial.HasValue() && sourceImperial.unitSI != destImperial.unitSI)
                throw new ArgumentException($"Can't convert {sourceUnit} to {destUnit}");
            else if (sourceSI.HasValue() && destSI.HasValue() && sourceSI.unitSI == destSI.unitSI)
                return sourcePrefixMult * amount / destPrefixMult;
            else if (sourceSI.HasValue() && destSI.HasValue() && sourceSI.unitSI != destSI.unitSI)
                throw new ArgumentException($"Can't convert {sourceUnit} to {destUnit}");
            else
                throw new ArgumentException($"{sourceUnit} or {destUnit} is not Conversion Dictionary");
        }

        private double ConvertImperialToSI(Unit sourceUnit, Unit destUnit, double amount)
        {
            if (Conversion.ContainsKey((sourceUnit, destUnit)))
                return amount * Conversion[(sourceUnit, destUnit)];
            else
                throw new ArgumentException($"Conversion from {sourceUnit} to {destUnit}  is not in Conversion Dictionary");
        }

        private double ConvertSIToImperial(Unit sourceUnit, Unit destUnit, double amount)
        {
            if (Conversion.ContainsKey((destUnit, sourceUnit)))
                return amount / Conversion[(destUnit, sourceUnit)];
            else
                throw new ArgumentException($"Conversion from {sourceUnit} to {destUnit}  is not in Conversion Dictionary");
        }

        private double ConvertImperialToImperial(Unit sourceUnit, Unit destUnit, Unit UnitSI, double amount)
        {
            if (Conversion.ContainsKey((sourceUnit, UnitSI)) && Conversion.ContainsKey((destUnit, UnitSI)))
                return amount * Conversion[(sourceUnit, UnitSI)] / Conversion[(destUnit, UnitSI)];
            else
                throw new ArgumentException($"{sourceUnit} or {destUnit} is not in Conversion Dictionary");
        }

        private (Prefix prefix, Unit unit, double amount) SplitSourceValue(string value)
        {
            string pattern = @"^(\d+(\.|\,)?\d*)\s*([a-z\s]+)";
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(value);
            if (!match.Success)
                throw new ArgumentException("wrong input {value} for convert");

            string unitWithPrefix = match.Groups[3].Value;
            if (!double.TryParse(match.Groups[1].Value, out double amount))
                throw new ArgumentException($"wrong { value } format");

            (Prefix prefix, Unit unit) = SplitPrefix(unitWithPrefix);
            return (prefix, unit, amount);
        }

        private (Prefix prefix, Unit unit) SplitPrefix(string value)
        {
            Prefix prefix = Prefixes.Keys.FirstOrDefault(p => value.StartsWith(p.ToString()));
            if (prefix != Prefix.none)
                value = value.Replace(prefix.ToString(), "");

            string pattern_s = "s$";
            string pattern_es = "es$";
            Unit unit;

            //tries to remove (e)s from end of unit
            if (Enum.TryParse(value, out unit))
                return (prefix, unit);
            else if (Enum.TryParse(Regex.Replace(value, pattern_s, ""), out unit))
                return (prefix, unit);
            else if (Enum.TryParse(Regex.Replace(value, pattern_es, ""), out unit))
                return (prefix, unit);
            else
                throw new ArgumentException($"{value} is not in enum Unit");
        }
    }
}
