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
	public partial class Allocated
	{
		private int _id;
		public virtual int Id 
		{ 
		    get
		    {
		        return this._id;
		    }
		    set
		    {
		        this._id = value;
		    }
		}
		
		private int _userId;
		public virtual int UserId 
		{ 
		    get
		    {
		        return this._userId;
		    }
		    set
		    {
		        this._userId = value;
		    }
		}
		
		private int _orderId;
		public virtual int OrderId 
		{ 
		    get
		    {
		        return this._orderId;
		    }
		    set
		    {
		        this._orderId = value;
		    }
		}
		
		private int _itemId;
		public virtual int ItemId 
		{ 
		    get
		    {
		        return this._itemId;
		    }
		    set
		    {
		        this._itemId = value;
		    }
		}
		
		private float _markQty;
		public virtual float MarkQty 
		{ 
		    get
		    {
		        return this._markQty;
		    }
		    set
		    {
		        this._markQty = value;
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
		
		private User _user;
		public virtual User User 
		{ 
		    get
		    {
		        return this._user;
		    }
		    set
		    {
		        this._user = value;
		    }
		}
		
	}
}