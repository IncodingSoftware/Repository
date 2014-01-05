namespace Repository.Domain
{
    #region << Using >>

    using System;
    using Incoding.CQRS;

    #endregion

    public class AddProductCommand : CommandBase
    {
        #region Properties

        public string Code { get; set; }

        public string Name { get; set; }

        public long Price { get; set; }

        #endregion

        public override void Execute()
        {
            Repository.Save(new Product
                                {
                                        Code = Code,
                                        Name = Name,
                                        Price = Price,
                                        CreateDt = DateTime.Now
                                });
        }
    }
}