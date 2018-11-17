using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeatherApp.WeatherModels
{
    public class CurrentWeather : IWeather
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        
        public DateTime Date{ get; private set; }
        
        [NotMapped]
        private string _dt_txt { get; set; }
 
        [NotMapped]
        public string Dt_txt
        {
            get => _dt_txt;
            set  { _dt_txt = value;
                Date = DateTime.Parse(value);
            }
        }
    }
}