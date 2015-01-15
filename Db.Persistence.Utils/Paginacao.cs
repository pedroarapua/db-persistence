using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Db.Persistence.Utils
{
	public class Paginacao<T>
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		private Int32 _totalRegistros;
		private List<T> _lstObjetos;

		#endregion

		#region     .....:::::     CONSTRUTORES     :::::.....

		public Paginacao()
		{
			this._lstObjetos = new List<T>();
		}

		public Paginacao(Int32 totalRegistros, List<T> lstObjetos)
		{
			this._lstObjetos = lstObjetos;
			this._totalRegistros = totalRegistros;
		}

		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		public Int32 TotalRegistros 
		{
			get {
				return _totalRegistros;
			}
			set {
				_totalRegistros = value;
			}
		}

		public List<T> LstObjetos
		{
			get
			{
				return _lstObjetos;
			}
			set
			{
				_lstObjetos = value;
			}
		}

		#endregion


	}
}
