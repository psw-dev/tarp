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

        protected IDbConnection _connection;
        protected IDbTransaction _transaction;

        #endregion

        #region Protected properties

        protected string TableName { get; set; }
        protected string PrimaryKeyName { get; set; }

        #endregion

        #region Public properties

        public Entity Entity { get; set; }

        #endregion

        #region Protected Constructors

        protected Repository(IDbConnection connection)
        {
            _connection = connection;
        }

        #endregion

        #region Public Methods

        public virtual T Get(int id)
        {
            return _connection.Query<T>(string.Format("SELECT TOP 1 * FROM {0} WHERE [{2}] = '{1}'", TableName, id, PrimaryKeyName),
                                        transaction: _transaction).FirstOrDefault();
        }

        public virtual T Get(string id)
        {
            return _connection.Query<T>(string.Format("SELECT TOP 1 * FROM {0} WHERE [{2}] = '{1}'", TableName, id, PrimaryKeyName),
                                        transaction: _transaction).FirstOrDefault();
        }

        public virtual IEnumerable<T> Get()
        {
            return (IEnumerable<T>)_connection.Query<T>(string.Format("SELECT * FROM {0}", TableName),
                                                        transaction: _transaction);
        }


        public virtual int Add(T _entity)
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

            int result = _connection.ExecuteScalar<int>(query,
                                                        transaction: _transaction);
            return result;
        }
        public virtual void Delete(Entity _entity)
        {
            var Entity = _entity;
            var query = string.Format("DELETE {0} WHERE {2} = '{1}';", Entity.TableName, Entity.PrimaryKey, Entity.PrimaryKeyName);
            _connection.Execute(query,
                                transaction: _transaction);
        }

        public virtual void Delete(int id)
        {
            _connection.Query<T>(string.Format("DELETE FROM {0} WHERE {2} = '{1}'", TableName, id, PrimaryKeyName),
                                 transaction: _transaction).FirstOrDefault();
        }

        public virtual List<T> Where(object propertyValues)
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
            return _connection.Query<T>(string.Format(query, TableName, whereBulder),
                                        transaction: _transaction).ToList();
        }

        public virtual List<T> GetPage(int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(TableName) || pageSize < 1 || pageNumber < 1 || string.IsNullOrWhiteSpace(PrimaryKeyName))
                return new List<T>();

            var query = @"SELECT * FROM {2} ORDER BY {3} OFFSET(({1}-1)*{0}) ROWS FETCH NEXT {1} ROWS ONLY";
            query = string.Format(query, pageSize, pageNumber, TableName, PrimaryKeyName);

            return _connection.Query<T>(query,
                                        transaction: _transaction).ToList();
        }

        public virtual int Update(Entity entity)
        {

            StringBuilder values = new StringBuilder();
            Dictionary<string, object> columns = entity.GetColumns();

            foreach (KeyValuePair<string, object> item in columns)
            {
                if (entity.PrimaryKeyName == item.Key)
                    continue;

                values.Append(string.Format(item.Value == null ? "{0}={1}," : "{0}='{1}',", item.Key, item.Value == null ? "null" : item.Value.ToString()));
            }

            // Need to Remove last comma
            string colValues = values.ToString().TrimEnd(',');

            string query = string.Format("Update {0} Set {1} WHERE {2} = {3}", this.TableName, colValues, this.PrimaryKeyName, columns[this.PrimaryKeyName]); ;

            int numberOfRowsUpdated = _connection.ExecuteScalar<int>(query,
                                                                    null,
                                                                    transaction: _transaction);
            return numberOfRowsUpdated;
        }

        public virtual T Find(int id)
        {
            return _connection.Query<T>(
                string.Format("SELECT top 1 * FROM {0} where {1} = {2}", this.TableName, this.PrimaryKeyName, id),
                param: new { TableName = this.TableName, PrimaryKeyName = this.PrimaryKeyName, Id = id },
                transaction: _transaction
                ).FirstOrDefault();
        }

        public virtual void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public virtual int Count(string ColumnValue, string ColumnName)
        {
            string query = string.Format("SELECT count({0}) FROM {1} where {0} = '{2}'", ColumnName, this.TableName, ColumnValue);
            int numberOfRecords = _connection.ExecuteScalar<int>(query, transaction: _transaction);
            return numberOfRecords;
        }

        #endregion
    }
}