using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Conarh_2016.Application.Domain
{
	public class UniqueItem
	{
		[JsonProperty(JsonKeys.Id, NullValueHandling = NullValueHandling.Ignore)]
		[PrimaryKey, Unique]
		public string Id
		{
			set;
			get;
		}
        /*
        [JsonProperty(JsonKeys.XID)]
        public string Xid
        {
            set;
            get;
        }
        */

        public static class JsonKeys
		{
			public const string Id = "_id";
            //public const string XID = "id";
        }

		public override string ToString ()
		{
			return Id;
		}

		public virtual void UpdateWithItem(UniqueItem item)
		{
		}
	}

	public class UpdatedUniqueItem:UniqueItem, IComparable<UpdatedUniqueItem>
	{
		#region IComparable implementation

		public virtual int CompareTo (UpdatedUniqueItem other)
		{
			return other.UpdatedAtTime.CompareTo (UpdatedAtTime);
		}

		#endregion

		public const string UpdatedAtTimePropertyName = "UpdatedAtTime";

		[JsonProperty(JsonKeys.UpdatedAtTime,NullValueHandling = NullValueHandling.Ignore)]
		public virtual DateTime UpdatedAtTime
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.CreatedAtTime, NullValueHandling = NullValueHandling.Ignore)]
		public virtual DateTime CreatedAtTime
		{
			set;
			get;
		}


		public new static class JsonKeys
		{
			public const string UpdatedAtTime = "updated_at";
			public const string CreatedAtTime = "created_at";
		}

	}
}