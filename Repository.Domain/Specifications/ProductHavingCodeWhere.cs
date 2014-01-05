namespace Repository.Domain
{
    using System;
    using System.Linq.Expressions;
    using Incoding;

    public class ProductHavingCodeWhere : Specification<Product>
    {
        #region Fields

        readonly string code;

        #endregion

        #region Constructors

        public ProductHavingCodeWhere(string code)
        {
            this.code = code;
        }

        #endregion

        public override Expression<Func<Product, bool>> IsSatisfiedBy()
        {
            if (string.IsNullOrWhiteSpace(this.code))
                return null;

            return product => product.Code.Contains(this.code);
        }
    }
}