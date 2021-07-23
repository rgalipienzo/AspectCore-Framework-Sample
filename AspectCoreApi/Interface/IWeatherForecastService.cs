using AspectCoreApi.Model;
using System.Collections.Generic;

namespace AspectCoreApi.Interface
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
    }
}
