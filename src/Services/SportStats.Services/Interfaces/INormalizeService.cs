namespace SportStats.Services.Interfaces;
public interface INormalizeService
{
    string MapOlympicGamesCountriesAndWorldCountries(string code);

    string NormalizeHostCityName(string hostCity);

    string NormalizeEventName(string name);

    string CleanEventName(string name);
}