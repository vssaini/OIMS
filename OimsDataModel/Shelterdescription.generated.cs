#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using OimsDataModel;


namespace OimsDataModel	
{
	public partial class Shelterdescription
	{
		private int _pD_Id;
		public virtual int PD_Id 
		{ 
		    get
		    {
		        return this._pD_Id;
		    }
		    set
		    {
		        this._pD_Id = value;
		    }
		}
		
		private int _p_Id;
		public virtual int P_Id 
		{ 
		    get
		    {
		        return this._p_Id;
		    }
		    set
		    {
		        this._p_Id = value;
		    }
		}
		
		private int _i_Id;
		public virtual int I_Id 
		{ 
		    get
		    {
		        return this._i_Id;
		    }
		    set
		    {
		        this._i_Id = value;
		    }
		}
		
		private int _i_Qty;
		public virtual int I_Qty 
		{ 
		    get
		    {
		        return this._i_Qty;
		    }
		    set
		    {
		        this._i_Qty = value;
		    }
		}
		
		private Item _item;
		public virtual Item Item 
		{ 
		    get
		    {
		        return this._item;
		    }
		    set
		    {
		        this._item = value;
		    }
		}
		
		private Shelter _shelter;
		public virtual Shelter Shelter 
		{ 
		    get
		    {
		        return this._shelter;
		    }
		    set
		    {
		        this._shelter = value;
		    }
		}
		
	}
}
