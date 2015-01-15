using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Db.Persistence.Utils;
namespace Db.Persistence.Test
{
	[ClassAnnotation(TableName = "MAG_T_MM_CLIENTE")]
	public class ClienteVO
	{

		#region     .....:::::     ATRIBUTOS     :::::.....

		private Int64? _codcli;
		private Int64? _codfilcad;
		private Int64? _digcli;
		private DateTime? _dtcadast;
		private DateTime? _tsultalt;
		private String _natjur;
		private String _nomcli;
		private String _dtnasc;
		private String _nomemae;
		private String _nomepai;
		private Int64? _cgccpf;
		private String _numdoc;
		private String _emissor;
		private String _sexo;
		private Int64? _codcargo;
		private Int64? _codestcivil;
		private String _rescli;
		private String _ramatv;
		private String _tpinsc;
		private String _inscr;
		private Int64? _codport;
		private String _user_inclusao;
		private String _empresa;
		private Decimal? _vlrendbruto;
		private Decimal? _vloutrendas;
		private String _conjuge;
		private Decimal? _rendaconj;
		private String _respinterno;
		private String _codsitcredneg;
		private DateTime? _dtemissaorg;
		private Int64? _codnatprof;
		private String _profisconj;
		private Decimal? _vlfatano;
		private Int64? _qtfunc;
		private String _flpresenteado;
		private Int64? _cgccpfpres;
		private String _flnaocontactar;
		private Int64? _cpfresp;
		private DateTime? _dtatucad;
		private String _e_mailresp;
		private String _dddrefcoml1;
		private String _fonerefcoml1;
		private String _naturalidade;
		private String _ufnatural;
		private Int64? _codregiao;
		private Int64? _coduser;
		private String _flnac;
		private String _identproc;

		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		[FieldAnnotation(ColumName = "CODCLI", IsPrimaryKey = true, IsForeignKey = false)]
		public Int64? Codcli
		{
			get { return this._codcli; }
			set { this._codcli = value; }
		}

		[FieldAnnotation(ColumName = "CODFILCAD")]
		public Int64? Codfilcad
		{
			get { return this._codfilcad; }
			set { this._codfilcad = value; }
		}

		[FieldAnnotation(ColumName = "DIGCLI")]
		public Int64? Digcli
		{
			get { return this._digcli; }
			set { this._digcli = value; }
		}

		[FieldAnnotation(ColumName = "DTCADAST")]
		public DateTime? Dtcadast
		{
			get { return this._dtcadast; }
			set { this._dtcadast = value; }
		}

		[FieldAnnotation(ColumName = "TSULTALT")]
		public DateTime? Tsultalt
		{
			get { return this._tsultalt; }
			set { this._tsultalt = value; }
		}

		[FieldAnnotation(ColumName = "NATJUR")]
		public String Natjur
		{
			get { return this._natjur; }
			set { this._natjur = value; }
		}

		[FieldAnnotation(ColumName = "NOMCLI")]
		public String Nomcli
		{
			get { return this._nomcli; }
			set { this._nomcli = value; }
		}

		[FieldAnnotation(ColumName = "DTNASC")]
		public String Dtnasc
		{
			get { return this._dtnasc; }
			set { this._dtnasc = value; }
		}

		[FieldAnnotation(ColumName = "NOMEMAE")]
		public String Nomemae
		{
			get { return this._nomemae; }
			set { this._nomemae = value; }
		}

		[FieldAnnotation(ColumName = "NOMEPAI")]
		public String Nomepai
		{
			get { return this._nomepai; }
			set { this._nomepai = value; }
		}

		[FieldAnnotation(ColumName = "CGCCPF")]
		public Int64? Cgccpf
		{
			get { return this._cgccpf; }
			set { this._cgccpf = value; }
		}

		[FieldAnnotation(ColumName = "NUMDOC")]
		public String Numdoc
		{
			get { return this._numdoc; }
			set { this._numdoc = value; }
		}

		[FieldAnnotation(ColumName = "EMISSOR")]
		public String Emissor
		{
			get { return this._emissor; }
			set { this._emissor = value; }
		}

		[FieldAnnotation(ColumName = "SEXO")]
		public String Sexo
		{
			get { return this._sexo; }
			set { this._sexo = value; }
		}

		[FieldAnnotation(ColumName = "CODCARGO")]
		public Int64? Codcargo
		{
			get { return this._codcargo; }
			set { this._codcargo = value; }
		}

		[FieldAnnotation(ColumName = "CODESTCIVIL")]
		public Int64? Codestcivil
		{
			get { return this._codestcivil; }
			set { this._codestcivil = value; }
		}

		[FieldAnnotation(ColumName = "RESCLI")]
		public String Rescli
		{
			get { return this._rescli; }
			set { this._rescli = value; }
		}

		[FieldAnnotation(ColumName = "RAMATV")]
		public String Ramatv
		{
			get { return this._ramatv; }
			set { this._ramatv = value; }
		}

		[FieldAnnotation(ColumName = "TPINSC")]
		public String Tpinsc
		{
			get { return this._tpinsc; }
			set { this._tpinsc = value; }
		}

		[FieldAnnotation(ColumName = "INSCR")]
		public String Inscr
		{
			get { return this._inscr; }
			set { this._inscr = value; }
		}

		[FieldAnnotation(ColumName = "CODPORT")]
		public Int64? Codport
		{
			get { return this._codport; }
			set { this._codport = value; }
		}

		[FieldAnnotation(ColumName = "USER_INCLUSAO")]
		public String User_inclusao
		{
			get { return this._user_inclusao; }
			set { this._user_inclusao = value; }
		}

		[FieldAnnotation(ColumName = "EMPRESA")]
		public String Empresa
		{
			get { return this._empresa; }
			set { this._empresa = value; }
		}

		[FieldAnnotation(ColumName = "VLRENDBRUTO")]
		public Decimal? Vlrendbruto
		{
			get { return this._vlrendbruto; }
			set { this._vlrendbruto = value; }
		}

		[FieldAnnotation(ColumName = "VLOUTRENDAS")]
		public Decimal? Vloutrendas
		{
			get { return this._vloutrendas; }
			set { this._vloutrendas = value; }
		}

		[FieldAnnotation(ColumName = "CONJUGE")]
		public String Conjuge
		{
			get { return this._conjuge; }
			set { this._conjuge = value; }
		}

		[FieldAnnotation(ColumName = "RENDACONJ")]
		public Decimal? Rendaconj
		{
			get { return this._rendaconj; }
			set { this._rendaconj = value; }
		}

		[FieldAnnotation(ColumName = "RESPINTERNO")]
		public String Respinterno
		{
			get { return this._respinterno; }
			set { this._respinterno = value; }
		}

		[FieldAnnotation(ColumName = "CODSITCREDNEG")]
		public String Codsitcredneg
		{
			get { return this._codsitcredneg; }
			set { this._codsitcredneg = value; }
		}

		[FieldAnnotation(ColumName = "DTEMISSAORG")]
		public DateTime? Dtemissaorg
		{
			get { return this._dtemissaorg; }
			set { this._dtemissaorg = value; }
		}

		[FieldAnnotation(ColumName = "CODNATPROF")]
		public Int64? Codnatprof
		{
			get { return this._codnatprof; }
			set { this._codnatprof = value; }
		}

		[FieldAnnotation(ColumName = "PROFISCONJ")]
		public String Profisconj
		{
			get { return this._profisconj; }
			set { this._profisconj = value; }
		}

		[FieldAnnotation(ColumName = "VLFATANO")]
		public Decimal? Vlfatano
		{
			get { return this._vlfatano; }
			set { this._vlfatano = value; }
		}

		[FieldAnnotation(ColumName = "QTFUNC")]
		public Int64? Qtfunc
		{
			get { return this._qtfunc; }
			set { this._qtfunc = value; }
		}

		[FieldAnnotation(ColumName = "FLPRESENTEADO")]
		public String Flpresenteado
		{
			get { return this._flpresenteado; }
			set { this._flpresenteado = value; }
		}

		[FieldAnnotation(ColumName = "CGCCPFPRES")]
		public Int64? Cgccpfpres
		{
			get { return this._cgccpfpres; }
			set { this._cgccpfpres = value; }
		}

		[FieldAnnotation(ColumName = "FLNAOCONTACTAR")]
		public String Flnaocontactar
		{
			get { return this._flnaocontactar; }
			set { this._flnaocontactar = value; }
		}

		[FieldAnnotation(ColumName = "CPFRESP")]
		public Int64? Cpfresp
		{
			get { return this._cpfresp; }
			set { this._cpfresp = value; }
		}

		[FieldAnnotation(ColumName = "DTATUCAD")]
		public DateTime? Dtatucad
		{
			get { return this._dtatucad; }
			set { this._dtatucad = value; }
		}

		[FieldAnnotation(ColumName = "E_MAILRESP")]
		public String E_mailresp
		{
			get { return this._e_mailresp; }
			set { this._e_mailresp = value; }
		}

		[FieldAnnotation(ColumName = "dddrefcoml1")]
		public String Dddrefcoml1
		{
			get { return this._dddrefcoml1; }
			set { this._dddrefcoml1 = value; }
		}

		[FieldAnnotation(ColumName = "FONEREFCOML1")]
		public String Fonerefcoml1
		{
			get { return this._fonerefcoml1; }
			set { this._fonerefcoml1 = value; }
		}

		[FieldAnnotation(ColumName = "NATURALIDADE")]
		public String Naturalidade
		{
			get { return this._naturalidade; }
			set { this._naturalidade = value; }
		}

		[FieldAnnotation(ColumName = "UFNATURAL")]
		public String Ufnatural
		{
			get { return this._ufnatural; }
			set { this._ufnatural = value; }
		}

		[FieldAnnotation(ColumName = "CODREGIAO")]
		public Int64? Codregiao
		{
			get { return this._codregiao; }
			set { this._codregiao = value; }
		}

		[FieldAnnotation(ColumName = "CODUSER")]
		public Int64? Coduser
		{
			get { return this._coduser; }
			set { this._coduser = value; }
		}

		[FieldAnnotation(ColumName = "FLNAC")]
		public String Flnac
		{
			get { return this._flnac; }
			set { this._flnac = value; }
		}

		[FieldAnnotation(ColumName = "IDENTPROC")]
		public String Identproc
		{
			get { return this._identproc; }
			set { this._identproc = value; }
		}

		#endregion


		#region     .....:::::     CONSTRUTORES     :::::.....

		public ClienteVO() { }

		#endregion
	}
}
