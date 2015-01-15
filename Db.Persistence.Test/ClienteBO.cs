using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Db.Persistence.DataAccess;
namespace Db.Persistence.Test
{
	public class ClienteBO:BaseBO<ClienteVO>
	{

		#region     .....:::::     MÉTODOS     :::::.....

		public List<ClienteVO> Buscar()
		{
			List<ClienteVO> lst = new SqlBaseDAO().Buscar<ClienteVO>();
			return lst;
		}

		public ClienteVO BuscarPorId(ClienteVO cliente)
		{
			SqlBaseDAO baseDao = new SqlBaseDAO();
			ClienteVO obj = baseDao.BuscarPorId<ClienteVO>(cliente);
			return obj;
		}

		public void Inserir(ClienteVO obj)
		{
			try
			{
				new SqlBaseDAO().Inserir(obj);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void Atualizar(ClienteVO obj)
		{
			try
			{
				new SqlBaseDAO().Atualizar(obj);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void Excluir(Decimal? codcli)
		{
			try
			{
				new SqlBaseDAO().Excluir(codcli);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		#endregion
	}
}
