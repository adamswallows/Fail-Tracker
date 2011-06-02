using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FailTracker.Core.Data;
using FailTracker.Core.Domain;
using FailTracker.Web.ActionResults;
using FailTracker.Web.Models.Account;

namespace FailTracker.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly IRepository<User> _repository;

		public AccountController(IRepository<User> repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public ActionResult LogOn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnForm form)
		{
			var user = (from u in _repository.Query()
			            where u.EmailAddress == form.EmailAddress
			            select u).SingleOrDefault();

			if (user == null || !user.IsThisTheUsersPassword(form.Password))
			{
				return View(new LogOnForm {EmailAddress = form.EmailAddress})
						.WithErrorMessage("Invalid username or password.");
			}

			FormsAuthentication.SetAuthCookie(form.EmailAddress, true);
			return Redirect("~/");
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return Redirect("~/");
		}
	}
}