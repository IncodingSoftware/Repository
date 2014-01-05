namespace Repository.Domain
{
    using System;
    using System.Linq.Expressions;
    using Incoding;

    public class ProductInPriceWhere : Specification<Product>
    {
        #region Fields

        readonly long? to;

        readonly long? @from;

        #endregion

        #region Constructors

        public ProductInPriceWhere(long? @from, long? to)
        {
            this.to = to;
            this.@from = @from;
        }

        #endregion

        public override Expression<Func<Product, bool>> IsSatisfiedBy()
        {
            if (!this.@from.HasValue && !this.to.HasValue)
                return null;

            return product => (!this.@from.HasValue || product.Price >= this.@from) && (!this.to.HasValue || product.Price <= this.to);
        }
    }
}