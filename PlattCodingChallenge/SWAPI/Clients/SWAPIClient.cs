using Newtonsoft.Json;
using PlattCodingChallenge.SWAPI.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PlattCodingChallenge.SWAPI.Clients
{
	// maybe have a base SWAPI client, then have child clients that know their models/URIs/parameters instead of cluttering one class with all the things
    public class SWAPIClient : IDisposable
	{
		private HttpClient client;

		public SWAPIClient()
		{ 
			client = new HttpClient();
		}

        public void Dispose()
        {
			this.client.Dispose();
        }

		// stole knowledge from
		// https://stackoverflow.com/questions/9620278/how-do-i-make-calls-to-a-rest-api-using-c
		// try separating out the base api call/recursive next api call/deserialization into separate methods/classes
		// i do like how https://github.com/M-Yankov/SWapi-CSharp/blob/master/SWapi-CSharp/Repository.cs separated stuff into various dataservices/repositories
		// i think all the api endpoints have a 'count, previous, next, results' model we can leverage
		public SWAPIVehicleResults GetSWAPIVehicles(SWAPIVehicleResults previousVehicleResults = null)
		{
			Uri baseUri = !string.IsNullOrWhiteSpace(previousVehicleResults?.NextAPIURI)
				? new Uri(previousVehicleResults.NextAPIURI)
				: new Uri("https://swapi.dev/api/vehicles/");

			HttpResponseMessage response = this.client.GetAsync(baseUri).Result;
			if (response.IsSuccessStatusCode)
			{
				var dataObjects = response.Content.ReadAsStringAsync().Result;

				if (previousVehicleResults != null)
				{
					SWAPIVehicleResults vehicleSummaryViewModel = JsonConvert.DeserializeObject<SWAPIVehicleResults>(dataObjects);
					vehicleSummaryViewModel.Vehicles.AddRange(previousVehicleResults.Vehicles);
					previousVehicleResults = vehicleSummaryViewModel;
				}
				else
				{
					previousVehicleResults = JsonConvert.DeserializeObject<SWAPIVehicleResults>(dataObjects);
				}

				if (previousVehicleResults.NextAPIURI != null)
				{
					previousVehicleResults = GetSWAPIVehicles(previousVehicleResults);
				}
			}
			else
			{
				//failed request
			}

			return previousVehicleResults;
		}
	}
}
