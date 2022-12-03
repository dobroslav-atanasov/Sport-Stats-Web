namespace SportStats.Services.Data.SportStats.Interfaces;

public interface IAthleteCountryService : IAddable
{
    bool AthleteCountryExists(int athleteId, int countryId);
}