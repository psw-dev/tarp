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
using SqlKata;
using SqlKata.Compilers;

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
        protected SqlServerCompiler _sqlCompiler;

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
            _sqlCompiler = new SqlServerCompiler();
        }

        #endregion

        #region Public Methods

        public virtual T Get(int id)
        {
            // Using parametrized query for security
            return _connection.Query<T>(string.Format("SELECT TOP 1 * FROM {0} WHERE [{1}] = @_id", TableName, PrimaryKeyName),
                                            new { _id = id },
                                            transaction: _transaction).FirstOrDefault();
        }

        public virtual T Get(string id)
        {
            // Using parametrized query for security
            return _connection.QueryFirst<T>(string.Format("SELECT TOP 1 * FROM {0} WHERE [{1}] = @_id", TableName, PrimaryKeyName),
                                         new { _id = id },
                                         transaction: _transaction);
        }

        public IEnumerable<T> Get()
        {
            return (IEnumerable<T>)_connection.Query<T>(string.Format("SELECT * FROM {0}", TableName),
                                                        transaction: _transaction);
        }

        public int Add(T _entity)
        {
            var Entity = _entity;
            var query  = "INSERT INTO {0} ({1}) VALUES({2}); SELECT SCOPE_IDENTITY()";

            var cols   = new StringBuilder();
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

            var sqlQuery = "Declare @id table (ID Bigint); " + query + " SELECT ID FROM @id;";
            sqlQuery.Replace("VALUES", "OUTPUT inserted.ID Into @id VALUES");

            int result = _connection.ExecuteScalar<int>(sqlQuery,
                                                        transaction: _transaction);
            return result;
        }

        public void Delete(Entity _entity)
        {
            var Entity = _entity;

            var query = string.Format("DELETE {0} WHERE {2} = '{1}';", Entity.TableName, Entity.PrimaryKey, Entity.PrimaryKeyName);
            _connection.Execute(query,
                                transaction: _transaction);
        }

        public virtual void Delete(int id)
        {
            // Using parametrized query for security
            _connection.Query<T>(string.Format("DELETE FROM {0} WHERE {1} = @_id", TableName, PrimaryKeyName), new
            {
                _id = id
            },
            transaction: _transaction).FirstOrDefault();
        }

        public List<T> Where(object propertyValues)
        {
            var query = new Query().FromRaw(TableName);
            query = query.Where(propertyValues);

            var result = _sqlCompiler.Compile(query);
            var sql = result.Sql;
            var parameters = new DynamicParameters(result.NamedBindings);

            return _connection.Query<T>(sql, param: parameters, transaction: _transaction).ToList();
        }

        public List<T> GetPage(int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(TableName) || pageSize < 1 || pageNumber < 1 || string.IsNullOrWhiteSpace(PrimaryKeyName))
                return new List<T>();

            var query = @"SELECT * FROM {2} ORDER BY {3} OFFSET(({1}-1)*{0}) ROWS FETCH NEXT {1} ROWS ONLY";
            query = string.Format(query, pageSize, pageNumber, TableName, PrimaryKeyName);

            return _connection.Query<T>(query,
                                        transaction: _transaction).ToList();
        }

        public int Update(Entity entity)
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

        public T Find(int id)
        {
            return _connection.Query<T>(
                string.Format("SELECT top 1 * FROM {0} where {1} = {2}", this.TableName, this.PrimaryKeyName, id),
                param: new { TableName = this.TableName, PrimaryKeyName = this.PrimaryKeyName, Id = id },
                transaction: _transaction
                ).FirstOrDefault();
        }
        public T Find(string id)
        {
            return _connection.Query<T>(
                string.Format("SELECT top 1 * FROM {0} where {1} = {2}", this.TableName, this.PrimaryKeyName, id),
                param: new { TableName = this.TableName, PrimaryKeyName = this.PrimaryKeyName, Id = id },
                transaction: _transaction
                ).FirstOrDefault();
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public int Count(string ColumnValue, string ColumnName)
        {
            string query = string.Format("SELECT count({0}) FROM {1} where {0} = '{2}'", ColumnName, this.TableName, ColumnValue);
            int numberOfRecords = _connection.ExecuteScalar<int>(query, transaction: _transaction);
            return numberOfRecords;
        }

        #endregion
    }
}