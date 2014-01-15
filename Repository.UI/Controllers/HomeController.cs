namespace Repository.UI.Controllers
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.MvcContrib;
    using Repository.Domain;

    #endregion

    public class HomeController : IncControllerBase
    {
        #region Api Methods

        public ActionResult Index()
        {
            if (!dispatcher.Query(new GetEntitiesQuery<Product>()).Any())
                Refresh();

            return View();
        }

        public ActionResult Refresh()
        {
            return TryPush(composite =>
                               {
                                   composite.Quote(new DeleteAllProductCommand());
                                   composite.Quote(new AddProductCommand { Code = "1", Name = "Audi", Price = 101 });
                                   composite.Quote(new AddProductCommand { Code = "2", Name = "BMW", Price = 202 });
                                   composite.Quote(new AddProductCommand { Code = "3", Name = "Porshe", Price = 303 });
                                   composite.Quote(new AddProductCommand { Code = "4", Name = "Infinity", Price = 404 });
                                   composite.Quote(new AddProductCommand { Code = "5", Name = "Acura", Price = 505 });
                                   composite.Quote(new AddProductCommand { Code = "6", Name = "Alpina", Price = 606 });
                                   composite.Quote(new AddProductCommand { Code = "7", Name = "Aston Martin", Price = 707 });
                                   composite.Quote(new AddProductCommand { Code = "8", Name = "Bentley", Price = 808 });
                               });
        }

        #endregion
    }
}