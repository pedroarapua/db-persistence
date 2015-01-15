using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
namespace Db.Persistence.Utils
{
	public class Condition
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		private PropertyInfo propriedade;
		private object valor;
		private ETipoWhere tipo;
		private ObjetoSelect objSelect;

		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		public PropertyInfo Propriedade
		{
			get { return this.propriedade; }
			set { this.propriedade = value; }
		}

		public object Valor
		{
			get { return this.valor; }
			set { this.valor = value; }
		}

		public ETipoWhere Tipo
		{
			get { return this.tipo; }
			set { this.tipo = value; }
		}

		public ObjetoSelect ObjSelect
		{
			get { return this.objSelect; }
			set { this.objSelect = value; }
		}

		#endregion

		#region     .....:::::     METODOS     :::::.....

		//public void Where(KeyValuePair<String, object> where, ETipoWhere tipo)
		//{
		//    if (condicoes.Count == 0 && tipo == ETipoWhere.Or)
		//    {
		//        throw (new Exception("Não foi adicionada nenhuma condição anteriormente para se fazer \"OR\"."));
		//    }

		//    KeyValuePair<String, KeyValuePair<object, ETipoWhere>> keyValue = new KeyValuePair<string,KeyValuePair<object,ETipoWhere>>(where.Key, new KeyValuePair<object,ETipoWhere>(where.Value, tipo));
		//    condicoes.Add(keyValue);
		//}

		//public void Where(String property, object value, ETipoWhere tipo)
		//{
		//    if (condicoes.Count == 0 && tipo == ETipoWhere.Or)
		//    {
		//        throw (new Exception("Não foi adicionada nenhuma condição anteriormente para se fazer \"OR\"."));
		//    }

		//    KeyValuePair<String, KeyValuePair<object, ETipoWhere>> keyValue = new KeyValuePair<string, KeyValuePair<object, ETipoWhere>>(property, new KeyValuePair<object, ETipoWhere>(value, tipo));
		//    condicoes.Add(keyValue);
		//}

		//public void WhereAnd(KeyValuePair<String, object> where)
		//{
		//    KeyValuePair<String, KeyValuePair<object, ETipoWhere>> keyValue = new KeyValuePair<string, KeyValuePair<object, ETipoWhere>>(where.Key, new KeyValuePair<object, ETipoWhere>(where.Value, ETipoWhere.And));
		//    condicoes.Add(keyValue);
		//}

		//public void WhereAnd(String property, object value)
		//{
		//    KeyValuePair<String, KeyValuePair<object, ETipoWhere>> keyValue = new KeyValuePair<string, KeyValuePair<object, ETipoWhere>>(property, new KeyValuePair<object, ETipoWhere>(value, ETipoWhere.And));
		//    condicoes.Add(keyValue);
		//}

		//public void WhereOr(KeyValuePair<String, object> where)
		//{
		//    if (condicoes.Count == 0)
		//    {
		//        throw (new Exception("Não foi adicionada nenhuma condição anteriormente para se fazer \"OR\"."));
		//    }

		//    KeyValuePair<String, KeyValuePair<object, ETipoWhere>> keyValue = new KeyValuePair<string, KeyValuePair<object, ETipoWhere>>(where.Key, new KeyValuePair<object, ETipoWhere>(where.Value, ETipoWhere.Or));
		//    condicoes.Add(keyValue);
		//}

		//public void WhereOr(String property, object value)
		//{
		//    if (condicoes.Count == 0)
		//    {
		//        throw (new Exception("Não foi adicionada nenhuma condição anteriormente para se fazer \"OR\"."));
		//    }

		//    KeyValuePair<String, KeyValuePair<object, ETipoWhere>> keyValue = new KeyValuePair<string, KeyValuePair<object, ETipoWhere>>(property, new KeyValuePair<object, ETipoWhere>(value, ETipoWhere.Or));
		//    condicoes.Add(keyValue);
		//}

		//public List<KeyValuePair<String, KeyValuePair<object, ETipoWhere>>> GetCondicoes()
		//{
		//    return condicoes;
		//}

		//public static String ToTextTipoWhere(ETipoWhere tipo)
		//{
		//    switch (tipo)
		//    {
		//        case ETipoWhere.And:
		//            return "and ";
		//            break;
		//        case ETipoWhere.Or:
		//            return "or ";
		//            break;
		//    }
		//    return String.Empty;
		//}

		#endregion
	}
}
