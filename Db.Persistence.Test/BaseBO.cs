using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Db.Persistence.Utils;
using Db.Persistence.DataAccess;

namespace Db.Persistence.Test
{
	public class BaseBO<T> where T : new()
	{

		#region     .....:::::     MÉTODOS     :::::.....

		public void Inserir(T obj, params String[] props)
		{
			new SqlBaseDAO().Inserir<T>(obj, props);
		}

		public void Atualizar(T obj, params String[] props)
		{
			new SqlBaseDAO().Atualizar<T>(obj, props);
		}

		public void Excluir(T obj, params String[] props)
		{
			new SqlBaseDAO().Excluir<T>(obj, props);
		}

		public List<T> Buscar(params String[] props)
		{
			SqlBaseDAO baseDao = new SqlBaseDAO();
			baseDao.AddProperties<T>(props);
			List<T> lstRetorno = baseDao.Buscar<T>(props);
			return lstRetorno;
		}

		public List<T> BuscarOrdenado(String[] orders, ETipoOrder tipo, params String[] props)
		{
			SqlBaseDAO baseDao = new SqlBaseDAO();
			baseDao.AddProperties<T>(props);
			baseDao.AddOrderBy<T>(tipo, orders);
			List<T> lstRetorno = baseDao.Buscar<T>(props);
			return lstRetorno;
		}

		public T BuscarPorId(T obj, params String[] props)
		{
			return new SqlBaseDAO().BuscarPorId<T>(obj, props);
		}

		public Paginacao<T> BuscarComPaginacao(Int32 indexPagina, Int32 totalRegistros, params String[] props)
		{
			Paginacao<T> paginacao = new SqlBaseDAO().BuscarComPaginacao<T>(indexPagina, totalRegistros, props);
			return paginacao;
		}

		public Paginacao<T> BuscarComPaginacao(String[] orders, ETipoOrder tipo, Int32 indexPagina, Int32 totalRegistros, params String[] props)
		{
			SqlBaseDAO baseDao = new SqlBaseDAO();
			baseDao.AddProperties<T>(props);
			baseDao.AddOrderBy<T>(tipo, orders);
			Paginacao<T> paginacao = baseDao.BuscarComPaginacao<T>(indexPagina, totalRegistros, props);
			return paginacao;
		}

		#endregion

	}
}
