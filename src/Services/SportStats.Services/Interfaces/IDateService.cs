﻿namespace SportStats.Services.Interfaces;

public interface IDateService
{
    Tuple<DateTime?, DateTime?> MatchStartAndEndDate(string text);
}