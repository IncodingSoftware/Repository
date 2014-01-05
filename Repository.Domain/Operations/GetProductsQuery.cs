namespace Repository.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetProductsQuery : QueryBase<List<GetProductsQuery.Response>>
    {
        #region Properties

        public long? From { get; set; }

        public long? To { get; set; }

        public string Code { get; set; }

        public ProductOrder.OrderBy OrderBy { get; set; }

        public bool Desc { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Article { get; set; }

            public string Price { get; set; }

            public string CreateDt { get; set; }

            public string Id { get; set; }

            #endregion
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            return Repository.Query(whereSpecification: new ProductHavingCodeWhere(Code)
                                            .And(new ProductInPriceWhere(From, To)),
                                    orderSpecification: new ProductOrder(OrderBy, Desc))
                             .ToList()
                             .Select(r => new Response
                                              {
                                                      Id = r.Id.ToString(),
                                                      Article = "{0} - {1}".F(r.Name, r.Code),
                                                      Price = r.Price.ToString(),
                                                      CreateDt = r.CreateDt.ToShortDateString()
                                              })
                             .ToList();
        }
    }
}