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
	public partial class Shelter
	{
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
		
		private string _p_Name;
		public virtual string P_Name 
		{ 
		    get
		    {
		        return this._p_Name;
		    }
		    set
		    {
		        this._p_Name = value;
		    }
		}
		
		private int? _p_Dummy;
		public virtual int? P_Dummy 
		{ 
		    get
		    {
		        return this._p_Dummy;
		    }
		    set
		    {
		        this._p_Dummy = value;
		    }
		}
		
		private IList<Sheltersrequest> _sheltersrequests = new List<Sheltersrequest>();
		public virtual IList<Sheltersrequest> Sheltersrequests 
		{ 
		    get
		    {
		        return this._sheltersrequests;
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
		
		private IList<Cartstuff> _cartstuffs = new List<Cartstuff>();
		public virtual IList<Cartstuff> Cartstuffs 
		{ 
		    get
		    {
		        return this._cartstuffs;
		    }
		}
		
	}
}