/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.ComponentModel;
using System.Collections.Generic;

namespace PSW.ITMS.Data.Entities
{
    public abstract class Entity
    {
		#region Protected Properties

        public string TableName { get; set; }

        #endregion

        #region public constructor

        #endregion

        #region public Properties

        public object PrimaryKey { get; set; }
        
		public string PrimaryKeyName { get; set; }

		#endregion

        #region PropertyChange

        #endregion

		#region public Methods

		public virtual Dictionary<string, object> GetColumns()
        {
            return null;
        }

		#endregion
    }
}