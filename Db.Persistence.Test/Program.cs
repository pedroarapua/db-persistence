using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Db.Persistence.DataAccess;
using Db.Persistence.Utils;

namespace Db.Persistence.Test
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//List<KeyValuePair<ETipoOrder, String[]>> lst1 = new List<KeyValuePair<ETipoOrder, String[]>>();
			//lst1.Add(new KeyValuePair<ETipoOrder, String[]>(ETipoOrder.Asc, new String[] { "Codcli", "Cgccpfpres" }));
			//lst1.Add(new KeyValuePair<ETipoOrder, String[]>(ETipoOrder.Desc, new String[] { "Cgccpf", "Codcargo" }));

			Utils.Utils.namespaceEntidades = "Db.Persistence.Test";
			Utils.Utils.entidadeAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			//List<ClienteVO> lst = new ClienteBO().BuscarOrdenado(lst1, "Codcli");

			//List<EnderecoVO> Enderecos = new EnderecoBO().BuscarTudo();
            List<EnderecoVO> lstWhere = new EnderecoBO().BuscarTudoPaginacaoOrdenadoWhere();
			Paginacao<ClienteVO> paginacao = new ClienteBO().BuscarComPaginacao(1, 10, new String[] { "Codcli", "Codfilcad", "Digcli", "Dtcadast", "Tsultalt", "Natjur", "Nomcli", "Dtnasc", "Nomemae", "Nomepai", "Cgccpf", "Numdoc", "Emissor", "Sexo", "Codcargo", "Codestcivil", "Rescli", "Ramatv", "Tpinsc", "Inscr", "Codport", "User_inclusao", "Empresa", "Vlrendbruto", "Vloutrendas", "Conjuge", "Rendaconj", "Respinterno", "Codsitcredneg", "Dtemissaorg", "Codnatprof", "Profisconj", "Vlfatano", "Qtfunc", "Flpresenteado", "Cgccpfpres", "Flnaocontactar", "Cpfresp", "Dtatucad", "E_mailresp", "Dddrefcoml1", "Fonerefcoml1", "Naturalidade", "Ufnatural", "Codregiao", "Coduser", "Flnac", "Identproc" });
			List<EnderecoVO> EnderecosA = new EnderecoBO().Buscar();
			List<EnderecoVO> EnderecosB = new EnderecoBO().Buscar("Cod_End");
			EnderecoVO endereco = new EnderecoVO();
			endereco.Cod_End = -2;
			EnderecoVO EnderecosC = new EnderecoBO().BuscarPorId(endereco, "Cod_End");
			List<EnderecoVO> EnderecosD = new EnderecoBO().BuscarOrdenado(new String[] { "Cod_End" }, ETipoOrder.Desc);
			//Paginacao<EnderecoVO> paginacao = new EnderecoBO().BuscarComPaginacao(1, 10);
			//List<EnderecoVO> EnderecosE = new EnderecoBO().BuscarTudoOrdenado();
			//Paginacao<EnderecoVO> EnderecosF = new EnderecoBO().BuscarTudoPaginacaoOrdenado();
			//Paginacao<EnderecoVO> EnderecosG = new EnderecoBO().BuscarTudoPaginacaoOrdenadoWhere();
			//Condition condicao = new Condition();
			//condicao.WhereAnd("Codcli", 1);
			//condicao.WhereAnd("Codfilcad", 1);

			//Paginacao<ClienteVO> paginacao = new ClienteBO().BuscarComPaginacao(condicao, new String[] { "Codcli", "Cgccpfpres" }, ETipoOrder.Asc, 1, 10);
			//Paginacao<FilialCdVO> paginacao = new BaseBO<FilialCdVO>().BuscarComPaginacao(1, 10);
		}
	}
}