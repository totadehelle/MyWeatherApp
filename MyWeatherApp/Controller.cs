using System.Threading;

namespace MyWeatherApp
{
    public class Controller
    {
        private Model _model = new Model();
        private View _view = new View();
        
        public Controller(string[] args)
        {
            _model = new Model();
            _view = new View();
        }
    }
}