namespace CMS.Web.V1.Models.DataAcces
{
    public class DataAccess
    {
        private static DataAccessLayer dal;
        protected static DataAccessLayer DAL
        {
            get
            {
                if (dal == null)
                    dal = new DataAccessLayer();
                return dal;
            }
        }
    }
}