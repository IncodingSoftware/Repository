namespace Repository.UI.Controllers
{    
    using Incoding.MvcContrib.MVD;
    using Repository.Domain;

    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController()
                : base(typeof(Bootstrapper).Assembly) { }
    }
}