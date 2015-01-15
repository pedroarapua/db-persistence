using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Db.Persistence.DataAccess;
using Db.Persistence.Utils;
namespace Db.Persistence.Test
{
	public class EnderecoBO : BaseBO<EnderecoVO>
	{

		#region     .....:::::     MÉTODOS     :::::.....

		//public List<EnderecoVO> Buscar()
		//{
		//    List<EnderecoVO> lst = new SqlBaseDAO().Buscar<EnderecoVO>();
		//    return lst;
		//}

		//public List<EnderecoVO> BuscarTudo()
		//{
		//    SqlBaseDAO baseDao = new SqlBaseDAO();
		//    baseDao.AddProperties<EnderecoVO>("Cod_End", "Cliente", "Filial");
		//    baseDao.AddProperties<EnderecoVO>(typeof(ClienteVO), "Codcli");
		//    baseDao.AddProperties<EnderecoVO>(typeof(FilialVO), "Cod_filial");
		//    baseDao.AddProperties<ClienteVO>(typeof(FilialVO), "Filial", new String[]{ "Cod_filial"});
		//    List<EnderecoVO> lst = baseDao.Buscar<EnderecoVO>();
		//    return lst;
		//}

		//public List<EnderecoVO> BuscarTudoOrdenado()
		//{
		//    SqlBaseDAO baseDao = new SqlBaseDAO();
		//    baseDao.AddProperties<EnderecoVO>("Cod_End", "Cliente", "Filial");
		//    baseDao.AddProperties<EnderecoVO>(typeof(ClienteVO), "Codcli");
		//    baseDao.AddProperties<EnderecoVO>(typeof(FilialVO), "Cod_filial");
		//    baseDao.AddProperties<ClienteVO>(typeof(FilialVO), "Filial", new String[] { "Cod_filial" });

		//    //baseDao.AddProperties<EnderecoVO>();
		//    baseDao.AddOrderBy<EnderecoVO>(ETipoOrder.Asc, new String[] { "Cod_End" });
		//    baseDao.AddOrderBy<ClienteVO>(ETipoOrder.Desc, new String[] { "Codcli" });
		//    baseDao.AddOrderBy<FilialVO>("Filial", ETipoOrder.Asc, new String[] { "Cod_filial" });
		//    List<EnderecoVO> lstRetorno = baseDao.Buscar<EnderecoVO>();
		//    return lstRetorno;
		//}

		//public Paginacao<EnderecoVO> BuscarTudoPaginacaoOrdenado()
		//{
		//    SqlBaseDAO baseDao = new SqlBaseDAO();
		//    baseDao.AddProperties<EnderecoVO>("Cod_End", "Cliente", "Filial");
		//    baseDao.AddProperties<EnderecoVO>(typeof(ClienteVO), "Codcli");
		//    baseDao.AddProperties<EnderecoVO>(typeof(FilialVO), "Cod_filial");
		//    baseDao.AddProperties<ClienteVO>(typeof(FilialVO), "Filial", new String[] { "Cod_filial" });

		//    baseDao.AddOrderBy<EnderecoVO>(ETipoOrder.Asc, new String[] { "Cod_End" });
		//    baseDao.AddOrderBy<ClienteVO>(ETipoOrder.Desc, new String[] { "Codcli" });
		//    baseDao.AddOrderBy<FilialVO>("Filial", ETipoOrder.Asc, new String[] { "Cod_filial" });
		//    Paginacao<EnderecoVO> paginacao = baseDao.BuscarComPaginacao<EnderecoVO>(1, 10);
		//    return paginacao;
		//}

        public List<EnderecoVO> BuscarTudoPaginacaoOrdenadoWhere()
        {
            SqlBaseDAO baseDao = new SqlBaseDAO();
            //baseDao.AddProperties<EnderecoVO>("Cod_End", "Cliente", "Filial");
            //baseDao.AddProperties<EnderecoVO>(typeof(ClienteVO), "Codcli");
            //baseDao.AddProperties<EnderecoVO>(typeof(FilialVO), "Cod_filial");
            //baseDao.AddProperties<ClienteVO>(typeof(FilialVO), "Filial", new String[] { "Cod_filial" });

            //baseDao.AddOrderBy<EnderecoVO>(ETipoOrder.Asc, new String[] { "Cod_End" });
            //baseDao.AddOrderBy<ClienteVO>(ETipoOrder.Desc, new String[] { "Codcli" });
            //baseDao.AddOrderBy<FilialVO>("Filial", ETipoOrder.Asc, new String[] { "Cod_filial" });

            baseDao.AddWhere<EnderecoVO>("Cod_End", -2);
            //baseDao.AddWhere<ClienteVO>("Codcli", -5);
            //baseDao.AddWhere<FilialVO>("Filial", "Cod_filial", 1);

            List<EnderecoVO> lst = baseDao.Buscar<EnderecoVO>("Cod_End");
            return lst;
        }

		//public void Inserir(EnderecoVO obj)
		//{
		//    try
		//    {
		//        new SqlBaseDAO().Inserir(obj);
		//    }
		//    catch(Exception ex)
		//    {
		//        throw ex;
		//    }
		//}

		//public void Atualizar(EnderecoVO obj)
		//{
		//    try
		//    {
		//        new SqlBaseDAO().Atualizar(obj);
		//    }
		//    catch(Exception ex)
		//    {
		//        throw ex;
		//    }
		//}

		//public void Excluir(Decimal? codcli)
		//{
		//    try
		//    {
		//        new SqlBaseDAO().Excluir(codcli);
		//    }
		//    catch(Exception ex)
		//    {
		//        throw ex;
		//    }
		//}

		#endregion
	}
}
