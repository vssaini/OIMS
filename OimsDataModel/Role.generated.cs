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
	public partial class Role
	{
		private int _r_Id;
		public virtual int R_Id 
		{ 
		    get
		    {
		        return this._r_Id;
		    }
		    set
		    {
		        this._r_Id = value;
		    }
		}
		
		private string _r_Name;
		public virtual string R_Name 
		{ 
		    get
		    {
		        return this._r_Name;
		    }
		    set
		    {
		        this._r_Name = value;
		    }
		}
		
		private IList<User> _users = new List<User>();
		public virtual IList<User> Users 
		{ 
		    get
		    {
		        return this._users;
		    }
		}
		
	}
}
