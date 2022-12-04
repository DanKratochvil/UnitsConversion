using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitsConversion
{
    enum Unit
    {
        none, inch, hand, foot, feet, yard, mile, meter,
        ounce, pound, stone, quarter, gram,
        gill, pint, quart, gallon, liter,
        celsius, fahrenheit,
        @byte, bit
    }

    enum Prefix { none, yotta, zetta, exa, peta, tera, giga, mega, kilo, hecto, deca, deci, centi, mili, micro, nano, pico, femto }
}
