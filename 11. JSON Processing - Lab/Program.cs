using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;



namespace JSON_Lab
{
    public class Program
    {
        static void Main(string[] args)
        {
            var car = new Car()
            {
                Extras = new List<string>() { "Klimatronik", "4x4", "Farove" },
                ManufacturedOn = DateTime.Now,
                Model = "Golf",
                Vendor = "VW",
                Price = 12345.56M,
                Engine = new Engine() { Volume = 1.6M, HorsePower = 80 }
            };

            var options = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            Console.WriteLine(JsonConvert.SerializeObject(car,options));

        }

    }

}
