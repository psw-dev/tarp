/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Data.Sql.Repositories
{
    /// <summary>
    /// the base class for all Repositories . this contains generic functionalities of all repositories
    /// like Get,Add etc..
    /// </summary>
    /// <typeparam name="T">the type of the Entity</typeparam>
	public abstract class Repository<T> : IRepository<T> where T : Entity
    {
		#region Protected Fields

        private IDbConnection connection;

		#endregion

		#region Protected properties

		protected string TableName {get;set;}
		protected string PrimaryKeyName {get;set;}

		#endregion

		#region Public properties

        public Entity Entity { get; set; }

		#endregion

        #region Protected Constructors

		 protected Repository(IDbConnection _connection)
        {
            connection = _connection;
        }

		#endregion

		#region Public Methods

        public virtual T Get(int id)
        {
            return connection.Query<T>(string.Format("SELECT TOP 1 * FROM {0} WHERE {2} = '{1}'", TableName, id, PrimaryKeyName)).FirstOrDefault();
        }

        public virtual T Get(string id)
        {
            return connection.Query<T>(string.Format("SELECT TOP 1 * FROM {0} WHERE {2} = '{1}'", TableName, id, PrimaryKeyName)).FirstOrDefault();
        }

        public Task<IEnumerable<T>> Get()
        {
            return (Task<IEnumerable<T>>)connection.Query<T>(string.Format("SELECT * FROM {0}", TableName));
        }
        

        public void Add(T _entity)
        {
            var Entity = _entity;
            var query = "INSERT INTO {0} ({1}) VALUES({2}); SELECT SCOPE_IDENTITY()";

            var cols = new StringBuilder();
            var values = new StringBuilder();


            var columns = Entity.GetColumns();
            foreach (var item in columns.Where(c => c.Key != Entity.PrimaryKeyName))
            {
                cols.Append("[" + item.Key + "],");
                if (item.Value == null)
                    values.Append("NULL,");
                else
                    values.AppendFormat("'{0}',", item.Value.ToString().Replace("'", "''"));
            }

            query = string.Format(query, Entity.TableName, cols.ToString().TrimEnd(','), values.ToString().TrimEnd(',')) + ";";

            connection.Query<T>(query);
        }

		public void Delete(Entity _entity)
        {
            var Entity = _entity;
            var query = string.Format("DELETE {0} WHERE {2} = '{1}';", Entity.TableName, Entity.PrimaryKey, Entity.PrimaryKeyName);
            connection.Execute(query);
        }

        public virtual void Delete(int id)
        {
            connection.Query<T>(string.Format("DELETE FROM {0} WHERE {2} = '{1}'", TableName, id, PrimaryKeyName)).FirstOrDefault();
        }

        public List<T> Where(object propertyValues)
        {
            const string query = "SELECT * FROM {0} WHERE {1}";

            var whereBulder = new StringBuilder();
            var objectType = propertyValues.GetType();
            var first = true;
            foreach (var property in objectType.GetProperties())
            {
                whereBulder.AppendFormat("{2} {0} = '{1}'", property.Name, property.GetValue(propertyValues).ToString().Replace("'", "''"), first ? "" : "AND");
                first = false;
            }
            return connection.Query<T>(string.Format(query, TableName, whereBulder)).ToList();
        }

        public List<T> GetPage(int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(TableName) || pageSize < 1 || pageNumber < 1 || string.IsNullOrWhiteSpace(PrimaryKeyName))
                return new List<T>();

            var query = @"SELECT * FROM {2} ORDER BY {3} OFFSET(({1}-1)*{0}) ROWS FETCH NEXT {1} ROWS ONLY";
            query = string.Format(query, pageSize, pageNumber, TableName, PrimaryKeyName);

            return connection.Query<T>(query).ToList();
        }

	public int Update(Entity entity)
        {
            
            StringBuilder values=new StringBuilder();
            Dictionary<string,object> columns = entity.GetColumns();
            foreach (KeyValuePair<string,object> item in columns)
            {
                //item.Key //column's name;
                //item.Value //column's value;
                values.Append(string.Format("{0}='{1}',",item.Key,item.Value.ToString()));
                //"Update Table Set ColumnName=Value Where Id=1";
            }

            return connection.Query<T>(
                "Update @TableName Set @Values WHERE @PrimaryKeyName = @Id",
                param: new { TableName=this.TableName,Values=values.ToString(),PrimaryKeyName=this.PrimaryKeyName, Id = columns[this.PrimaryKeyName] }).Count();

        }

		#endregion
    }
}
