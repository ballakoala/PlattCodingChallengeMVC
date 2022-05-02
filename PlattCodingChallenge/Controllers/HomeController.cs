using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlattCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PlattCodingChallenge.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetAllPlanets()
		{
			var model = new AllPlanetsViewModel();

			// TODO: Implement this controller action

			return View(model);
		}

		public ActionResult GetPlanetById(int planetid)
		{
			var model = new SinglePlanetViewModel();

			// TODO: Implement this controller action

			return View(model);
		}

		public ActionResult GetResidentsOfPlanet(string planetname)
		{
			var model = new PlanetResidentsViewModel();

			// TODO: Implement this controller action

			return View(model);
		}

		public ActionResult VehicleSummary()
		{
			SWAPIVehicleResults vehicleResults = GetSWAPIVehicles();

			var model = new VehicleSummaryViewModel();
			model.Details = vehicleResults.Vehicles.Where(v => v.Cost != "unknown")
				.GroupBy(v => v.ManufacturerName).Select(g =>
				{
					return new VehicleStatsViewModel()
					{
						ManufacturerName = g.Key,
						VehicleCount = g.Count(),
						AverageCost = g.Average(v => int.Parse(v.Cost))
					};
				})
				.OrderByDescending(m => m.VehicleCount)
				.ThenByDescending(m => m.AverageCost)
				.ToList();
			model.VehicleCount = model.Details.Sum(vStats => vStats.VehicleCount);
			model.ManufacturerCount = model.Details.Count();

			return View(model);
		}

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

		public class SWAPVehicle
        {
			[JsonProperty(PropertyName = "cost_in_credits")]
			public string Cost { get; set; }

			[JsonProperty(PropertyName = "manufacturer")]
			public string ManufacturerName { get; set; }
        }

		// https://stackoverflow.com/questions/9620278/how-do-i-make-calls-to-a-rest-api-using-c
		private SWAPIVehicleResults GetSWAPIVehicles(SWAPIVehicleResults previousVehicleResults = null)
		{
			HttpClient httpClient = new HttpClient();
			Uri baseUri = !string.IsNullOrWhiteSpace(previousVehicleResults?.NextAPIURI) 
				? new Uri(previousVehicleResults.NextAPIURI) 
				: new Uri("https://swapi.dev/api/vehicles/");
			httpClient.BaseAddress = baseUri;

			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpResponseMessage response = httpClient.GetAsync(baseUri).Result;
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

			httpClient.Dispose();

			return previousVehicleResults;
		}
    }
}
