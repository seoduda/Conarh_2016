using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms;
using SQLite.Net.Attributes;
using Conarh_2016.Application.Domain.JsonConverters;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<EventData>))]
	public sealed class EventData:UpdatedUniqueItem
	{
		public const string BackgroundImagePropertyName = "BackgroundImageSource";
		public const string PointsImagePathPropertyName = "PointsImagePath";
		public const string SponsorImagePathPropertyName = "SponsorImagePath";
		public const string TitlePropertyName = "Title";
		public const string DescriptionPropertyName = "Description";
		public const string TakePlaceAuditoriumPropertyName = "TakePlaceAuditorium";
		public const string TimeDurationPropertyName = "TimeDuration";
		public const string DatePropertyName = "Date";
		public const string SpeechersPropertyName = "Speechers";
		public const string EventPlacePropertyName = "EventPlace";
		public const string BackgroundColorPropertyName = "BackgroundColor";
		public const string BackgroundColorNonOpacityPropertyName = "BackgroundColorNonOpacity";
		public const string FavEventImagePropertyName = "FavEventImage";

		public ImageSource BackgroundImageSource
		{
			get 
			{
				var image = AppResources.GetImageSource (BackgroundImageId);
				return image;
			}
		}

		[JsonProperty(JsonKeys.BackgroundImagePath)]
		public int BackgroundImageId
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.PointsImagePath)]
		public string PointsImagePath
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.SponsorImagePath)]
		public string SponsorImagePath
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Title)]
		public string Title
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Description)]
		public string Description
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.TakePlaceAuditorium)]
		public string TakePlaceAuditorium
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.FreeAttending)]
		public bool FreeAttending
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.TimeDuration)]
		public string TimeDuration
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Date)]
		public DateTime Date
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Speechers)]
		[Ignore]
		public List<Speaker> Speechers { get; set; }

		[JsonIgnore]
		public string SpeechersList
		{
			set;
			get;
		}

		public override string ToString()
		{
			string speechers = string.Empty;

			if (Speechers != null) 
			{
				foreach (Speaker speaker in Speechers)
					speechers = string.Format ("{0}{1}|", speechers, speaker.Id);
			}
			
			return string.Format ("[EventData] [ Id: {0} Title: {1} Description: {2} TakePlaceAuditorium: {3} FreeAttending: {4} Date: {5} TimeDuration: {6} Speechers: {7} ]",
				Id, Title, Description, TakePlaceAuditorium, FreeAttending, Date, TimeDuration, speechers);
		}

		public new static class JsonKeys
		{
			public const string BackgroundImagePath = "background_image";
			public const string PointsImagePath = "points_image";
			public const string SponsorImagePath = "sponsor_image";

			public const string Description = "description";
			public const string Title = "title";
			public const string FreeAttending = "free_attending";
			public const string TakePlaceAuditorium = "auditorium";

			public const string TimeDuration = "time_schedule";
			public const string Date = "date";

			public const string Speechers = "speechers";
		}

		#region Binding UIData

		[Ignore]
		public string EventPlace { get { return this.TakePlaceAuditorium.ToUpper ();}}

		[Ignore]
		public Color BackgroundColor 
		{
			get 
			{ 
				var result = BackgroundColorNonOpacity;
				return new Color (result.R, result.G, result.B, 0.95f);
			}
		}

		[Ignore]
		public Color BackgroundColorNonOpacity 
		{
			get 
			{ 
				return this.FreeAttending ? AppResources.AgendaExpoColor : AppResources.AgendaCongressoColor;
			}
		}

		#endregion
	}
}