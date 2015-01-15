using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using Db.Persistence.Utils;
using System.Data.SqlClient;
namespace Db.Persistence.DataAccess
{
	public class SqlBaseDAO : BaseDAO
	{

		#region     .....:::::     CONSTRUTORES     :::::.....

		public SqlBaseDAO() : base("", "@"){}

		#endregion

		#region     .....:::::     MÉTODOS     :::::.....

		public SqlConnection GetConnection()
		{
			SqlConnection connection = new SqlConnection(connectionString.Count > 0 ? connectionString[0] : String.Empty);
			return connection;
		}

		public SqlConnection GetConnection(Int32 index)
		{
			SqlConnection connection = new SqlConnection(connectionString.Count >= (index + 1) ? ConfigurationManager.ConnectionStrings[index].ConnectionString : String.Empty);
			return connection;
		}

		public void Atualizar<T>(T obj, params String[] props)
		{
			base.AddProperties<T>(props);
			List<PropertyInfo> lstChavePrimaria = base.GetChavePrimaria(obj.GetType());
			ClassAnnotation classAnnotation = base.GetClassAnnotation(obj.GetType());
			
			if (base.ObjetoSelect.Propriedades.Count > 0 && classAnnotation != null && lstChavePrimaria.Count > 0)
			{
				StringBuilder query = new StringBuilder();
				// Escreve TABELA
				query.Append(String.Format("UPDATE {0} SET ", classAnnotation.TableName));

				// Escreve UDPATE COLUNAS
				query.Append(base.ToTextAtualizaColunas(this.ObjetoSelect));
				
				// Escreve WHERE
				query.Append("WHERE ");
				query.Append(base.ToTextWhere(lstChavePrimaria, false));
				
				SqlConnection conexao = null;
				try
				{
					conexao = this.GetConnection();
					DbCommand command = conexao.CreateCommand();
					
					// Escreve PARÂMETROS
					this.AddParameters<T>(command, obj);

					// Escreve PARÂMETROS
					this.AddParameters<T>(command, lstChavePrimaria, obj);

					PrintLog(query, command.Parameters);
					command.CommandText = query.ToString();
					command.CommandType = CommandType.Text;
					conexao.Open();
					command.ExecuteNonQuery();
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (conexao != null)
					{
						conexao.Close();
					}
				}
			}
		}

		public void Inserir<T>(T obj, params String[] props)
		{
			base.AddProperties<T>(props);
			
			ClassAnnotation classAnnotation = base.GetClassAnnotation(obj.GetType());
			if (base.ObjetoSelect.Propriedades.Count > 0 && classAnnotation != null)
			{
				StringBuilder query = new StringBuilder();
				query.Append(String.Format("INSERT INTO {0} (", classAnnotation.TableName));

				// Escreve COLUNAS
				query.Append(base.ToTextColunas(base.ObjetoSelect));
				
				// Escreve PARAMETROS
				query.Append(") VALUES (");

				query.Append(base.ToTextParametrosColunas(base.ObjetoSelect));

				query.Append(")");

				SqlConnection conexao = null;
				try
				{
					conexao = this.GetConnection();
					DbCommand command = conexao.CreateCommand();

					// Escreve VALORES PARAMETROS
					this.AddParameters<T>(command, obj);
					
					PrintLog(query, command.Parameters);

					command.CommandText = query.ToString();
					command.CommandType = CommandType.Text;
					conexao.Open();
					command.ExecuteNonQuery();
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (conexao != null)
					{
						conexao.Close();
					}
				}
			}
		}

		public void Excluir<T>(T obj, params String[] props)
		{
			base.AddProperties<T>(props);
			
			if (base.ObjetoSelect.Propriedades.Count == 0)
			{
				throw new Exception("Informe pelo menos uma propriedade para poder excluir a informação");
			}

			ClassAnnotation classAnnotation = base.GetClassAnnotation(obj.GetType());
			if (classAnnotation != null)
			{
				StringBuilder query = new StringBuilder();
				query.Append(String.Format("DELETE FROM {0} {1} WHERE ", classAnnotation.TableName, this.ObjetoSelect.Alias));

				// Escreve WHERE
				query.Append(base.ToTextWhere(base.ObjetoSelect.Propriedades, false));

				SqlConnection conexao = null;
				try
				{
					conexao = this.GetConnection();
					DbCommand command = conexao.CreateCommand();

					// Adiciona PARAMETROS
					this.AddParameters<T>(command, obj);
					
					PrintLog(query, command.Parameters);

					command.CommandText = query.ToString();
					command.CommandType = CommandType.Text;
					conexao.Open();
					command.ExecuteNonQuery();
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (conexao != null)
					{
						conexao.Close();
					}
				}
			}
		}

		public List<T> Buscar<T>(params String[] props)
		{
			DataTable dtRetorno = new DataTable();
			Type type = typeof(T);
			List<T> lst = new List<T>();
			base.AddProperties<T>(props);

			if (base.ObjetoSelect.Propriedades.Count > 0 && base.ValidaClassAnnotation(type))
			{
				ClassAnnotation classAttribute = base.GetClassAnnotation(type);
				StringBuilder query = new StringBuilder();
				query.Append("SELECT ");

				// Escreve COLUNAS
				query.Append(base.ToTextColumnSelect(new StringBuilder(), base.ObjetoSelect));
				
				// Escreve TABELAS
				query.Append(String.Format(" FROM {0}", base.ToTextTableSelect(new StringBuilder(), base.ObjetoSelect)));

				// Escreve WHERE
				String where = base.ToTextWhere(new StringBuilder(), base.ObjetoSelect, true);
				if (!String.IsNullOrEmpty(where))
				{
				    query.Append(String.Format(" WHERE {0} ",where));	
				}

				// Escreve ORDERBY
				String orderBy = base.ToTextOrderBy();
				if(!String.IsNullOrEmpty(orderBy))
					query.Append(String.Format(" order by {0}", orderBy));

				SqlConnection conexao = null;
				try
				{
					conexao = this.GetConnection();
					DbCommand command = conexao.CreateCommand();

					this.AddParameters(command, this.ObjetoSelect);
					
					PrintLog(query, command.Parameters);

					command.CommandText = query.ToString();
					command.CommandType = CommandType.Text;
					conexao.Open();
					dtRetorno.Load(command.ExecuteReader());
					lst = base.ConvertDataTableToList<T>(dtRetorno, base.ObjetoSelect);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (conexao != null)
					{
						conexao.Close();
					}
				}
			}
			return lst;
		}
			
		public T BuscarPorId<T>(T obj, params String[] props)
		{
			List<PropertyInfo> lstPropetiesChave = base.GetChavePrimaria(obj.GetType());
			
			base.AddProperties<T>(props);
			
			if (lstPropetiesChave.Count == 0)
			{
				throw new Exception("Objeto não possui chave primária para a busca");
			}

			if (base.ObjetoSelect.Propriedades.Count > 0 && base.ValidaClassAnnotation(obj.GetType()))
			{
				ClassAnnotation classAttribute = base.GetClassAnnotation(obj.GetType());
				StringBuilder query = new StringBuilder();
				query.Append("SELECT ");

				// Escreve COLUNAS
				query.Append(base.ToTextColumnSelect(new StringBuilder(), base.ObjetoSelect));

				query.Append(String.Format(" FROM {0} WHERE ", base.ToTextTableSelect(new StringBuilder(), base.ObjetoSelect)));

				// Escreve WHERE
				query.Append(base.ToTextWhere(lstPropetiesChave, true));
				
				SqlConnection conexao = null;
				try
				{
					conexao = this.GetConnection();
					DbCommand command = conexao.CreateCommand();

					// Escreve PARAMETROS
					this.AddParameters<T>(command, lstPropetiesChave, obj);
					
					PrintLog(query, command.Parameters);

					command.CommandText = query.ToString();
					command.CommandType = CommandType.Text;
					conexao.Open();
					DataTable dtRetorno = new DataTable();
					dtRetorno.Load(command.ExecuteReader());
					if (dtRetorno.Rows.Count > 0)
					{
						T obj1 = base.ConvertDataRowToObject<T>(dtRetorno.Rows[0], base.ObjetoSelect);
						return obj1;
						
					}
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (conexao != null)
					{
						conexao.Close();
					}
				}
			}
			return default(T);
		}

		public Paginacao<T> BuscarComPaginacao<T>(Int32 indexPagina, Int32 totalRegistros, params String[] props)
		{
			base.AddProperties<T>(props);
			return this.BuscarComPaginacao<T>(indexPagina, totalRegistros);
		}

		public Paginacao<T> BuscarComPaginacao<T>(Int32 indexPagina, Int32 totalRegistros)
		{
			Paginacao<T> paginacao = new Paginacao<T>();
			Type type = typeof(T);
			
			SqlConnection conexao = null;
			try
			{
				ClassAnnotation classAnnotation = base.GetClassAnnotation(type);
				if (classAnnotation != null && base.ObjetoSelect.Propriedades.Count > 0)
				{
					StringBuilder query = new StringBuilder();
					#region     .....:::::     SELECT QUE RETORNA O CURSOR     :::::.....

					query.Append("SELECT PAGING.* FROM (SELECT ROW_NUMBER() ");

					// Escreve ORDERBY
					String orderBy = base.ToTextOrderBy();
					if (!String.IsNullOrEmpty(orderBy))
					{
						query.Append(String.Format("OVER (ORDER BY {0}) ", base.ToTextOrderBy()));
					}
					else
					{
						query.Append("OVER (ORDER BY (SELECT 1)) ");
					}

					query.Append("AS 'ROWNUMBER', ");

					// Escreve COLUNAS
					query.Append(base.ToTextColumnSelect(new StringBuilder(), base.ObjetoSelect));

					// Escreve TABELAS
					query.Append(String.Format(" FROM {0} ", base.ToTextTableSelect(new StringBuilder(), base.ObjetoSelect)));

					// Escreve WHERE
					String where = base.ToTextWhere(new StringBuilder(), this.ObjetoSelect, true);
					if (!String.IsNullOrEmpty(where))
					{
						query.Append(String.Format("WHERE {0} ",where));	
					}
					
					query.Append(") PAGING ");
					query.AppendLine(String.Format("WHERE PAGING.ROWNUMBER BETWEEN (({0} * {1}) - ({1} - 1)) and {0} * {1};", indexPagina, totalRegistros));
					
					#endregion

					#region     .....:::::     SELECT COUNT REGISTROS     :::::.....

					// Escreve TABELAS
					query.Append("SELECT COUNT(1) AS COUNT ");
					query.Append(String.Format(" FROM {0} ", base.ToTextTableSelect(new StringBuilder(), base.ObjetoSelect)));

					// Escreve WHERE
					if (!String.IsNullOrEmpty(where))
					{
						query.Append(String.Format("WHERE {0} ", where));
					}

					query.Append(";");
					
					#endregion

					conexao = this.GetConnection();
					DbCommand command = conexao.CreateCommand();

					this.AddParameters(command, this.ObjetoSelect);
					
					PrintLog(query, command.Parameters);

					command.CommandText = query.ToString();
					command.CommandType = CommandType.Text;
					conexao.Open();
					
					DataTable dtResultado = new DataTable();
					DbDataReader reader = command.ExecuteReader();
					dtResultado.Load(reader);

					Int32 qtdRegistros = 0;
					if (reader.Read())
					{
						qtdRegistros = Convert.ToInt32(reader["COUNT"]);
					}
					paginacao = new Paginacao<T>(qtdRegistros,base.ConvertDataTableToList<T>(dtResultado, base.ObjetoSelect));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (conexao != null)
				{
					conexao.Close();
				}
			}
			return paginacao;
		}

		private void AddParameters(DbCommand command, ObjetoSelect objSelect)
		{
			if (objSelect.Condicoes.Count > 0)
			{
				foreach (Condition condicao in objSelect.Condicoes)
				{
					DbParameter param = command.CreateParameter();
					param.ParameterName = String.Format("{0}{1}_{2}", BaseDAO.separador, objSelect.Alias, condicao.Propriedade.Name);
					param.Value = condicao.Valor;
                    if (param.Value == null)
                        param.Value = DBNull.Value;
                    param.DbType = Utils.Utils.ConvertDbType(condicao.Valor.GetType());
					command.Parameters.Add(param);
				}
			}

			foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
			{
				this.AddParameters(command, objSelectAux);
			}
		}

		private void AddParameters<T>(DbCommand command, List<PropertyInfo> lstProperties, T obj)
		{
			foreach (PropertyInfo prop in lstProperties)
			{
				DbParameter param = command.CreateParameter();
				param.ParameterName = String.Format("{0}{1}_{2}", BaseDAO.separador, "t1", prop.Name);
				param.Value = prop.GetValue(obj, null);
                if (param.Value == null)
                    param.Value = DBNull.Value;
                param.DbType = Utils.Utils.ConvertDbType(prop.PropertyType);
				command.Parameters.Add(param);
			}
		}
		
		private void AddParameters<T>(DbCommand command, T obj)
		{
			this.AddParameters<T>(command, this.ObjetoSelect.Propriedades, obj);
		}

		private void PrintLog(StringBuilder query, DbParameterCollection parameters)
		{
			if (showSql)
			{
				Debug.WriteLine(String.Format("comando: {0}", query.ToString()));
				List<String> logParam = new List<String>();
				foreach (DbParameter paramLog in parameters)
				{
					logParam.Add(String.Format("{0} = {1} ({2})", paramLog.ParameterName, paramLog.Value, paramLog.DbType.ToString()));
				}
				Debug.WriteLine(String.Format("parâmetros: {0}", String.Join(", ", logParam.ToArray())));
			}
		}

		#endregion
	}
}
