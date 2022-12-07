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
	private readonly VenueConverter olympediaVenueConverter;
	private readonly EventConverter olympediaEventConverter;
	private readonly AthleteConverter olympediaAtheleteConverter;
	private readonly ParticipantConverter olympediaParticipantConverter;

	public ConverterManager(WorldCountryConverter worldCountryConverter,
		NOCConverter olympediaNOCConverter,
		GameConverter olympediaGameConverter,
		SportDisciplineConverter olympediaSportDisciplineConverter,
		VenueConverter olympediaVenueConverter,
		EventConverter olympediaEventConverter,
		AthleteConverter olympediaAtheleteConverter,
		ParticipantConverter olympediaParticipantConverter)
	{
		this.worldCountryConverter = worldCountryConverter;
		this.olympediaNOCConverter = olympediaNOCConverter;
		this.olympediaGameConverter = olympediaGameConverter;
		this.olympediaSportDisciplineConverter = olympediaSportDisciplineConverter;
		this.olympediaVenueConverter = olympediaVenueConverter;
		this.olympediaEventConverter = olympediaEventConverter;
		this.olympediaAtheleteConverter = olympediaAtheleteConverter;
		this.olympediaParticipantConverter = olympediaParticipantConverter;
	}

	public async Task RunWorldCountriesConverters()
	{
		await this.worldCountryConverter.ConvertAsync(ConverterConstants.WORLD_COUNTRIES_CONVERTER);
	}

	public async Task RunOlympediaConverters()
	{
		//await this.olympediaNOCConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_NOC_CONVERTER);
		//await this.olympediaGameConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_GAME_CONVERTER);
		//await this.olympediaSportDisciplineConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_SPORT_DISCIPLINE_CONVERTER);
		//await this.olympediaVenueConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_VENUE_CONVERTER);
		//await this.olympediaEventConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_RESULT_CONVERTER);
		//await this.olympediaAtheleteConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_ATHELETE_CONVERTER);
		await this.olympediaParticipantConverter.ConvertAsync(ConverterConstants.OLYMPEDIA_RESULT_CONVERTER);
	}
}