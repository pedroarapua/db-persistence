using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Db.Persistence.Utils;
namespace Db.Persistence.Test
{
	[ClassAnnotation(TableName = "MAG_T_MM_ENDCLI_OFF")]
	public class EnderecoVO
	{

		#region     .....:::::     ATRIBUTOS     :::::.....

		private Int32? _cod_end;
		private ClienteVO _cliente;
		private FilialVO _filial;
		
		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		[FieldAnnotation( ColumName = "CODEND", IsPrimaryKey = true )]
		public Int32? Cod_End
		{
			get { return this._cod_end; } 
			set { this._cod_end = value; }
		}

		[FieldAnnotation( ColumName = "CODCLI", IsForeignKey = true )]
		public ClienteVO Cliente
		{
			get { return this._cliente; } 
			set { this._cliente = value; }
		}

		[FieldAnnotation( ColumName = "CODFILCAD", IsForeignKey = true )]
		public FilialVO Filial
		{
			get { return this._filial; } 
			set { this._filial = value; }
		}

		#endregion

		#region     .....:::::     CONSTRUTORES     :::::.....

		public EnderecoVO(){}

		#endregion
	}
}
