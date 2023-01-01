namespace SportStats.Services.Interfaces;

using SportStats.Data.Models.Enumerations;

public interface IOlympediaService
{
    IList<int> FindAthleteNumbers(string text);

    IList<string> FindCountryCodes(string text);

    List<int> FindVenues(string text);

    int FindAthleteNumber(string text);

    string FindCountryCode(string text);

    Dictionary<string, int> FindIndexes(List<string> headers);

    MedalType FindMedal(string text);
}