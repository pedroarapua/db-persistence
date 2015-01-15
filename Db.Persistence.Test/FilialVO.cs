using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Db.Persistence.Utils;
namespace Db.Persistence.Test
{
	[ClassAnnotation(TableName = "MAG_T_MM_FILIAL")]
	public class FilialVO
	{

		#region     .....:::::     ATRIBUTOS     :::::.....

		private Int32? _cod_filial;
		
		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		[FieldAnnotation( ColumName = "INTFILIAL", EnumName = "FilialCdCod_filial" , IsPrimaryKey = true )]
		public Int32? Cod_filial
		{
			get { return this._cod_filial; } 
			set { this._cod_filial = value; }
		}

		#endregion

		#region     .....:::::     CONSTRUTORES     :::::.....

		public FilialVO(){}

		//public FilialCdVO(DataRow row)
		//{
		//    this._cod_filial = Utils.Utils.GetValue<Int32?>(row, Atributos.FilialCdCod_filial.ToString());
		//    this._cod_deposito = Utils.Utils.GetValue<Int32?>(row, Atributos.FilialCdCod_deposito.ToString());
		//    this._nro_ordem = Utils.Utils.GetValue<Int16?>(row, Atributos.FilialCdNro_ordem.ToString());
		//    this._prz_entrega = Utils.Utils.GetValue<Int16?>(row, Atributos.FilialCdPrz_entrega.ToString());
		//    this._fl_logico = Utils.Utils.GetValue<String>(row, Atributos.FilialCdFl_logico.ToString());
		//    this._st_filial_cd = Utils.Utils.GetValue<Int16?>(row, Atributos.FilialCdSt_filial_cd.ToString());
		//}

		#endregion
	}
}
