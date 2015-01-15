using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Db.Persistence.Utils
{
	public class ClassAnnotation : System.Attribute
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		private String _tableName;

		#endregion
		#region     .....:::::     PROPRIEDADES     :::::.....

		public String TableName
		{
			get { return this._tableName; }
			set { this._tableName = value; }
		}

		#endregion
	}
}
