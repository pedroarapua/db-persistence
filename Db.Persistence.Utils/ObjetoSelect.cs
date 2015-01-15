using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Db.Persistence.Utils
{
	public class ObjetoSelect
	{

		#region     .....:::::     ATRIBUTOS     :::::.....

		private Type tipo;
		private Type tipoReferencia;
		private PropertyInfo propriedadeReferencia;
		private List<PropertyInfo> propriedades;
		private ObjetoSelect objetoParent;
		private String alias;
		private List<ObjetoSelect> objetosSelect;
		private List<Condition> condicoes;
		private List<OrderBy> ordenacoes;

		#endregion

		#region     .....:::::     CONSTRUTORES     :::::.....

		public ObjetoSelect()
		{
			this.propriedades = new List<PropertyInfo>();
			this.objetosSelect = new List<ObjetoSelect>();
			this.condicoes = new List<Condition>();
			this.ordenacoes = new List<OrderBy>();
		}

		#endregion

		#region     .....:::::     PROPRIEDADES     :::::.....

		public Type Tipo 
		{
			get { return this.tipo; }
			set { this.tipo = value; }
		}

		public Type TipoReferencia 
		{
			get { return this.tipoReferencia; }
			set { this.tipoReferencia = value; }
		}
		
		public PropertyInfo PropriedadeReferencia 
		{
			get { return this.propriedadeReferencia; }
			set { this.propriedadeReferencia = value; }
		}
		
		public List<PropertyInfo> Propriedades 
		{
			get { return this.propriedades; }
			set { this.propriedades = value; }
		}

		public List<ObjetoSelect> ObjetosSelect
		{
			get { return this.objetosSelect; }
			set { this.objetosSelect = value; }
		}

		public ObjetoSelect ObjetoParent
		{
			get { return this.objetoParent; }
			set { this.objetoParent = value; }
		}

		public String Alias
		{
			get { return this.alias; }
			set { this.alias = value; }
		}

		public List<Condition> Condicoes
		{
			get { return this.condicoes; }
			set { this.condicoes = value; }
		}

		public List<OrderBy> Ordenacoes
		{
			get { return this.ordenacoes; }
			set { this.ordenacoes = value; }
		}

		#endregion

	}
}
