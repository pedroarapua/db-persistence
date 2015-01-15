using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Db.Persistence.Utils
{
	public class FieldAnnotation : System.Attribute
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		private String _columName;
		private String _enumName;
		private Boolean _isPrimaryKey;
		private Boolean _isForeignKey;

		#endregion
		#region     .....:::::     PROPRIEDADES     :::::.....

		public String ColumName
		{
			get { return this._columName; }
			set { this._columName = value; }
		}

		public String EnumName
		{
			get { return this._enumName; }
			set { this._enumName = value; }
		}

		public Boolean IsPrimaryKey
		{
			get { return this._isPrimaryKey; }
			set { this._isPrimaryKey = value; }
		}
		
		public Boolean IsForeignKey
		{
			get { return this._isForeignKey; }
			set { this._isForeignKey = value; }
		}

		#endregion
	}
}
