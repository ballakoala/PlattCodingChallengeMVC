using Microsoft.AspNetCore.Mvc;
using PlattCodingChallenge.Models;
using PlattCodingChallenge.SWAPI.Clients;
using PlattCodingChallenge.SWAPI.Models;
using System.Linq;

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

			//get SWAPI vehicle data
			SWAPIClient client = new SWAPIClient();
			SWAPIVehicleResults vehicleResults = client.GetSWAPIVehicles();
			client.Dispose();

			// maybe there's a cleaner/other way to map this data from model to model
			// possibly push this elsewhere
			model.Details = vehicleResults.Vehicles.Where(v => v.Cost != "unknown")
				// had to relearn groupBy
				.GroupBy(v => v.ManufacturerName).Select(g =>
				{
					return new VehicleStatsViewModel()
					{
						ManufacturerName = g.Key,
						VehicleCount = g.Count(),
						AverageCost = g.Average(v => double.Parse(v.Cost))
					};
				})
				.OrderByDescending(m => m.VehicleCount)
				// stole knowledge from
				// https://dotnettutorials.net/lesson/linq-thenby-and-thenbydescending/
				.ThenByDescending(m => m.AverageCost)
				.ToList();
			model.VehicleCount = model.Details.Sum(vStats => vStats.VehicleCount);
			model.ManufacturerCount = model.Details.Count();

			return View(model);
		}
    }
}
