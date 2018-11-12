using System.Threading.Tasks;

namespace MyWeatherApp
{
    public class LimitProxy : IModel
    {
        public readonly string _locationId;
        public readonly int _daysAhead;
        private Model _realModel;
        
        public LimitProxy(string locationId, int daysAhead)
        {
            _locationId = locationId;
            _daysAhead = daysAhead;
            _realModel = new Model(_locationId, _daysAhead);
        }
        
        
        public Task<Forecast> GetForecast()
        {
            //проверка что последний запрос был более чем 10 мин назад
            
            return _realModel.GetForecast();
        }
    }
}