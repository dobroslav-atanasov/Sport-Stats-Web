﻿namespace SportStats.Services.Data.SportStats.Interfaces;

using global::SportStats.Data.Models.Cache;
using global::SportStats.Data.Models.Entities.SportStats;

public interface ICountriesService
{
    Task<TCountry> AddAsync<TCountry>(TCountry country);

    Task<TCountry> UpdateAsync<TCountry>(TCountry country);

    Task<WorldCountry> GetWorldCountryAsync(string code);

    Task<OGCountry> GetOlympicGameCountryAsync(string code);

    ICollection<OGCountryCacheModel> GetOGCountriesCache();
}