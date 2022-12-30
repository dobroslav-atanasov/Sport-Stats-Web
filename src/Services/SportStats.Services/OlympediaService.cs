namespace SportStats.Services;

using System.Collections.Generic;

using SportStats.Common.Constants;
using SportStats.Data.Models.Enumerations;
using SportStats.Services.Interfaces;

public class OlympediaService : IOlympediaService
{
    private readonly IRegexService regexService;

    public OlympediaService(IRegexService regexService)
    {
        this.regexService = regexService;
    }

    public int FindAthleteNumber(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            var numberMatch = this.regexService.Match(text, @"<a href=""\/athletes\/(\d+)"">");
            if (numberMatch != null)
            {
                return int.Parse(numberMatch.Groups[1].Value);
            }
        }

        return 0;
    }

    public IList<int> FindAthleteNumbers(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<int>();
        }

        var numbers = this.regexService
            .Matches(text, @"<a href=""\/athletes\/(\d+)"">")
            .Select(x => int.Parse(x.Groups[1].Value))?
            .ToList();

        return numbers;
    }

    public string FindCountryCode(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var numberMatch = this.regexService.Match(text, @"<a href=""\/countries\/(.*?)"">");
        if (numberMatch == null)
        {
            return null;
        }

        return numberMatch.Groups[1].Value;
    }

    public IList<string> FindCountryCodes(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<string>();
        }

        var codes = this.regexService
            .Matches(text, @"<a href=""\/countries\/(.*?)"">")
            .Select(x => x.Groups[1].Value)?
            .Where(x => x != "UNK")
            .ToList();

        return codes;
    }

    public Dictionary<string, int> FindIndexes(List<string> headers)
    {
        var indexes = new Dictionary<string, int>();

        for (int i = 0; i < headers.Count; i++)
        {
            var header = headers[i].ToLower();
            switch (header)
            {
                case "pos":
                    indexes[ConverterConstants.INDEX_POSITION] = i;
                    break;
                case "noc":
                    indexes[ConverterConstants.INDEX_NOC] = i;
                    break;
                case "nr":
                case "number":
                    indexes[ConverterConstants.INDEX_NR] = i;
                    break;
                case "seed":
                    indexes[ConverterConstants.INDEX_SEED] = i;
                    break;
                case "group":
                    indexes[ConverterConstants.INDEX_GROUP] = i;
                    break;
                case "lane":
                    indexes[ConverterConstants.INDEX_LANE] = i;
                    break;
                case "archer":
                case "athlete":
                case "biathlete":
                case "boarder":
                case "boat":
                case "bobsleigh":
                case "boxer":
                case "climber":
                case "competitor":
                case "competitor(s)":
                case "competitors":
                case "cyclist":
                case "cyclists":
                case "diver":
                case "divers":
                case "fencer":
                case "fencers":
                case "fighter":
                case "gymnast":
                case "gymnasts":
                case "judoka":
                case "jumper":
                case "karateka":
                case "lifter":
                case "pair":
                case "pentathlete":
                case "player":
                case "rider":
                case "shooter":
                case "skater":
                case "skier":
                case "slider":
                case "surfer":
                case "swimmer":
                case "team":
                case "triathlete":
                case "walker":
                case "wrestler":
                    indexes[ConverterConstants.INDEX_NAME] = i;
                    break;
                case "time":
                    indexes[ConverterConstants.INDEX_TIME] = i;
                    break;
                case "run 1":
                    indexes[ConverterConstants.INDEX_RUN1] = i;
                    break;
                case "run 2":
                    indexes[ConverterConstants.INDEX_RUN2] = i;
                    break;
                case "downhill":
                    indexes[ConverterConstants.INDEX_DOWNHILL] = i;
                    break;
                case "slalom":
                    indexes[ConverterConstants.INDEX_SLALOM] = i;
                    break;
                case "points":
                case "total points":
                case "tp":
                    indexes[ConverterConstants.INDEX_POINTS] = i;
                    break;
                case "10s":
                    indexes[ConverterConstants.INDEX_10S] = i;
                    break;
                case "9s":
                    indexes[ConverterConstants.INDEX_9S] = i;
                    break;
                case "xs":
                    indexes[ConverterConstants.INDEX_XS] = i;
                    break;
                case "target":
                    indexes[ConverterConstants.INDEX_TARGET] = i;
                    break;
                case "th":
                    indexes[ConverterConstants.INDEX_TH] = i;
                    break;
                case "golds":
                    indexes[ConverterConstants.INDEX_GOLDS] = i;
                    break;
                case "score":
                    indexes[ConverterConstants.INDEX_SCORE] = i;
                    break;
                case "shoot-off":
                    indexes[ConverterConstants.INDEX_SHOOT_OFF] = i;
                    break;
                case "1st half":
                case "part #1":
                    indexes[ConverterConstants.INDEX_PART_1] = i;
                    break;
                case "2nd half":
                case "part #2":
                    indexes[ConverterConstants.INDEX_PART_2] = i;
                    break;
                case "set points":
                    indexes[ConverterConstants.INDEX_SET_POINTS] = i;
                    break;
                case "ip":
                    indexes[ConverterConstants.INDEX_INDIVIDUAL_POINTS] = i;
                    break;
                case "set 1":
                case "set 1 points":
                    indexes[ConverterConstants.INDEX_SET_1] = i;
                    break;
                case "set 2":
                case "set 2 points":
                    indexes[ConverterConstants.INDEX_SET_2] = i;
                    break;
                case "set 3":
                case "set 3 points":
                    indexes[ConverterConstants.INDEX_SET_3] = i;
                    break;
                case "set 4":
                case "set 4 points":
                    indexes[ConverterConstants.INDEX_SET_4] = i;
                    break;
                case "set 5":
                case "set 5 points":
                    indexes[ConverterConstants.INDEX_SET_5] = i;
                    break;
                case "1":
                case "arrow 1":
                    indexes[ConverterConstants.INDEX_ARROW_1] = i;
                    break;
                case "2":
                case "arrow 2":
                    indexes[ConverterConstants.INDEX_ARROW_2] = i;
                    break;
                case "3":
                case "arrow 3":
                    indexes[ConverterConstants.INDEX_ARROW_3] = i;
                    break;
                case "4":
                case "arrow 4":
                    indexes[ConverterConstants.INDEX_ARROW_4] = i;
                    break;
                case "5":
                case "arrow 5":
                    indexes[ConverterConstants.INDEX_ARROW_5] = i;
                    break;
                case "6":
                case "arrow 6":
                    indexes[ConverterConstants.INDEX_ARROW_6] = i;
                    break;
                case "7":
                case "arrow 7":
                    indexes[ConverterConstants.INDEX_ARROW_7] = i;
                    break;
                case "8":
                case "arrow 8":
                    indexes[ConverterConstants.INDEX_ARROW_8] = i;
                    break;
                case "9":
                case "arrow 9":
                    indexes[ConverterConstants.INDEX_ARROW_9] = i;
                    break;
                case "10":
                case "arrow 10":
                    indexes[ConverterConstants.INDEX_ARROW_10] = i;
                    break;
                case "11":
                case "arrow 11":
                    indexes[ConverterConstants.INDEX_ARROW_11] = i;
                    break;
                case "12":
                case "arrow 12":
                    indexes[ConverterConstants.INDEX_ARROW_12] = i;
                    break;
                case "13":
                case "arrow 13":
                    indexes[ConverterConstants.INDEX_ARROW_13] = i;
                    break;
                case "14":
                case "arrow 14":
                    indexes[ConverterConstants.INDEX_ARROW_14] = i;
                    break;
                case "15":
                case "arrow 15":
                    indexes[ConverterConstants.INDEX_ARROW_15] = i;
                    break;
                case "16":
                case "arrow 16":
                    indexes[ConverterConstants.INDEX_ARROW_16] = i;
                    break;
                case "bb":
                case "balance beam":
                    indexes[ConverterConstants.INDEX_BALANCE_BEAM] = i;
                    break;
                case "fe":
                case "floor exercise":
                    indexes[ConverterConstants.INDEX_FLOOR_EXERCISE] = i;
                    break;
                case "hb":
                case "horizontal bar":
                    indexes[ConverterConstants.INDEX_HORIZONTAL_BAR] = i;
                    break;
                case "hv":
                case "horse vault":
                    indexes[ConverterConstants.INDEX_HORSE_VAULT] = i;
                    break;
                case "pb":
                case "parallel bars":
                    indexes[ConverterConstants.INDEX_PARALLEL_BARS] = i;
                    break;
                case "ph":
                case "pommelled horse":
                    indexes[ConverterConstants.INDEX_POMMELLED_HORSE] = i;
                    break;
                case "rings":
                    indexes[ConverterConstants.INDEX_RINGS] = i;
                    break;
                case "ub":
                case "uneven bars":
                    indexes[ConverterConstants.INDEX_UNEVEN_BARS] = i;
                    break;
                case "cep":
                case "cp":
                case "tcp":
                    indexes[ConverterConstants.INDEX_COMPULSORY_EXERCISES_POINTS] = i;
                    break;
                case "oep":
                case "op":
                case "top":
                    indexes[ConverterConstants.INDEX_OPTIONAL_EXERCISES_POINTS] = i;
                    break;
                case "c1ep":
                    indexes[ConverterConstants.INDEX_COMPULSORY_EXERCISES_POINTS_1] = i;
                    break;
                case "c2ep":
                    indexes[ConverterConstants.INDEX_COMPULSORY_EXERCISES_POINTS_2] = i;
                    break;
                case "o1ep":
                    indexes[ConverterConstants.INDEX_OPTIONAL_EXERCISES_POINTS_1] = i;
                    break;
                case "o2ep":
                    indexes[ConverterConstants.INDEX_OPTIONAL_EXERCISES_POINTS_2] = i;
                    break;
                case "vault 1":
                case "1jp":
                case "fj#1p":
                case "j#1p":
                case "jump 1":
                case "j-o1jp":
                    indexes[ConverterConstants.INDEX_VAULT_1] = i;
                    break;
                case "vault 2":
                case "2jp":
                case "fj#2p":
                case "j#2p":
                case "jump 2":
                case "j-o2jp":
                    indexes[ConverterConstants.INDEX_VAULT_2] = i;
                    break;
                case "fp":
                    indexes[ConverterConstants.INDEX_FINAL_POINTS] = i;
                    break;
                case "line penalty":
                    indexes[ConverterConstants.INDEX_LINE_PENALTY] = i;
                    break;
                case "other penalty":
                    indexes[ConverterConstants.INDEX_OTHER_PENALTY] = i;
                    break;
                case "penalty":
                    indexes[ConverterConstants.INDEX_PENALTY] = i;
                    break;
                case "time penalty":
                    indexes[ConverterConstants.INDEX_TIME_PENALTY] = i;
                    break;
                case "qp(50%)":
                    indexes[ConverterConstants.INDEX_QUALIFICATION_POINTS] = i;
                    break;
                case "1ep":
                    indexes[ConverterConstants.INDEX_EXERCISE_POINTS_1] = i;
                    break;
                case "2ep":
                    indexes[ConverterConstants.INDEX_EXERCISE_POINTS_2] = i;
                    break;
                case "1tt":
                    indexes[ConverterConstants.INDEX_TRIAL_TIME_1] = i;
                    break;
                case "2tt":
                    indexes[ConverterConstants.INDEX_TRIAL_TIME_2] = i;
                    break;
                case "3tt":
                    indexes[ConverterConstants.INDEX_TRIAL_TIME_3] = i;
                    break;
                case "j-op":
                    indexes[ConverterConstants.INDEX_JUMP_OFF_POINTS] = i;
                    break;
                case "d score":
                    indexes[ConverterConstants.INDEX_D_SCORE] = i;
                    break;
                case "e score":
                    indexes[ConverterConstants.INDEX_E_SCORE] = i;
                    break;
                case "d()":
                    indexes[ConverterConstants.INDEX_DISTANCE] = i;
                    break;
                case "qop":
                    indexes[ConverterConstants.INDEX_QUALIFYING_OPTIONAL_POINTS] = i;
                    break;
                case "height":
                    indexes[ConverterConstants.INDEX_HEIGHT] = i;
                    break;
                case "100p":
                    indexes[ConverterConstants.INDEX_POINTS_100] = i;
                    break;
                case "ap":
                case "apparatus":
                case "pap":
                    indexes[ConverterConstants.INDEX_APPARATUS_POINTS] = i;
                    break;
                case "atp(50%)":
                    indexes[ConverterConstants.INDEX_ADJUSTED_TEAM_POINS] = i;
                    break;
                case "gep":
                    indexes[ConverterConstants.INDEX_GROUP_EXERCISES_POINTS] = i;
                    break;
                case "ljp":
                    indexes[ConverterConstants.INDEX_LONG_JUMP_POINTS] = i;
                    break;
                case "spp":
                    indexes[ConverterConstants.INDEX_SHOT_PUT_POINTS] = i;
                    break;
                case "tdp":
                    indexes[ConverterConstants.INDEX_TEAM_DRILL_POINTS] = i;
                    break;
                case "tpp":
                    indexes[ConverterConstants.INDEX_TEAM_PRECISION_POINTS] = i;
                    break;
                case "":
                    // TODO
                    break;
            }
        }

        return indexes;
    }

    public MedalType FindMedal(string text)
    {
        var match = this.regexService.Match(text, @"<span class=""(?:Gold|Silver|Bronze)"">(Gold|Silver|Bronze)<\/span>");
        if (match != null)
        {
            var medalType = match.Groups[1].Value.ToLower();
            switch (medalType)
            {
                case "gold": return MedalType.Gold;
                case "silver": return MedalType.Silver;
                case "bronze": return MedalType.Bronze;
            }
        }

        return MedalType.None;
    }

    public List<int> FindVenues(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<int>();
        }

        var venues = this.regexService
            .Matches(text, @"\/venues\/(\d+)")
            .Select(x => int.Parse(x.Groups[1].Value))?
            .ToList();

        return venues;
    }
}