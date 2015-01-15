using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
namespace Db.Persistence.Utils
{
	public static class Utils
	{
		#region     .....:::::     ATRIBUTOS     :::::.....

		public static String namespaceEntidades = "Db.Persistence.Test";
		public static Assembly entidadeAssembly;

		#endregion

		#region     .....:::::     MÉTODOS     :::::.....

		public static object GetValue(DataRow row, string columnName, Type propType)
		{
			object result = null;
			object value = null;
			try
			{
				if (row.Table.Columns.Contains(columnName))
					value = row[columnName];
			}
			catch (IndexOutOfRangeException ex)
			{
				value = null;
			}
			catch (ArgumentException ex)
			{
				value = null;
			}
			if (value == DBNull.Value || value == null)
			{
				if(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
					result = null;
				else if(value != DBNull.Value)
					result = Convert.ChangeType(value, propType);
                else if(propType.IsValueType)
                    result = Activator.CreateInstance(propType);
			}
			else
			{
				result = Utils.Cast(value, propType);
			}

			return result;
		}

		public static T GetValue<T>(DataRow row, string columnName)
		{
			object result = null;
			object value = null;
			try
			{
				if(row.Table.Columns.Contains(columnName))
					value = row[columnName];
			}
			catch (IndexOutOfRangeException ex)
			{
				value = null;
			}
			catch (ArgumentException ex)
			{
				value = null;
			}
			if (value == DBNull.Value || value == null)
			{
				result = default(T);
			}
			else
			{
				result = Utils.Cast<T>(value);
			}

			return (T)result;
		}


		public static ObjetoSelect GetObjetoSelect(ObjetoSelect objSelect, PropertyInfo propObj, Object obj)
		{
			ObjetoSelect objSelectAux = null;
			foreach (ObjetoSelect objSelect1 in objSelect.ObjetosSelect)
			{
				if (objSelect1.Tipo == obj.GetType() && objSelect1.TipoReferencia == propObj.PropertyType && objSelect1.PropriedadeReferencia.Name == propObj.Name)
				{
					objSelectAux = objSelect1;
					break;
				}
				else
				{
					foreach (ObjetoSelect objSelect2 in objSelect1.ObjetosSelect)
					{
						objSelectAux = GetObjetoSelect(objSelect2, propObj, obj);
					}
				}
			}
			return objSelectAux;
		}
			

		public static PropertyInfo GetPropriedadeObjeto(Type type, String prop)
		{
			PropertyInfo retorno = new List<PropertyInfo>(type.GetProperties()).Find(delegate(PropertyInfo propAux) { return propAux.Name == prop; });
			return retorno;
		}

		public static String ToTextTipoOrder(ETipoOrder tipo)
		{
			switch (tipo)
			{
				case ETipoOrder.Asc:
					return " asc";
					break;
				case ETipoOrder.Desc:
					return " desc";
					break;
			}
			return String.Empty;
		}

		private static object Cast<T>(object value)
		{
			object retorno = value;
			Type type = value.GetType();
			Type typeCast = typeof(T);
			if (type != typeCast)
			{
				if (typeCast == typeof(Int16) || typeCast == typeof(Int16?))
				{
					retorno = Convert.ToInt16(value);
				}
				else if (typeCast == typeof(Int32) || typeCast == typeof(Int32?))
				{
					retorno = Convert.ToInt32(value);
				}
				else if (typeCast == typeof(Int64) || typeCast == typeof(Int64?))
				{
					retorno = Convert.ToInt64(value);
				}
				else if (typeCast == typeof(Decimal) || typeCast == typeof(Decimal?))
				{
					retorno = Convert.ToDecimal(value);
				}
				else if (typeCast == typeof(String))
				{
					retorno = Convert.ToString(value);
				}
				else if (typeCast == typeof(DateTime) || typeCast == typeof(DateTime?))
				{
					retorno = Convert.ToDateTime(value);
				}
				else if (typeCast == typeof(Boolean) || typeCast == typeof(Boolean?))
				{
					retorno = Convert.ToBoolean(value);
				}
				else if (typeCast == typeof(Single) || typeCast == typeof(Single?))
				{
					retorno = Convert.ToSingle(value);
				}
				else if (typeCast == typeof(DateTimeOffset) || typeCast == typeof(DateTimeOffset?))
				{
					retorno = Convert.ToDateTime(value);
				}
				else if (typeCast == typeof(byte[]))
				{
					retorno = Convert.FromBase64CharArray((char[])value, 0, Convert.ToString(value).Length);
				}
			}
			else
			{
				if (type == typeof(Double))
				{
					retorno = Convert.ToDecimal(value);
				}
				else if (type == typeof(Single))
				{
					retorno = Convert.ToDecimal(value);
				}
				else if (type == typeof(TimeSpan))
				{
					retorno = Convert.ToDateTime(value).TimeOfDay;
				}
			}
			
			return retorno;
		}

		private static object Cast(object value, Type propType)
		{
			object retorno = value;
			Type type = value.GetType();
			if (type != propType)
			{
				if (propType == typeof(Int16) || propType == typeof(Int16?))
				{
					retorno = Convert.ToInt16(value);
				}
				else if (propType == typeof(Int32) || propType == typeof(Int32?))
				{
					retorno = Convert.ToInt32(value);
				}
				else if (propType == typeof(Int64) || propType == typeof(Int64?))
				{
					retorno = Convert.ToInt64(value);
				}
				else if (propType == typeof(Decimal) || propType == typeof(Decimal?))
				{
					retorno = Convert.ToDecimal(value);
				}
				else if (propType == typeof(String))
				{
					retorno = Convert.ToString(value);
				}
				else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
				{
					retorno = Convert.ToDateTime(value);
				}
				else if (propType == typeof(Boolean) || propType == typeof(Boolean?))
				{
					retorno = Convert.ToBoolean(value);
				}
				else if (propType == typeof(Single) || propType == typeof(Single?))
				{
					retorno = Convert.ToSingle(value);
				}
				else if (propType == typeof(DateTimeOffset) || propType == typeof(DateTimeOffset?))
				{
					retorno = Convert.ToDateTime(value);
				}
				else if (propType == typeof(byte[]))
				{
					retorno = Convert.FromBase64CharArray((char[])value, 0, Convert.ToString(value).Length);
				}
			}
			else
			{
				if (type == typeof(Double))
				{
					retorno = Convert.ToDecimal(value);
				}
				else if (type == typeof(Single))
				{
					retorno = Convert.ToDecimal(value);
				}
				else if (type == typeof(TimeSpan))
				{
					retorno = Convert.ToDateTime(value).TimeOfDay;
				}
			}

			return retorno;
		}

        public static DbType ConvertDbType(Type type)
        {
            DbType dbType = DbType.String;

            if (type == typeof(long))
                dbType = DbType.Int64;
            else if (type == typeof(Int64))
                dbType = DbType.Int64;
            else if (type == typeof(Nullable<long>))
                dbType = DbType.Int64;
            else if (type == typeof(Nullable<Int64>))
                dbType = DbType.Int64;
            else if (type == typeof(UInt64))
                dbType = DbType.UInt64;
            else if (type == typeof(Nullable<UInt64>))
                dbType = DbType.UInt64;
            else if (type == typeof(int))
                dbType = DbType.Int32;
            else if (type == typeof(Nullable<int>))
                dbType = DbType.Int32;
            else if (type == typeof(Int32))
                dbType = DbType.Int32;
            else if (type == typeof(Nullable<Int32>))
                dbType = DbType.Int32;
            else if (type == typeof(uint))
                dbType = DbType.UInt32;
            else if (type == typeof(Nullable<uint>))
                dbType = DbType.UInt32;
            else if (type == typeof(UInt32))
                dbType = DbType.UInt32;
            else if (type == typeof(float))
                dbType = DbType.Single;
            else if (type == typeof(Nullable<float>))
                dbType = DbType.Single;
            else if (type == typeof(DateTime))
                dbType = DbType.DateTime;
            else if (type == typeof(Nullable<DateTime>))
                dbType = DbType.DateTime;
            else if (type == typeof(String))
                dbType = DbType.String;
            else if (type == typeof(string))
                dbType = DbType.String;
            else if (type == typeof(ushort))
                dbType = DbType.UInt16;
            else if (type == typeof(Nullable<ushort>))
                dbType = DbType.UInt16;
            else if (type == typeof(UInt16))
                dbType = DbType.UInt16;
            else if (type == typeof(Nullable<UInt16>))
                dbType = DbType.UInt16;
            else if (type == typeof(short))
                dbType = DbType.Int16;
            else if (type == typeof(Nullable<short>))
                dbType = DbType.Int16;
            else if (type == typeof(Int16))
                dbType = DbType.Int16;
            else if (type == typeof(Nullable<Int16>))
                dbType = DbType.Int16;
            else if (type == typeof(Byte))
                dbType = DbType.SByte;
            else if (type == typeof(Nullable<Byte>))
                dbType = DbType.SByte;
            else if (type == typeof(byte))
                dbType = DbType.SByte;
            else if (type == typeof(Nullable<byte>))
                dbType = DbType.SByte;
            else if (type == typeof(Object))
                dbType = DbType.Object;
            else if (type == typeof(object))
                dbType = DbType.Object;
            else if (type == typeof(Decimal))
                dbType = DbType.Decimal;
            else if (type == typeof(Nullable<Decimal>))
                dbType = DbType.Decimal;
            if (type == typeof(decimal))
                dbType = DbType.Decimal;
            else if (type == typeof(Nullable<decimal>))
                dbType = DbType.Decimal;
            else if (type == typeof(Double))
                dbType = DbType.Currency;
            else if (type == typeof(Nullable<Double>))
                dbType = DbType.Currency;
            else if (type == typeof(double))
                dbType = DbType.Currency;
            else if (type == typeof(Nullable<double>))
                dbType = DbType.Currency;
            else if (type == typeof(byte[]))
                dbType = DbType.Binary;
            else if (type == typeof(Guid))
                dbType = DbType.Guid;
            else if (type == typeof(Boolean))
                dbType = DbType.Boolean;
            else if (type == typeof(Nullable<Boolean>))
                dbType = DbType.Boolean;
            else if (type == typeof(bool))
                dbType = DbType.Boolean;
            else if (type == typeof(Nullable<bool>))
                dbType = DbType.Boolean;

            return dbType;
        }

        #endregion
	}
}
