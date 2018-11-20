using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace MyWeatherApp.Repositories
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public float Lat { get; private set; }
        public float Lon { get; private set; }
        public bool IsPreferred { get; set; } = false;

        [NotMapped]
        private Coordinates _coord;
        
        [NotMapped]
        public Coordinates Coord
        {
            get => _coord;
            set  { _coord = value;
                Lat = value.lat;
                Lon = value.lon;
            }
        }
    }
}