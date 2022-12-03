namespace SportStats.Services;

using System.Text.RegularExpressions;

using SportStats.Data.Models.Enumerations;
using SportStats.Services.Interfaces;

public class NormalizeService : INormalizeService
{
    public AthleteType MapAthleteType(string text)
    {
        text = text.Replace(" •", ",");
        var type = AthleteType.None;
        switch (text)
        {
            case "Competed in Intercalated Games":
            case "Competed in Intercalated Games, Non-starter":
            case "Competed in Olympic Games":
            case "Competed in Olympic Games (non-medal events)":
            case "Competed in Olympic Games (non-medal events), Competed in Intercalated Games":
            case "Competed in Olympic Games (non-medal events), Non-starter":
            case "Competed in Olympic Games, Competed in Intercalated Games":
            case "Competed in Olympic Games, Competed in Intercalated Games, Non-starter":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events)":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Competed in Intercalated Games":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Non-starter":
            case "Competed in Olympic Games, Competed in Youth Olympic Games":
            case "Competed in Olympic Games, Competed in Youth Olympic Games (non-medal events)":
            case "Competed in Olympic Games, Competed in Youth Olympic Games, Non-starter":
            case "Competed in Olympic Games, Non-starter":
            case "Competed in Olympic Games, Other":
            case "Competed in Youth Olympic Games":
            case "Competed in Youth Olympic Games, Non-starter":
            case "Non-starter":
            case "Non-starter, Other":
            case "Other":
                type = AthleteType.Athlete;
                break;
            case "Coach":
                type = AthleteType.Coach;
                break;
            case "Referee":
                type = AthleteType.Referee;
                break;
            case "Coach, Other":
            case "Competed in Olympic Games (non-medal events), Coach":
            case "Competed in Olympic Games, Coach":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Coach":
            case "Competed in Olympic Games, Non-starter, Coach":
            case "Non-starter, Coach":
                type = AthleteType.Athlete | AthleteType.Coach;
                break;
            case "Coach, Referee":
                type = AthleteType.Coach | AthleteType.Referee;
                break;
            case "Competed in Olympic Games, Competed in Intercalated Games, Non-starter, Referee":
            case "Competed in Olympic Games, Competed in Intercalated Games, Referee":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Competed in Intercalated Games, Referee":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Referee":
            case "Competed in Olympic Games, Non-starter, Referee":
            case "Competed in Olympic Games, Referee":
            case "Non-starter, Referee":
            case "Referee, Other":
                type = AthleteType.Athlete | AthleteType.Referee;
                break;
            case "Competed in Olympic Games, Coach, Referee":
            case "Competed in Olympic Games, Non-starter, Coach, Referee":
            case "Non-starter, Coach, Referee":
                type = AthleteType.Athlete | AthleteType.Coach | AthleteType.Referee;
                break;
            case "Competed in Olympic Games, IOC member":
            case "Competed in Olympic Games, Non-starter, IOC member":
                type = AthleteType.Athlete | AthleteType.IOCMember;
                break;
            case "Competed in Olympic Games, IOC member, Referee":
                type = AthleteType.Athlete | AthleteType.Referee | AthleteType.IOCMember;
                break;
            case "IOC member":
                type = AthleteType.IOCMember;
                break;
            case "IOC member, Coach":
                type = AthleteType.Coach | AthleteType.IOCMember;
                break;
            case "IOC member, Referee":
                type = AthleteType.Referee | AthleteType.IOCMember;
                break;
        }

        return type;
    }

    public string MapOlympicGamesCountriesAndWorldCountries(string code)
    {
        return code switch
        {
            "AFG" => "AFG",
            "ALB" => "ALB",
            "ALG" => "DZA",
            "ASA" => "ASM",
            "AND" => "AND",
            "ANG" => "AGO",
            "ANT" => "ATG",
            "ARG" => "ARG",
            "ARM" => "ARM",
            "ARU" => "ABW",
            "AUS" => "AUS",
            "AUT" => "AUT",
            "AZE" => "AZE",
            "BAH" => "BHS",
            "BRN" => "BHR",
            "BAN" => "BGD",
            "BAR" => "BRB",
            "BLR" => "BLR",
            "BEL" => "BEL",
            "BIZ" => "BLZ",
            "BEN" => "BEN",
            "BER" => "BMU",
            "BHU" => "BTN",
            "BOL" => "BOL",
            "BIH" => "BIH",
            "BOT" => "BWA",
            "BRA" => "BRA",
            "IVB" => "VGB",
            "BRU" => "BRN",
            "BUL" => "BGR",
            "BUR" => "BFA",
            "BDI" => "BDI",
            "CAM" => "KHM",
            "CMR" => "CMR",
            "CAN" => "CAN",
            "CPV" => "CPV",
            "CAY" => "CYM",
            "CAF" => "CAF",
            "CHA" => "TCD",
            "CHI" => "CHL",
            "COL" => "COL",
            "COM" => "COM",
            "CGO" => "COG",
            "COK" => "COK",
            "CRC" => "CRI",
            "CIV" => "CIV",
            "CRO" => "HRV",
            "CUB" => "CUB",
            "CYP" => "CYP",
            "CZE" => "CZE",
            "PRK" => "PRK",
            "COD" => "COD",
            "DEN" => "DNK",
            "DJI" => "DJI",
            "DMA" => "DMA",
            "DOM" => "DOM",
            "ECU" => "ECU",
            "EGY" => "EGY",
            "ESA" => "SLV",
            "GEQ" => "GNQ",
            "ERI" => "ERI",
            "EST" => "EST",
            "SWZ" => "SWZ",
            "ETH" => "ETH",
            "FSM" => "FSM",
            "FIJ" => "FJI",
            "FIN" => "FIN",
            "FRA" => "FRA",
            "GAB" => "GAB",
            "GEO" => "GEO",
            "GER" => "DEU",
            "GHA" => "GHA",
            "GBR" => "GBR",
            "GRE" => "GRC",
            "GRN" => "GRD",
            "GUM" => "GUM",
            "GUA" => "GTM",
            "GUI" => "GIN",
            "GBS" => "GNB",
            "GUY" => "GUY",
            "HAI" => "HTI",
            "HON" => "HND",
            "HKG" => "HKG",
            "HUN" => "HUN",
            "ISL" => "ISL",
            "IND" => "IND",
            "INA" => "IDN",
            "IRQ" => "IRQ",
            "IRL" => "IRL",
            "IRI" => "IRN",
            "ISR" => "ISR",
            "ITA" => "ITA",
            "JAM" => "JAM",
            "JPN" => "JPN",
            "JOR" => "JOR",
            "KAZ" => "KAZ",
            "KEN" => "KEN",
            "KSA" => "SAU",
            "KIR" => "KIR",
            "KOS" => "UNK",
            "KUW" => "KWT",
            "KGZ" => "KGZ",
            "LAO" => "LAO",
            "LAT" => "LVA",
            "LBN" => "LBN",
            "LES" => "LSO",
            "LBR" => "LBR",
            "LBA" => "LBY",
            "LIE" => "LIE",
            "LTU" => "LTU",
            "LUX" => "LUX",
            "MAD" => "MDG",
            "MAW" => "MWI",
            "MAS" => "MYS",
            "MDV" => "MDV",
            "MLI" => "MLI",
            "MLT" => "MLT",
            "MHL" => "MHL",
            "MTN" => "MRT",
            "MRI" => "MUS",
            "MEX" => "MEX",
            "MON" => "MCO",
            "MGL" => "MNG",
            "MNE" => "MNE",
            "MAR" => "MAR",
            "MOZ" => "MOZ",
            "MYA" => "MMR",
            "NAM" => "NAM",
            "NRU" => "NRU",
            "NEP" => "NPL",
            "NED" => "NLD",
            "NZL" => "NZL",
            "NCA" => "NIC",
            "NIG" => "NER",
            "NGR" => "NGA",
            "MKD" => "MKD",
            "NOR" => "NOR",
            "OMA" => "OMN",
            "PAK" => "PAK",
            "PLW" => "PLW",
            "PLE" => "PSE",
            "PAN" => "PAN",
            "PNG" => "PNG",
            "PAR" => "PRY",
            "CHN" => "CHN",
            "PER" => "PER",
            "PHI" => "PHL",
            "POL" => "POL",
            "POR" => "PRT",
            "PUR" => "PRI",
            "QAT" => "QAT",
            "KOR" => "KOR",
            "MDA" => "MDA",
            "ROU" => "ROU",
            "RUS" => "RUS",
            "RWA" => "RWA",
            "SKN" => "KNA",
            "LCA" => "LCA",
            "VIN" => "VCT",
            "SAM" => "WSM",
            "SMR" => "SMR",
            "STP" => "STP",
            "SEN" => "SEN",
            "SRB" => "SRB",
            "SEY" => "SYC",
            "SLE" => "SLE",
            "SGP" => "SGP",
            "SVK" => "SVK",
            "SLO" => "SVN",
            "SOL" => "SLB",
            "SOM" => "SOM",
            "RSA" => "ZAF",
            "SSD" => "SSD",
            "ESP" => "ESP",
            "SRI" => "LKA",
            "SUD" => "SDN",
            "SUR" => "SUR",
            "SWE" => "SWE",
            "SUI" => "CHE",
            "SYR" => "SYR",
            "TJK" => "TJK",
            "THA" => "THA",
            "GAM" => "GMB",
            "TLS" => "TLS",
            "TOG" => "TGO",
            "TGA" => "TON",
            "TTO" => "TTO",
            "TUN" => "TUN",
            "TUR" => "TUR",
            "TKM" => "TKM",
            "TUV" => "TUV",
            "UGA" => "UGA",
            "UKR" => "UKR",
            "UAE" => "ARE",
            "TAN" => "TZA",
            "USA" => "USA",
            "ISV" => "VIR",
            "URU" => "URY",
            "UZB" => "UZB",
            "VAN" => "VUT",
            "VEN" => "VEN",
            "VIE" => "VNM",
            "YEM" => "YEM",
            "ZAM" => "ZMB",
            "ZIM" => "ZWE",
            _ => null
        };
    }

    public string NormalizeEventName(string name, int gameYear, string disciplineName)
    {
        name = Regex.Replace(name, @"(\d+)\s+(\d+)", me =>
        {
            return $"{me.Groups[1].Value.Trim()}{me.Groups[2].Value.Trim()}";
        });

        name = Regex.Replace(name, @"(\d+),(\d+)", me =>
        {
            return $"{me.Groups[1].Value.Trim()}{me.Groups[2].Value.Trim()}";
        });

        name = name.Replace(" x ", "x")
            .Replace("82½", "82.5")
            .Replace("67½", "67.5")
            .Replace("333⅓", "333 1/3")
            .Replace(" × ", "x")
            .Replace("¼", "1/4")
            .Replace("⅓", "1/3")
            .Replace("½", "1/2")
            .Replace("²", string.Empty)
            .Replace("kilometer", "kilometers")
            .Replace("metres", "meters")
            .Replace("kilometres", "kilometers")
            .Replace("≤", "-")
            .Replace(">", "+");

        name = name.Replace(" / ", "/")
            .Replace(" meters", "m")
            .Replace(" kilometers", "km")
            .Replace(" miles", "miles")
            .Replace(" mile", "mile")
            .Replace(" km", "km")
            .Replace("Pommelled Horse", "Pommel Horse")
            .Replace("Teams", "Team")
            .Replace("Horse Vault", "Vault")
            .Replace("Alpine Combined", "Combined")
            .Replace("Super Combined", "Combined")
            .Replace("Birds", "Bird")
            .Replace("Pole Archery", "Fixed")
            .Replace("Apparatus Work and Field Sports", string.Empty)
            .Replace("Individual All-Around, Apparatus Work", "Triathlon")
            .Replace("Individual All-Around, 4 Events", "Combined")
            .Replace("European System", string.Empty)
            .Replace("Four/Five", "Four")
            .Replace("Canadian Singles", "C-1")
            .Replace("Canadian Doubles", "C-2")
            .Replace("Kayak Singles", "K-1")
            .Replace("Kayak Doubles", "K-2")
            .Replace("Kayak Fours", "K-4")
            .Replace("Kayak Relay", "K-1")
            .Replace("Two-Man Teams With Cesta", "Team")
            .Replace("Eights", "Eight")
            .Replace("Coxed Fours", "Coxed Four")
            .Replace("Coxed Pairs", "Coxed Pair")
            .Replace("Coxless Fours", "Coxless Four")
            .Replace("Coxless Pairs", "Coxless Pair")
            .Replace("Covered Courts", "Indoor")
            //.Replace("", "")
            //.Replace("", "")
            .Replace("Target Archery", "Moving Bird");

        if (gameYear == 1924 && disciplineName == "Artistic Gymnastics" && name == "Side Horse, Men")
        {
            name = "Pommel Horse, Men";
        }

        return name;
    }

    public string NormalizeHostCityName(string hostCity)
    {
        return hostCity switch
        {
            "Athina" => "Athens",
            "Antwerpen" => "Antwerp",
            "Ciudad de México" => "Mexico City",
            "Moskva" => "Moscow",
            "Sankt Moritz" => "St. Moritz",
            "Roma" => "Rome",
            "München" => "Munich",
            "Montréal" => "Montreal",
            "Torino" => "Turin",
            _ => hostCity
        };
    }

    public string ReplaceNonEnglishLetters(string name)
    {
        name = name.Replace("-", "-")
            .Replace("‐", "-")
            .Replace("–", "-")
            .Replace(",", string.Empty)
            .Replace(".", string.Empty)
            .Replace("'", string.Empty)
            .Replace("’", string.Empty)
            .Replace("(", string.Empty)
            .Replace(")", string.Empty)
            .Replace("`", string.Empty)
            .Replace("а", "a")
            .Replace("А", "A")
            .Replace("і", "i")
            .Replace("о", "o")
            .Replace("á", "а")
            .Replace("Á", "А")
            .Replace("à", "а")
            .Replace("À", "А")
            .Replace("ă", "а")
            .Replace("ằ", "а")
            .Replace("â", "а")
            .Replace("Â", "А")
            .Replace("ấ", "а")
            .Replace("ầ", "а")
            .Replace("ẩ", "а")
            .Replace("å", "а")
            .Replace("Å", "А")
            .Replace("ä", "а")
            .Replace("Ä", "А")
            .Replace("ã", "а")
            .Replace("ą", "а")
            .Replace("ā", "а")
            .Replace("Ā", "А")
            .Replace("ả", "а")
            .Replace("ạ", "а")
            .Replace("ặ", "а")
            .Replace("ậ", "а")
            .Replace("æ", "ае")
            .Replace("Æ", "Ae")
            .Replace("ć", "c")
            .Replace("Ć", "C")
            .Replace("č", "c")
            .Replace("Č", "C")
            .Replace("ç", "c")
            .Replace("Ç", "C")
            .Replace("ď", "d")
            .Replace("Ď", "D")
            .Replace("đ", "d")
            .Replace("Đ", "D")
            .Replace("ð", "d")
            .Replace("Ð", "D")
            .Replace("é", "e")
            .Replace("É", "E")
            .Replace("è", "e")
            .Replace("È", "E")
            .Replace("ĕ", "e")
            .Replace("ê", "e")
            .Replace("Ê", "E")
            .Replace("ế", "e")
            .Replace("ề", "e")
            .Replace("ễ", "e")
            .Replace("ể", "e")
            .Replace("ě", "e")
            .Replace("ë", "e")
            .Replace("ė", "e")
            .Replace("ę", "e")
            .Replace("ē", "e")
            .Replace("Ē", "E")
            .Replace("ệ", "e")
            .Replace("ə", "e")
            .Replace("Ə", "E")
            .Replace("Ǵ", "G")
            .Replace("ğ", "g")
            .Replace("ģ", "g")
            .Replace("Ģ", "G")
            .Replace("í", "i")
            .Replace("Í", "I")
            .Replace("ì", "i")
            .Replace("î", "i")
            .Replace("ï", "i")
            .Replace("İ", "I")
            .Replace("ī", "i")
            .Replace("ị", "i")
            .Replace("ı", "i")
            .Replace("ķ", "k")
            .Replace("Ķ", "K")
            .Replace("ľ", "l")
            .Replace("Ľ", "L")
            .Replace("ļ", "l")
            .Replace("ł", "l")
            .Replace("Ł", "L")
            .Replace("ń", "n")
            .Replace("ň", "n")
            .Replace("ñ", "n")
            .Replace("ņ", "n")
            .Replace("Ņ", "N")
            .Replace("ó", "o")
            .Replace("Ó", "O")
            .Replace("ò", "o")
            .Replace("ô", "o")
            .Replace("ố", "o")
            .Replace("ồ", "o")
            .Replace("ỗ", "o")
            .Replace("ö", "o")
            .Replace("Ö", "O")
            .Replace("ő", "o")
            .Replace("Ő", "O")
            .Replace("õ", "o")
            .Replace("Õ", "O")
            .Replace("ø", "o")
            .Replace("Ø", "O")
            .Replace("ơ", "o")
            .Replace("ớ", "o")
            .Replace("ờ", "o")
            .Replace("ọ", "o")
            .Replace("œ", "oe")
            .Replace("ř", "r")
            .Replace("Ř", "R")
            .Replace("ś", "s")
            .Replace("Ś", "S")
            .Replace("š", "s")
            .Replace("Š", "S")
            .Replace("ş", "s")
            .Replace("Ş", "S")
            .Replace("ș", "s")
            .Replace("Ș", "S")
            .Replace("ß", "ss")
            .Replace("ť", "t")
            .Replace("Ť", "T")
            .Replace("ţ", "t")
            .Replace("Ţ", "T")
            .Replace("ț", "t")
            .Replace("Ț", "T")
            .Replace("ú", "u")
            .Replace("Ú", "U")
            .Replace("ù", "u")
            .Replace("û", "u")
            .Replace("ů", "u")
            .Replace("ü", "u")
            .Replace("Ü", "U")
            .Replace("ű", "u")
            .Replace("ũ", "u")
            .Replace("ū", "u")
            .Replace("Ū", "U")
            .Replace("ủ", "u")
            .Replace("ư", "u")
            .Replace("ứ", "u")
            .Replace("ữ", "u")
            .Replace("ụ", "u")
            .Replace("ý", "y")
            .Replace("Ý", "Y")
            .Replace("ỳ", "y")
            .Replace("ÿ", "y")
            .Replace("ỹ", "y")
            .Replace("ỷ", "y")
            .Replace("ź", "z")
            .Replace("Ź", "Z")
            .Replace("ž", "z")
            .Replace("Ž", "Z")
            .Replace("ż", "z")
            .Replace("Ż", "Z")
            .Replace("þ", "th")
            .Replace("Þ", "Th")
            .Replace("ϊ", "i");

        return name;
    }
}