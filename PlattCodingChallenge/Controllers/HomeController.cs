using Microsoft.AspNetCore.Mvc;
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
			var model = new VehicleSummaryViewModel();

			// TODO: Implement this controller action

			SWAPIVehicleResults vehicleResults = GetSWAPIVehicles();

			vehicleResults.results.Where(v => v.cost_in_credits != "unknown");

			model.VehicleCount = vehicleResults.count;

			return View(model);
		}

		public class SWAPIVehicleResults
        {
			public int count { get; set; }
            public string next { get; set; }
            public string previous { get; set; }
			public List<SWAPVehicle> results { get; set; }
		}

		public class SWAPVehicle
        {
            public string cargo_capacity { get; set; }
            public string consumables { get; set; }
            public string cost_in_credits { get; set; }
            public string created { get; set; }
            public string crew { get; set; }
            public string edited { get; set; }
            public string length { get; set; }
            public string manufacturer { get; set; }
            public string max_atmosphering_speed { get; set; }
            public string model { get; set; }
            public string name { get; set; }
            public string passengers { get; set; }
            //public string pilots { get; set; }
            public List<string> films { get; set; }
            public string url { get; set; }
            public string vehicle_class { get; set; }
        }

		// https://stackoverflow.com/questions/9620278/how-do-i-make-calls-to-a-rest-api-using-c
		private SWAPIVehicleResults GetSWAPIVehicles(SWAPIVehicleResults previousVehicleResults = null)
		{
			if (previousVehicleResults == null)
            {
				previousVehicleResults = new SWAPIVehicleResults();

			}
			HttpClient httpClient = new HttpClient();
			Uri baseUri = string.IsNullOrWhiteSpace(previousVehicleResults?.next) 
				? new Uri(previousVehicleResults.next) 
				: new Uri("https://swapi.dev/api/vehicles/");
			httpClient.BaseAddress = baseUri;

			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpResponseMessage response = httpClient.GetAsync(baseUri).Result;
			if (response.IsSuccessStatusCode)
			{
				var dataObjects = response.Content.ReadAsStringAsync().Result;

				if (previousVehicleResults != null)
				{
					SWAPIVehicleResults vehicleSummaryViewModel = JsonSerializer.Deserialize<SWAPIVehicleResults>(dataObjects);
					vehicleSummaryViewModel.results.AddRange(previousVehicleResults.results);
					previousVehicleResults = vehicleSummaryViewModel;
				}
				else
                {
					previousVehicleResults = JsonSerializer.Deserialize<SWAPIVehicleResults>(dataObjects);
				}

				if (previousVehicleResults.next != null)
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
