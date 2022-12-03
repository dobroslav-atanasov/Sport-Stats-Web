namespace SportStats.Services.Interfaces;

using SportStats.Data.Models.Enumerations;

public interface INormalizeService
{
    string MapOlympicGamesCountriesAndWorldCountries(string code);

    string NormalizeHostCityName(string hostCity);

    string NormalizeEventName(string name, int gameYear, string disciplineName);

    string ReplaceNonEnglishLetters(string name);

    AthleteType MapAthleteType(string text);
}