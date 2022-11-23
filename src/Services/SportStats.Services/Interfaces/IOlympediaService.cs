﻿namespace SportStats.Services.Interfaces;

public interface IOlympediaService
{
    IList<int> FindAthleteNumbers(string text);

    IList<string> FindCountryCodes(string text);
}