using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsConversion;

namespace UnitsConversion
{
    public static class MyExtensions
    {      
        internal static bool HasValue(this (Unit, Unit) tuple)
        {
            return tuple != (Unit.none, Unit.none);
        }
    }
}
