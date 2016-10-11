using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Library.Repositories.DAL
{
    public class Repository<TEntity> : IDisposable where TEntity : new()
    {
        protected readonly SqlConnection connection = DAL.Database.GetConnection();
        protected SqlDataReader reader;

        protected IEnumerable<TEntity> ToList(IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                List<TEntity> items = new List<TEntity>();
                while (reader.Read())
                {
                    var item = new TEntity();
                    Map(reader, item);
                    items.Add(item);
                }
                return items;
            }
        }

        protected virtual void Map(IDataRecord record, TEntity entity) { }

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Close();
            }

            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
