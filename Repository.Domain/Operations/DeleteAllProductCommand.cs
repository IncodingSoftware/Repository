namespace Repository.Domain
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class DeleteAllProductCommand : CommandBase
    {
        public override void Execute()
        {
            foreach (var product in Repository.Query<Product>())
                Repository.Delete(product);
        }
    }
}