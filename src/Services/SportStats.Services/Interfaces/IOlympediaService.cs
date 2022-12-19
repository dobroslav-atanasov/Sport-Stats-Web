namespace SportStats.Services.Interfaces;

public interface IOlympediaService
{
    IList<int> FindAthleteNumbers(string text);

    IList<string> FindCountryCodes(string text);

    List<int> FindVenues(string text);

    int FindAthleteNumber(string text);

    string FindCountryCode(string text);
}