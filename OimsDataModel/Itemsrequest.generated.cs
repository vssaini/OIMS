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
	public partial class Itemsrequest
	{
		private int _oI_Id;
		public virtual int OI_Id 
		{ 
		    get
		    {
		        return this._oI_Id;
		    }
		    set
		    {
		        this._oI_Id = value;
		    }
		}
		
		private int _o_Id;
		public virtual int O_Id 
		{ 
		    get
		    {
		        return this._o_Id;
		    }
		    set
		    {
		        this._o_Id = value;
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
		
		private float _oI_Quantity;
		public virtual float OI_Quantity 
		{ 
		    get
		    {
		        return this._oI_Quantity;
		    }
		    set
		    {
		        this._oI_Quantity = value;
		    }
		}
		
		private float? _oI_ItemRecom;
		public virtual float? OI_ItemRecom 
		{ 
		    get
		    {
		        return this._oI_ItemRecom;
		    }
		    set
		    {
		        this._oI_ItemRecom = value;
		    }
		}
		
		private float? _oI_ItemAlloc;
		public virtual float? OI_ItemAlloc 
		{ 
		    get
		    {
		        return this._oI_ItemAlloc;
		    }
		    set
		    {
		        this._oI_ItemAlloc = value;
		    }
		}
		
		private float? _oI_Pending;
		public virtual float? OI_Pending 
		{ 
		    get
		    {
		        return this._oI_Pending;
		    }
		    set
		    {
		        this._oI_Pending = value;
		    }
		}
		
		private int _oI_CreatedBy;
		public virtual int OI_CreatedBy 
		{ 
		    get
		    {
		        return this._oI_CreatedBy;
		    }
		    set
		    {
		        this._oI_CreatedBy = value;
		    }
		}
		
		private DateTime _oI_CreatedDate;
		public virtual DateTime OI_CreatedDate 
		{ 
		    get
		    {
		        return this._oI_CreatedDate;
		    }
		    set
		    {
		        this._oI_CreatedDate = value;
		    }
		}
		
		private int? _oI_UpdatedBy;
		public virtual int? OI_UpdatedBy 
		{ 
		    get
		    {
		        return this._oI_UpdatedBy;
		    }
		    set
		    {
		        this._oI_UpdatedBy = value;
		    }
		}
		
		private DateTime? _oI_UpdatedDate;
		public virtual DateTime? OI_UpdatedDate 
		{ 
		    get
		    {
		        return this._oI_UpdatedDate;
		    }
		    set
		    {
		        this._oI_UpdatedDate = value;
		    }
		}
		
		private Request _request;
		public virtual Request Request 
		{ 
		    get
		    {
		        return this._request;
		    }
		    set
		    {
		        this._request = value;
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
		
	}
}