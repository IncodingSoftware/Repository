namespace Repository.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;

    #endregion

    public class InitDataBaseSetUp : ISetUp
    {
        #region Fields

        readonly IManagerDataBase managerDataBase;

        #endregion

        #region Constructors

        public InitDataBaseSetUp(IManagerDataBase managerDataBase)
        {
            this.managerDataBase = managerDataBase;
        }

        #endregion

        #region ISetUp Members

        public int GetOrder()
        {
            return 0;
        }

        public void Execute()
        {
            if (!this.managerDataBase.IsExist())
                this.managerDataBase.Update();
        }

        #endregion

        #region Disposable

        public void Dispose() { }

        #endregion
    }
}