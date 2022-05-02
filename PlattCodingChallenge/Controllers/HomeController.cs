using Microsoft.AspNetCore.Mvc;
using PlattCodingChallenge.Models;

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
			// YEAHH BOI GONNA DO STUFF HERERERERE!!!

			return View(model);
		}
    }
}
