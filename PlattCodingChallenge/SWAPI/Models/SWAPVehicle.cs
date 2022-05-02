using Newtonsoft.Json;

namespace PlattCodingChallenge.SWAPI.Models
{
	// stole knowledge from
	// https://www.newtonsoft.com/json/help/html/SerializationAttributes.htm
	public class SWAPVehicle
	{
		[JsonProperty(PropertyName = "cost_in_credits")]
		public string Cost { get; set; }

		[JsonProperty(PropertyName = "manufacturer")]
		public string ManufacturerName { get; set; }
	}
}
