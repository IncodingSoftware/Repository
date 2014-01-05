namespace Repository.Domain
{
    #region << Using >>

    using System;
    using Incoding.Data;

    #endregion

    public class ProductOrder : OrderSpecification<Product>
    {
        #region Fields

        readonly OrderBy orderBy;

        readonly OrderType desc;

        #endregion

        #region Constructors

        public ProductOrder(OrderBy orderBy, bool desc)
        {
            this.orderBy = orderBy;
            this.desc = desc ? OrderType.Descending : OrderType.Ascending;
        }

        #endregion

        #region Enums

        public enum OrderBy
        {
            Article,

            Price,

            CreateDt
        }

        #endregion

        public override Action<AdHocOrderSpecification<Product>> SortedBy()
        {
            switch (this.orderBy)
            {
                case OrderBy.Article:
                    return specification => specification.Order(r => r.Name, this.desc).Order(r => r.Code, this.desc);
                case OrderBy.Price:
                    return specification => specification.Order(r => r.Price, this.desc);
                case OrderBy.CreateDt:
                    return specification => specification.Order(r => r.CreateDt, this.desc);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}