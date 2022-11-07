namespace SportStats.Common.Converters;

using SportStats.Common.Constants;
using SportStats.Common.Converters.WorldCountries;

public class ConverterManager
{
	private readonly WorldCountryConverter worldCountryConverter;

	public ConverterManager(WorldCountryConverter worldCountryConverter)
	{
		this.worldCountryConverter = worldCountryConverter;
	}

	public async Task RunWorldCountriesConverters()
	{
		await this.worldCountryConverter.ConvertAsync(ConverterConstants.WORLD_COUNTRIES_MAIN_URL);
	}
}