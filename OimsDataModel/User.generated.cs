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
	public partial class User
	{
		private int _u_Id;
		public virtual int U_Id 
		{ 
		    get
		    {
		        return this._u_Id;
		    }
		    set
		    {
		        this._u_Id = value;
		    }
		}
		
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
		
		private string _u_FirstName;
		public virtual string U_FirstName 
		{ 
		    get
		    {
		        return this._u_FirstName;
		    }
		    set
		    {
		        this._u_FirstName = value;
		    }
		}
		
		private string _u_LastName;
		public virtual string U_LastName 
		{ 
		    get
		    {
		        return this._u_LastName;
		    }
		    set
		    {
		        this._u_LastName = value;
		    }
		}
		
		private string _u_Email;
		public virtual string U_Email 
		{ 
		    get
		    {
		        return this._u_Email;
		    }
		    set
		    {
		        this._u_Email = value;
		    }
		}
		
		private string _u_Password;
		public virtual string U_Password 
		{ 
		    get
		    {
		        return this._u_Password;
		    }
		    set
		    {
		        this._u_Password = value;
		    }
		}
		
		private Role _role;
		public virtual Role Role 
		{ 
		    get
		    {
		        return this._role;
		    }
		    set
		    {
		        this._role = value;
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