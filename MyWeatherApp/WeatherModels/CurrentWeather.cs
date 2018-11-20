using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeatherApp.WeatherModels
{
    public class CurrentWeather : IWeather
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        
        public DateTime Date{ get; private set; }
        
        [NotMapped]
        private string DtTxt { get; set; }
 
        [NotMapped]
        public string Dt_txt
        {
            get => DtTxt;
            set  { DtTxt = value;
                Date = DateTime.Parse(value);
            }
        }
    }
}