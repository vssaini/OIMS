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
	public partial class Item
	{
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
		
		private string _i_Name;
		public virtual string I_Name 
		{ 
		    get
		    {
		        return this._i_Name;
		    }
		    set
		    {
		        this._i_Name = value;
		    }
		}
		
		private float _i_Quantity;
		public virtual float I_Quantity 
		{ 
		    get
		    {
		        return this._i_Quantity;
		    }
		    set
		    {
		        this._i_Quantity = value;
		    }
		}
		
		private string _size;
		public virtual string Size 
		{ 
		    get
		    {
		        return this._size;
		    }
		    set
		    {
		        this._size = value;
		    }
		}
		
		private string _marking;
		public virtual string Marking 
		{ 
		    get
		    {
		        return this._marking;
		    }
		    set
		    {
		        this._marking = value;
		    }
		}
		
		private string _vendor;
		public virtual string Vendor 
		{ 
		    get
		    {
		        return this._vendor;
		    }
		    set
		    {
		        this._vendor = value;
		    }
		}
		
		private DateTime? _updatedOn;
		public virtual DateTime? UpdatedOn 
		{ 
		    get
		    {
		        return this._updatedOn;
		    }
		    set
		    {
		        this._updatedOn = value;
		    }
		}
		
		private IList<Shelterdescription> _shelterdescriptions = new List<Shelterdescription>();
		public virtual IList<Shelterdescription> Shelterdescriptions 
		{ 
		    get
		    {
		        return this._shelterdescriptions;
		    }
		}
		
		private IList<Itemsrequest> _itemsrequests = new List<Itemsrequest>();
		public virtual IList<Itemsrequest> Itemsrequests 
		{ 
		    get
		    {
		        return this._itemsrequests;
		    }
		}
		
		private IList<Itemslog> _itemslogs = new List<Itemslog>();
		public virtual IList<Itemslog> Itemslogs 
		{ 
		    get
		    {
		        return this._itemslogs;
		    }
		}
		
		private IList<Cartstuff> _cartstuffs = new List<Cartstuff>();
		public virtual IList<Cartstuff> Cartstuffs 
		{ 
		    get
		    {
		        return this._cartstuffs;
		    }
		}
		
		private IList<Allocated> _allocateds = new List<Allocated>();
		public virtual IList<Allocated> Allocateds 
		{ 
		    get
		    {
		        return this._allocateds;
		    }
		}
		
	}
}
