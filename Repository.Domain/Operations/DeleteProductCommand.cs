namespace Repository.Domain
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class DeleteProductCommand : CommandBase
    {
        #region Properties

        public string Id { get; set; }

        #endregion

        public override void Execute()
        {
            Repository.Delete<Product>(Id);
        }
    }
}