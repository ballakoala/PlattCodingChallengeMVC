using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlattCodingChallenge.SWAPI.Models
{
	// stole knowledge from
	// https://www.newtonsoft.com/json/help/html/SerializationAttributes.htm
	public class SWAPIVehicleResults
	{
		[JsonProperty(PropertyName = "count")]
		public int TotalVehicleCount { get; set; }

		[JsonProperty(PropertyName = "next")]
		public string NextAPIURI { get; set; }

		[JsonProperty(PropertyName = "previous")]
		public string PreviousAPIURI { get; set; }

		[JsonProperty(PropertyName = "results")]
		public List<SWAPVehicle> Vehicles { get; set; }
	}
}
