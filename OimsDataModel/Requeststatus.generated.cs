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
	public partial class Requeststatus
	{
		private int _s_Id;
		public virtual int S_Id 
		{ 
		    get
		    {
		        return this._s_Id;
		    }
		    set
		    {
		        this._s_Id = value;
		    }
		}
		
		private string _s_Name;
		public virtual string S_Name 
		{ 
		    get
		    {
		        return this._s_Name;
		    }
		    set
		    {
		        this._s_Name = value;
		    }
		}
		
		private int _forRole;
		public virtual int ForRole 
		{ 
		    get
		    {
		        return this._forRole;
		    }
		    set
		    {
		        this._forRole = value;
		    }
		}
		
		private IList<Request> _requests = new List<Request>();
		public virtual IList<Request> Requests 
		{ 
		    get
		    {
		        return this._requests;
		    }
		}
		
	}
}