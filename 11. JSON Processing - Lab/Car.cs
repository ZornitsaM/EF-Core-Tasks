using System;
using System.Collections.Generic;

namespace JSON_Lab
{
    class Car
    {
        public string Vendor { get; set; }
        public string Model { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public DateTime ManufacturedOn { get; set; }

        public List<string> Extras { get; set; }

        public Engine Engine { get; set; }

    }

}
