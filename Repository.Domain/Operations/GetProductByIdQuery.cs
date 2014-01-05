namespace Repository.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetProductByIdQuery : QueryBase<GetProductByIdQuery.Response>
    {
        #region Properties

        public string Id { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Article { get; set; }

            public string Price { get; set; }

            #endregion
        }

        #endregion

        protected override Response ExecuteResult()
        {
            var product = Repository.GetById<Product>(Id);            
            return new Response
                       {
                               Article = "{0}-{1}".F(product.Name, product.Code),
                               Price = product.Price.ToString()
                       };
        }
    }
}