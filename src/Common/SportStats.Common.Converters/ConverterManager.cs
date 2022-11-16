namespace SportStats.Common.Converters;

using SportStats.Common.Constants;
using SportStats.Common.Converters.Olympedia;
using SportStats.Common.Converters.WorldCountries;

public class ConverterManager
{
	private readonly WorldCountryConverter worldCountryConverter;
	private readonly NOCConverter olympediaNOCConverter;
	private readonly GameConverter olympediaGameConverter;
	private readonly SportDisciplineConverter olympediaSportDisciplineConverter;

	public ConverterManager(WorldCountryConverter worldCountryConverter,
		NOCConverter olympediaNOCConverter,
		GameConverter olympediaGameConverter,
		SportDisciplineConverter olympediaSportDisciplineConverter)
	{
		this.worldCountryConverter = worldCountryConverter;
		this.olympediaNOCConverter = olympediaNOCConverter;
		this.olympediaGameConverter = olympediaGameConverter;
		this.olympediaSportDisciplineConverter = olympediaSportDisciplineConverter;
	}

	public async Task RunWorldCountriesConverters()
	{
		await this.worldCountryConverter.ConvertAsync(ConverterConstants.WORLD_COUNTRIES_CONVERTER);
	}

	public async Task RunOlympediaConverters()
	{
		//await this.olympediaNOCConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_NOC_CONVERTER);
		//await this.olympediaGameConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_GAME_CONVERTER);
		await this.olympediaSportDisciplineConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_SPORT_DISCIPLINE_CONVERTER);
	}
}