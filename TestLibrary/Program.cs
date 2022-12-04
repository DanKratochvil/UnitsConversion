using UnitsConversion;

var convert = new ConvertUnits();
try
{
    double result = convert.Convert("24     kiloinches ", " decimeter  ");
    Console.WriteLine($"24 kiloinch decimeter {result}");

    result = convert.Convert("1542decimeters ","kiloinches");
    Console.WriteLine($"1542 decimeters kiloinches {result}");

    result = convert.Convert("3yard", "inch");
    Console.WriteLine($"3 yard inch {result}");

    result = convert.Convert("13 kilometer", "decimeter");
    Console.WriteLine($"13 kilometer meter {result}");

    result = convert.Convert("10 stone", "kilogram");
    Console.WriteLine($"10 stone kilogram {result}");

    result = convert.Convert("3 stone", "pound");
    Console.WriteLine($"3 stone pound {result}");

    result = convert.Convert("3 gallon", "hectoliter");
    Console.WriteLine($"3 gallon hectoliter { result}");

    result = convert.Convert("35,74 fahrenheit", "celsius");
    Console.WriteLine($"35.74 fahrenheit celsius {result}");

    result = convert.Convert("253 megabyte", "gigabyte");
    Console.WriteLine($"253 megabyte gigabyte {result}");    
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
