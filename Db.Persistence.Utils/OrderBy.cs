using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
namespace Db.Persistence.Utils
{
	public class OrderBy
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		private ETipoOrder tipo;
		private PropertyInfo propriedade;
		private ObjetoSelect objSelect;
		
		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		public ETipoOrder Tipo 
		{
			get { return tipo; }
			set { tipo = value; }
		}

		public PropertyInfo Propriedade
		{
			get { return propriedade; }
			set { propriedade = value; }
		}

		public ObjetoSelect ObjSelect
		{
			get { return objSelect; }
			set { objSelect = value; }
		}

		#endregion

		#region     .....:::::     CONSTRUTORES     :::::.....

		public OrderBy() { }

		public OrderBy(ETipoOrder tipo, PropertyInfo propriedade, ObjetoSelect objSelect)
		{
			this.tipo = tipo;
			this.propriedade = propriedade;
			this.objSelect = objSelect;
		}

		public OrderBy(PropertyInfo propriedade, ObjetoSelect objSelect)
		{
			this.tipo = ETipoOrder.Asc;
			this.propriedade = propriedade;
			this.objSelect = objSelect;
		}

		#endregion

		#region     .....:::::     METODOS     :::::.....

		#endregion
	}
}
