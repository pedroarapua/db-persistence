using System;
using System.Collections.Generic;
using System.Text;
using Db.Persistence.Utils;
using System.Reflection;
using System.Configuration;
using System.Data;

namespace Db.Persistence.DataAccess
{
	public class BaseDAO
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		private ObjetoSelect objetoSelect = new ObjetoSelect();
		private int indexAlias = 1;
		protected static String separador;
		protected static String prefixo;
		public static List<String> connectionString = new List<String>();
		public static Boolean showSql = false;
		public static Int32 pageSize = 10;

		#endregion

		#region     .....:::::     CONSTRUTORES     :::::.....

		public BaseDAO(){
			for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
			{
                if (i != 0 && !String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[i].ConnectionString.Trim()))
					connectionString.Add(ConfigurationManager.ConnectionStrings[i].ConnectionString);
			}
			if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ShowSql"]))
			{
				showSql = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowSql"]);
			}
			if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PageSize"]))
			{
				pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
			}
		}

		public BaseDAO(String _separador, String _prefixo)
			: this()
		{
			separador = _separador;
			prefixo = _prefixo;
		}

		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		public ObjetoSelect ObjetoSelect
		{
			get { return this.objetoSelect; }
			set { this.objetoSelect = value; }
		}

		#endregion

		#region     .....:::::     METODOS     :::::.....

		#region     .....:::::     MÉTODOS PÚBLICOS     :::::.....

		protected String ToTextOrderBy()
		{
			StringBuilder order = new StringBuilder();
			List<OrderBy> orderByAsc = new List<OrderBy>();
			List<OrderBy> orderByDesc = new List<OrderBy>();
			SetOrderBy(orderByAsc, this.ObjetoSelect, ETipoOrder.Asc);
			SetOrderBy(orderByDesc, this.ObjetoSelect, ETipoOrder.Desc);
			int index = 1;
			
			foreach (OrderBy orderBy in orderByAsc)
			{
				FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(orderBy.Propriedade);
				String aux = orderByAsc[orderByAsc.Count - 1].Propriedade.Name == orderBy.Propriedade.Name && orderByAsc[orderByAsc.Count - 1].ObjSelect.Alias == orderBy.ObjSelect.Alias ? String.Concat(Utils.Utils.ToTextTipoOrder(orderBy.Tipo), orderByAsc.Count == index && orderByDesc.Count == 0 ? String.Empty : ", ") : ", ";

				order.Append(String.Format("{0}.{1}{2}", orderBy.ObjSelect.Alias, fieldAnnotation.ColumName, aux));
				index++;
			}

			index = 1;
			foreach (OrderBy orderBy in orderByDesc)
			{
				FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(orderBy.Propriedade);
				order.Append(String.Format("{0}.{1}{2}", orderBy.ObjSelect.Alias, fieldAnnotation.ColumName, orderByDesc[orderByDesc.Count - 1].Propriedade.Name == orderBy.Propriedade.Name ? String.Concat(Utils.Utils.ToTextTipoOrder(orderBy.Tipo), orderByDesc.Count == index ? String.Empty : ", ") : ", "));
				index++;
			}
			return order.ToString();
		}

		protected String ToTextColumnSelect(StringBuilder query, ObjetoSelect objSelect)
		{
			List<PropertyInfo> lstPropSel = objSelect.Propriedades.FindAll(delegate(PropertyInfo p) { return !(p.PropertyType.Namespace.StartsWith(Utils.Utils.namespaceEntidades)); });
			int index = 0;
			foreach (PropertyInfo prop in lstPropSel)
			{
				if (index == 0 && !String.IsNullOrEmpty(query.ToString()))
				{
					query.Append(", ");
				}

			    FieldAnnotation fieldAttribute = this.GetFieldAnnotation(prop);
				if(fieldAttribute != null)
					query.Append(String.Format("{0}.{1} AS {0}_{2}", objSelect.Alias, fieldAttribute.ColumName, prop.Name, prop.Name != lstPropSel[lstPropSel.Count - 1].Name ? ", " : " "));
			}

			foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
			{
				this.ToTextColumnSelect(query, objSelectAux);
			}

			return query.ToString();
		}

		protected String ToTextTableSelect(StringBuilder query, ObjetoSelect objSelect)
		{
			if (objSelect.Alias == "t1")
			{
				ClassAnnotation classAnnotation = this.GetClassAnnotation(objSelect.TipoReferencia);
				if(classAnnotation == null)
					throw new Exception(String.Format("Classe {0} não possui assinatura ClassAnnotation, que especifica o nome da tabela.", objSelect.TipoReferencia.Name));
				query.Append(String.Format("{0} {1} ", classAnnotation.TableName, objSelect.Alias));
			}
			foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
			{
				ClassAnnotation classAnnotation = this.GetClassAnnotation(objSelectAux.TipoReferencia);
				FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(objSelectAux.PropriedadeReferencia);
				if(fieldAnnotation == null)
				{
					throw new Exception(String.Format("Propriedade de referência {0} no objeto {1} não possui anotação FieldAnnotation.", objSelectAux.PropriedadeReferencia.Name, objSelectAux.ObjetoParent.Tipo.Name));
				}

				List<PropertyInfo> lstChavePrimaria = this.GetChavePrimaria(objSelectAux.TipoReferencia);
				if(lstChavePrimaria.Count == 0)
					throw new Exception(String.Format("Objeto {0} não possui chave primária.", objSelectAux.TipoReferencia.Name));

				FieldAnnotation fieldAnnotationChave = this.GetFieldAnnotation(lstChavePrimaria[0]);
				query.Append(String.Format("INNER JOIN {0} {1} ON ({2}.{3} = {1}.{4}) ", classAnnotation.TableName, objSelectAux.Alias, objSelectAux.ObjetoParent.Alias, fieldAnnotation.ColumName, fieldAnnotationChave.ColumName));

				if(objSelectAux.ObjetosSelect.Count > 0)
				{
					this.ToTextTableSelect(query, objSelectAux);
				}
			}

			return query.ToString();
		}

		protected String ToTextAtualizaColunas(ObjetoSelect objSelect)
		{
			return this.ToTextAtualizaColunasParametro(objSelect.Propriedades, objSelect.Alias);
		}

		protected String ToTextWhere(StringBuilder query, ObjetoSelect objSelect, Boolean escreveAlias)
		{
			if (objSelect.Condicoes.Count > 0)
			{
				foreach(Condition condicao in objSelect.Condicoes)
				{
					if(!String.IsNullOrEmpty(query.ToString()))
					{
						query.Append(String.Format(" {0} ", condicao.Tipo.ToString().ToLower()));
					}
					FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(condicao.Propriedade);
					query.Append(String.Format("{0}{1} = {2}{3}{4}_{5} ", escreveAlias ? String.Concat(objSelect.Alias, ".") : String.Empty, fieldAnnotation.ColumName, BaseDAO.separador, BaseDAO.prefixo, objSelect.Alias, condicao.Propriedade.Name));
				}
			}

			foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
			{
				this.ToTextWhere(query, objSelectAux, escreveAlias);
			}

			return query.ToString();
		}

		protected String ToTextAtualizaColunasParametro(List<PropertyInfo> lstProperties, String alias)
		{
			StringBuilder query = new StringBuilder();
			foreach (PropertyInfo prop in lstProperties)
			{
				if (!String.IsNullOrEmpty(query.ToString()))
				{
					query.Append(", ");
				}
				FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(prop);
				query.Append(String.Format("{0} = {1}{2}{3}_{4} ", fieldAnnotation.ColumName, BaseDAO.separador, BaseDAO.prefixo, alias, prop.Name));
			}
			return query.ToString();
		}

		protected String ToTextColunas(ObjetoSelect objSelect)
		{
			return this.ToTextColunas(objSelect.Propriedades);
		}

		protected String ToTextColunas(List<PropertyInfo> lstProperties)
		{
			StringBuilder query = new StringBuilder();
			foreach (PropertyInfo prop in lstProperties)
			{
				FieldAnnotation fieldAttribute = this.GetFieldAnnotation(prop);
				query.Append(String.Format("{0}{1}", fieldAttribute.ColumName, prop.Name != lstProperties[lstProperties.Count - 1].Name ? ", " : String.Empty));
			}
			return query.ToString();
		}

		protected String ToTextParametrosColunas(ObjetoSelect objSelect)
		{
			return this.ToTextParametrosColunas(objSelect.Propriedades);
		}

		protected String ToTextParametrosColunas(List<PropertyInfo> lstProperties)
		{
			StringBuilder query = new StringBuilder();
			foreach (PropertyInfo prop in lstProperties)
			{
				FieldAnnotation fieldAttribute = this.GetFieldAnnotation(prop);
				query.Append(String.Format("{0}{1}{2}_{3}{4}", BaseDAO.separador, BaseDAO.prefixo, "t1", prop.Name, prop.Name != lstProperties[lstProperties.Count - 1].Name ? ", " : ""));
			}
			return query.ToString();
		}

		protected String ToTextWhere(List<PropertyInfo> lstProperties, Boolean escreveAlias)
		{
			StringBuilder query = new StringBuilder();

			foreach (PropertyInfo prop in lstProperties)
			{
                if (!String.IsNullOrEmpty(query.ToString()))
                {
                    query.Append(String.Format(" {0} ", ETipoWhere.And.ToString().ToLower()));
                }

				FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(prop);
				query.Append(String.Format("{0}{1} = {2}{3}{4}_{5} ", escreveAlias ? "t1." : string.Empty, fieldAnnotation.ColumName, BaseDAO.separador, BaseDAO.prefixo, "t1", prop.Name));
			}
			return query.ToString();
		}

		protected List<PropertyInfo> GetChavePrimaria(Type type)
		{
			List<PropertyInfo> lstChavePrimaria = new List<PropertyInfo>();
			foreach (PropertyInfo prop in type.GetProperties())
			{
				FieldAnnotation fieldAnnotation = this.GetFieldAnnotation(prop);
				if (fieldAnnotation != null)
				{
					if (fieldAnnotation.IsPrimaryKey)
						lstChavePrimaria.Add(prop);
				}
			}
			return lstChavePrimaria;
		}

		public void AddOrderBy<T>(String propRef, String[] orders)
		{
			this.AddOrderBy<T>(propRef, ETipoOrder.Asc, orders);
		}

		public void AddOrderBy<T>(String[] orders)
		{
			this.AddOrderBy<T>(ETipoOrder.Asc, orders);
		}

		public void AddOrderBy<T>(String propRef, ETipoOrder tipo, String[] orders)
		{
			foreach (String p in orders)
			{
				PropertyInfo prop = GetPropriedade<T>(p);
				AddPropertyOrderBy<T>(tipo, prop, propRef, this.ObjetoSelect);
			}
		}

		public void AddOrderBy<T>(ETipoOrder tipo, String[] orders)
		{
			foreach (String p in orders)
			{
				PropertyInfo prop = GetPropriedade<T>(p);
				this.AddPropertyOrderBy<T>(tipo, prop, String.Empty, this.ObjetoSelect);
			}
		}

		public void AddProperties<T>(params String[] props)
		{
			if (this.ObjetoSelect.Propriedades.Count == 0)
			{
				Type type = typeof(T);
				List<PropertyInfo> lstProperties = new List<PropertyInfo>(type.GetProperties());
				this.ObjetoSelect.Tipo = type;
				this.ObjetoSelect.TipoReferencia = type;
				this.ObjetoSelect.Alias = String.Format("t{0}", indexAlias);
				indexAlias++;

				this.AddTodasPropriedades<T>(this.ObjetoSelect, props);
			}
		}

		public void AddProperties<T>(Type typeRef, params String[] props)
		{
			Type type = typeof(T);
			List<PropertyInfo> lstProperties = new List<PropertyInfo>(typeRef.GetProperties());
			ObjetoSelect obj = new ObjetoSelect();
			obj.Tipo = type;
			obj.TipoReferencia = typeRef;
			obj.Alias = String.Format("t{0}", indexAlias);
			indexAlias++;

			obj.PropriedadeReferencia = new List<PropertyInfo>(type.GetProperties()).Find(delegate(PropertyInfo prop) { return prop.PropertyType == typeRef; });
			if (obj.PropriedadeReferencia == null)
			{
				throw new Exception(String.Format("Tipo {0} não encontrado no objeto {1}.", typeRef.Name, type.Name));
			}

			if (!this.ValidaTipoObjeto(obj))
			{
				throw new Exception(String.Format("Tipo {0} não encontrado no objeto {1}", typeRef.Name, type.Name));
			}

			this.AddTodasPropriedades<T>(obj, props);

			Boolean retorno = this.AddChildObjetoSelect(obj, this.ObjetoSelect);
			if (!retorno)
			{
				throw new Exception(String.Format("Objeto {0} não encontrado na árvore do objeto {1}", obj.TipoReferencia.Name, obj.Tipo.Name));
			}
		}

		public void AddProperties<T>(Type typeRef, String propertyRef, String[] props)
		{
			Type type = typeof(T);
			List<PropertyInfo> lstProperties = new List<PropertyInfo>(typeRef.GetProperties());
			List<PropertyInfo> lstPropertiesAux = new List<PropertyInfo>(type.GetProperties());
			ObjetoSelect obj = new ObjetoSelect();
			obj.Tipo = type;
			obj.TipoReferencia = typeRef;
			obj.PropriedadeReferencia = Utils.Utils.GetPropriedadeObjeto(type, propertyRef);
			obj.Alias = String.Format("t{0}", indexAlias);
			indexAlias++;

			if (!this.ValidaTipoObjeto(obj))
			{
				throw new Exception(String.Format("Tipo {0} não encontrado no objeto {1}", typeRef.Name, type.Name));
			}
			if (obj.PropriedadeReferencia == null)
			{
				throw new Exception(String.Format("Propriedade de referência {0} não encontrada no objeto {1}", propertyRef, type.Name));
			}

			this.AddTodasPropriedades<T>(obj, props);

			Boolean retorno = this.AddChildObjetoSelect(obj, this.ObjetoSelect);
			if (!retorno)
			{
				throw new Exception(String.Format("Objeto {0} não encontrado na árvore do objeto {1}", obj.TipoReferencia.Name, obj.Tipo.Name));
			}
		}

		public void AddWhere<T>(String prop, object value)
		{
			this.AddWhere<T>(ETipoWhere.And, prop, value);
		}

		public void AddWhere<T>(ETipoWhere tipo, String prop, object value)
		{
			PropertyInfo property = this.GetPropriedade<T>(prop);
			this.AddPropertyWhere<T>(tipo, property, String.Empty, value, this.ObjetoSelect);
		}

		public void AddWhere<T>(Type typeRef, String prop, object value)
		{
			this.AddWhere<T>(typeRef, ETipoWhere.And, prop, value);
		}

		public void AddWhere<T>(Type typeRef, ETipoWhere tipo, String prop, object value)
		{
			PropertyInfo property = this.GetPropriedade<T>(prop);
			this.AddPropertyWhere<T>(tipo, property, String.Empty, value, this.ObjetoSelect);
		}

		public void AddWhere<T>(String propertyRef, String prop, object value)
		{
			this.AddWhere<T>(propertyRef, ETipoWhere.And, prop, value);
		}

		public void AddWhere<T>(String propertyRef, ETipoWhere tipo, String prop, object value)
		{
			PropertyInfo property = this.GetPropriedade<T>(prop);
			this.AddPropertyWhere<T>(tipo, property, propertyRef, value, this.ObjetoSelect);
		}

		public List<T> ConvertDataTableToList<T>(DataTable dt, String alias)
		{
			ObjetoSelect objSelect = new ObjetoSelect();
			objSelect.Alias = alias;
			objSelect.Tipo = typeof(T);
			objSelect.TipoReferencia = typeof(T);
			List<T> lstRetorno = this.ConvertDataTableToList<T>(dt, objSelect);
			return lstRetorno;
		}

		public List<T> ConvertDataTableToList<T>(DataTable dt, ObjetoSelect objSelect)
		{
			List<T> lstRetorno = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T obj = this.ConvertDataRowToObject<T>(row, objSelect);

				lstRetorno.Add(obj);
			}
			return lstRetorno;
		}

		public T ConvertDataRowToObject<T>(DataRow row, String alias)
		{
			ObjetoSelect objSelect = new ObjetoSelect();
			objSelect.Alias = alias;
			objSelect.Tipo = typeof(T);
			objSelect.TipoReferencia = typeof(T);
			T obj = this.ConvertDataRowToObject<T>(row, objSelect);

			return obj;
		}

		public T ConvertDataRowToObject<T>(DataRow row, ObjetoSelect objSelect)
		{
			T obj = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
			foreach (PropertyInfo prop in objSelect.Propriedades)
			{
				if (prop.PropertyType.Namespace.StartsWith(Utils.Utils.namespaceEntidades))
				{
					Object objAux = this.ConvertDataRowToObject(row, objSelect.ObjetosSelect, prop.PropertyType);
					if (objAux != null)
						prop.SetValue(obj, objAux, null);
				}
				else
				{
					foreach (DataColumn column in row.Table.Columns)
					{
						List<String> lstSplit = new List<String>(column.ColumnName.Split('_'));
						lstSplit.RemoveAt(0);
						if (!column.ColumnName.StartsWith(objSelect.Alias))
							continue;
						if (String.Join("_", lstSplit.ToArray()) == prop.Name)
						{
							object value = Utils.Utils.GetValue(row, String.Format("{0}_{1}", objSelect.Alias, prop.Name), prop.PropertyType);
							prop.SetValue(obj, value, null);
						}
					}
				}
			}

			return obj;
		}

		public Object ConvertDataRowToObject(DataRow row, List<ObjetoSelect> lstObjSelect, Type type)
		{
			Object obj = null;
			foreach (ObjetoSelect objSelect in lstObjSelect)
			{
				if (objSelect.TipoReferencia == type)
				{
					obj = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
					foreach (PropertyInfo prop in objSelect.Propriedades)
					{
						if (!prop.PropertyType.Namespace.StartsWith(Utils.Utils.namespaceEntidades))
						{
							foreach (DataColumn column in row.Table.Columns)
							{
								List<String> lstSplit = new List<String>(column.ColumnName.Split('_'));
								lstSplit.RemoveAt(0);
								if (!column.ColumnName.StartsWith(objSelect.Alias))
									continue;
								if (String.Join("_", lstSplit.ToArray()) == prop.Name)
								{
									object value = Utils.Utils.GetValue(row, String.Format("{0}_{1}", objSelect.Alias, prop.Name), prop.PropertyType);
									prop.SetValue(obj, value, null);
								}
							}
						}
						else
						{
							if (objSelect.ObjetosSelect.Count > 0)
							{
								Object objAux = this.ConvertDataRowToObject(row, objSelect.ObjetosSelect, prop.PropertyType);
								if (objAux != null)
									prop.SetValue(obj, objAux, null);
							}
						}
					}

					if (objSelect.ObjetosSelect.Count > 0)
					{
						ConvertDataRowToObject(row, obj, objSelect.ObjetosSelect, this.ObjetoSelect);
					}
				}
			}

			return obj;
		}

		public void ConvertDataRowToObject(DataRow row, Object obj, List<ObjetoSelect> lstObjSelect, ObjetoSelect objSelect)
		{
			foreach (ObjetoSelect objSelectArvore in objSelect.ObjetosSelect)
			{
				foreach (ObjetoSelect objSelectFilho in lstObjSelect)
				{
					if (objSelectArvore.TipoReferencia == objSelectFilho.Tipo)
					{
						Object objNovo = objSelectFilho.PropriedadeReferencia.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { });
						foreach (PropertyInfo prop in objSelectFilho.Propriedades)
						{
							if (!prop.PropertyType.Namespace.StartsWith(Utils.Utils.namespaceEntidades))
							{
								foreach (DataColumn column in row.Table.Columns)
								{
									List<String> lstSplit = new List<String>(column.ColumnName.Split('_'));
									lstSplit.RemoveAt(0);
									if (!column.ColumnName.StartsWith(objSelectFilho.Alias))
										continue;
									if (String.Join("_", lstSplit.ToArray()) == prop.Name)
									{
										object value = Utils.Utils.GetValue(row, String.Format("{0}_{1}", objSelectFilho.Alias, prop.Name), prop.PropertyType);
										prop.SetValue(objNovo, value, null);
									}
								}
								obj.GetType().GetProperty(objSelectFilho.PropriedadeReferencia.Name).SetValue(obj, objNovo, null);
							}
							this.ConvertDataRowToObject(row, objNovo, objSelectFilho.ObjetosSelect, objSelect);
						}
					}
					this.ConvertDataRowToObject(row, obj, objSelectFilho.ObjetosSelect, objSelect);
				}
			}
		}

		#endregion

		#region     .....:::::     MÉTODOS PRIVADOS     :::::.....

		private PropertyInfo GetPropriedade<T>(String prop)
		{
			Type type = typeof(T);
			PropertyInfo propriedade = Utils.Utils.GetPropriedadeObjeto(type, prop);
			if (propriedade == null)
				throw new Exception(String.Format("Propriedade {0} inexistente no objeto {1}.", prop, type.Name));
			return propriedade;
		}

		private Boolean AddChildObjetoSelect(ObjetoSelect objetoSelect, ObjetoSelect objetoSelectRaiz)
		{
			Boolean encontrou = false;

			if (objetoSelect.Tipo == objetoSelectRaiz.Tipo && objetoSelect.PropriedadeReferencia == null && this.ExistePropriedadeReferencia(objetoSelect, objetoSelectRaiz))
			{
				objetoSelect.ObjetoParent = objetoSelectRaiz;
				objetoSelectRaiz.ObjetosSelect.Add(objetoSelect);
				encontrou = true;
			}
			else if (objetoSelect.PropriedadeReferencia != null && objetoSelect.Tipo == objetoSelectRaiz.TipoReferencia && this.ExistePropriedadeReferencia(objetoSelect.PropriedadeReferencia.Name, objetoSelectRaiz))
			{
				objetoSelect.ObjetoParent = objetoSelectRaiz;
				objetoSelectRaiz.ObjetosSelect.Add(objetoSelect);
				encontrou = true;
			}
			else
			{
				foreach (ObjetoSelect obj in objetoSelectRaiz.ObjetosSelect)
				{
					encontrou = AddChildObjetoSelect(objetoSelect, obj);
					if (encontrou)
						break;
				}
			}

			return encontrou;
		}

		private void AddTodasPropriedades<T>(ObjetoSelect objSelect, params String[] props)
		{
			Type type = objSelect.TipoReferencia;
			if (props.Length > 0)
			{
				foreach (String str in props)
				{
					PropertyInfo prop = new List<PropertyInfo>(type.GetProperties()).Find(delegate(PropertyInfo prop1) { return prop1.Name == str; });
					if (prop != null)
					{
						objSelect.Propriedades.Add(prop);
					}
					else
					{
						throw new Exception(String.Format("Propriedade {0} inexistente no objeto {1}", str, type.Name));
					}
				}
			}
			else
			{
				objSelect.Propriedades = new List<PropertyInfo>(type.GetProperties()).FindAll(delegate(PropertyInfo p) { return !p.PropertyType.Namespace.StartsWith(Utils.Utils.namespaceEntidades); });
			}
		}

		private Boolean ValidaTipoObjeto(ObjetoSelect objectSelect)
		{
			Boolean retorno = new List<PropertyInfo>(objectSelect.Tipo.GetProperties()).Exists(delegate(PropertyInfo prop) { return prop.PropertyType == objectSelect.TipoReferencia; });
			return retorno;
		}

		private Boolean ValidaTipoObjeto(List<ObjetoSelect> objetosSelect)
		{
			Boolean retorno = false;
			foreach (ObjetoSelect objSelect in objetosSelect)
			{
				retorno = objSelect.Propriedades.Exists(delegate(PropertyInfo prop) { return prop.PropertyType == objSelect.Tipo; });
				if (retorno == true)
					return retorno;
				else
				{
					if (objSelect.ObjetosSelect.Count > 0)
						retorno = this.ValidaTipoObjeto(objSelect.ObjetosSelect);
				}
			}
			return retorno;
		}

		private Boolean ValidaObjetoReferencia(ObjetoSelect objectSelect)
		{
			Boolean retorno = objectSelect.Propriedades.Exists(delegate(PropertyInfo prop) { return prop.PropertyType == objectSelect.TipoReferencia; });
			if (retorno == true)
				return retorno;
			else
			{
				if (objectSelect.ObjetosSelect.Count > 0)
				{
					retorno = this.ValidaTipoObjeto(objectSelect.ObjetosSelect);
				}
			}
			return retorno;
		}

		private Boolean ExistePropriedadeReferencia(ObjetoSelect objetoSelect, ObjetoSelect objetoSelectRaiz)
		{
			Boolean retorno = false;
			foreach (PropertyInfo prop in objetoSelectRaiz.Propriedades)
			{
				if (prop.Name == objetoSelect.PropriedadeReferencia.Name)
				{
					retorno = true;
					break;
				}
			}
			return retorno;
		}

		private Boolean ExistePropriedadeReferencia(String propStr, ObjetoSelect objetoSelectRaiz)
		{
			Boolean retorno = false;
			foreach (PropertyInfo prop in objetoSelectRaiz.Tipo.GetProperties())
			{
				if (prop.Name == propStr)
				{
					retorno = true;
					break;
				}
			}
			return retorno;
		}

		private void SetOrderBy(List<OrderBy> lst, ObjetoSelect objSelect, ETipoOrder tipo)
		{
			lst.AddRange(objSelect.Ordenacoes.FindAll(delegate(OrderBy orderby) { return orderby.Tipo == tipo; }));
			foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
			{
				SetOrderBy(lst, objSelectAux, tipo);
			}
		}
		
		private Boolean AddPropertyOrderBy<T>(ETipoOrder tipo, PropertyInfo prop, String propRef, ObjetoSelect objSelect)
		{
			Type type = typeof(T);
			Boolean encontrou = false;
			if (String.IsNullOrEmpty(propRef))
			{
				if (objSelect.TipoReferencia == type)
				{
					OrderBy orderby = new OrderBy();
					orderby.Tipo = tipo;
					orderby.Propriedade = prop;
					orderby.ObjSelect = objSelect;
					objSelect.Ordenacoes.Add(orderby);
					encontrou = true;
				}

				if (!encontrou)
				{
					foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
					{
						this.AddPropertyOrderBy<T>(tipo, prop, propRef, objSelectAux);
					}
				}
			}
			else
			{
				if (objSelect.PropriedadeReferencia != null && objSelect.PropriedadeReferencia.Name == propRef && type == objSelect.TipoReferencia)
				{
					OrderBy orderby = new OrderBy();
					orderby.Tipo = tipo;
					orderby.Propriedade = prop;
					orderby.ObjSelect = objSelect;
					objSelect.Ordenacoes.Add(orderby);
					encontrou = true;
				}

				if (!encontrou)
				{
					foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
					{
						encontrou = this.AddPropertyOrderBy<T>(tipo, prop, propRef, objSelectAux);
						if (encontrou)
							break;
					}
				}
			}

			return encontrou;
		}

		private Boolean AddPropertyWhere<T>(ETipoWhere tipo, PropertyInfo prop, String propRef, object value, ObjetoSelect objSelect)
		{
			Type type = typeof(T);
			Boolean encontrou = false;
			if (String.IsNullOrEmpty(propRef))
			{
				if (objSelect.TipoReferencia == type)
				{
					Condition condicao = new Condition();
					condicao.Propriedade = prop;
					condicao.Valor = value;
					condicao.Tipo = tipo;
					condicao.ObjSelect = objSelect;
					objSelect.Condicoes.Add(condicao);
					encontrou = true;
				}

				if (!encontrou)
				{
					foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
					{
						this.AddPropertyWhere<T>(tipo, prop, propRef, value, objSelectAux);
					}
				}
			}
			else
			{
				if (objSelect.PropriedadeReferencia != null && objSelect.PropriedadeReferencia.Name == propRef && type == objSelect.TipoReferencia)
				{
					Condition condicao = new Condition();
					condicao.Propriedade = prop;
					condicao.Valor = value;
					condicao.Tipo = tipo;
					condicao.ObjSelect = objSelect;
					objSelect.Condicoes.Add(condicao);
					encontrou = true;
				}

				if (!encontrou)
				{
					foreach (ObjetoSelect objSelectAux in objSelect.ObjetosSelect)
					{
						encontrou = this.AddPropertyWhere<T>(tipo, prop, propRef, value, objSelectAux);
						if (encontrou)
							break;
					}
				}
			}

			return encontrou;
		}

		#endregion

		#region     .....:::::     MÉTODOS PROTEGIDOS     :::::.....

		protected Boolean ValidaClassAnnotation(Type type)
		{
			ClassAnnotation[] lstClassAttribute = (ClassAnnotation[])type.GetCustomAttributes(typeof(ClassAnnotation), true);
			return lstClassAttribute.Length > 0;
		}

		protected Boolean ValidaFieldAnnotation(PropertyInfo prop)
		{
			FieldAnnotation[] fieldAttribute = (FieldAnnotation[])prop.GetCustomAttributes(typeof(FieldAnnotation), true);
			return fieldAttribute.Length > 0;
		}

		protected ClassAnnotation GetClassAnnotation(Type type)
		{
			ClassAnnotation classAnnotation = null;
			if (this.ValidaClassAnnotation(type))
			{
				ClassAnnotation[] lstClassAttribute = (ClassAnnotation[])type.GetCustomAttributes(typeof(ClassAnnotation), true);
				classAnnotation = lstClassAttribute[0];
			}
			return classAnnotation;
		}

		protected FieldAnnotation GetFieldAnnotation(PropertyInfo prop)
		{
			FieldAnnotation fieldAnnotation = null;
			if (this.ValidaFieldAnnotation(prop))
			{
				FieldAnnotation[] lstFieldAttribute = (FieldAnnotation[])prop.GetCustomAttributes(typeof(FieldAnnotation), true);
				fieldAnnotation = lstFieldAttribute[0];
			}
			return fieldAnnotation;
		}

		#endregion

		#endregion

	}
}
