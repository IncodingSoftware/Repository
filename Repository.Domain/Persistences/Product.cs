namespace Repository.Domain
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class Product : IncEntityBase
    {
        #region Properties

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual DateTime  CreateDt { get; set; }

        public virtual long Price { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<Product>
        {
            ////ncrunch: no coverage start

            #region Constructors

            protected Map()
            {
                Table("Product");
                IdGenerateByGuid(r => r.Id);
                Map(r => r.Name);
                Map(r => r.Code);
                Map(r => r.Price);
                Map(r => r.CreateDt);
            }

            #endregion

            ////ncrunch: no coverage end        
        }

        #endregion
    }
}