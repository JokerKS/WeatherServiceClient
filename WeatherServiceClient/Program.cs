using System;
using System.ServiceModel;
using System.Threading;
using WeatherServiceClient.WeatherService;

namespace WeatherServiceClient
{
    public class CallbackHandler : IWeatherCallback 
    {
        static InstanceContext site = new InstanceContext(new CallbackHandler());
        static WeatherClient proxy = new WeatherClient(site);

        public void WeatherUpdate(OpenWeather weather)
        {
            if (weather != null)
            {
                Console.WriteLine("{0}: \n\tTemperature = {1}", weather.City, weather.Forecast.Temp);
                Console.WriteLine("\tPressure = {0}", weather.Forecast.Pressure);
                Console.WriteLine("\tHumidity = {0}", weather.Forecast.Humidity);
                Console.WriteLine("\tWind: speed - {0}, direction - {1}", weather.Wind.Speed, weather.Wind.Direction);
                Console.WriteLine("\tCloudiness - {0}", weather.Clouds.Cloudiness);
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                try
                {
                    Console.WriteLine("Click <Enter> to close application");
                    Console.WriteLine("Entering to weather service...");
                    string uniqueId = proxy.LogInToService("admin", "admin");
                    Console.WriteLine("The weather: ");
                    //proxy.RegisterForUpdatesByCityName(uniqueId,"Kiev", "UA");
                    proxy.RegisterForUpdatesByCityId(uniqueId, proxy.GetCityIdByName("Rzeszow", null));
                }
                catch (FaultException<CityNotFound> e)
                {
                    Console.WriteLine(e.Detail.Message);
                }
                catch (FaultException<NotCorrectLoginOrPassword> e)
                {
                    Console.WriteLine(e.Detail.Message);
                }
                catch (FaultException<UniqueClientIdNotFound> e)
                {
                    Console.WriteLine(e.Detail.Message);
                }
                catch (FaultException<CountryNotFound> e)
                {
                    Console.WriteLine(e.Detail.Message);
                }
                catch (FaultException<ServerHasNoWeatherData> e)
                {
                    Console.WriteLine(e.Detail.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.ReadLine();
            }
        }
    }
}
