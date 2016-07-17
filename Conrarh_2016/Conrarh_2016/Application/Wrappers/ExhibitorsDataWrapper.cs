using System.Collections.Generic;
using System.Collections.ObjectModel;
using Conarh_2016.Application.Domain;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class ExhibitorsDynamicObservableData:DynamicObservableData<Exhibitor>
	{
		public const string SectionNamePropertyName = "SectionName";
		public const string SectionColorPropertyName = "SectionColor";

		public readonly SponsorType Sponsor;
		public ExhibitorsDynamicObservableData(SponsorType sponsorType) : base(true)
		{
			Sponsor = sponsorType;
		}

		public string SectionName
		{
			get 
			{
				return Sponsor.Title.ToUpper ();
			}
		}

		public Color SectionColor
		{
			get 
			{
				return Color.FromHex (Sponsor.Color);
			}
		}
	}
	
	public sealed class ExhibitorsDataWrapper:ObservableCollection<ExhibitorsDynamicObservableData>
	{
		public readonly DynamicListData<SponsorType> Sponsors;
		public readonly DynamicListData<Exhibitor> Exhibitors;

		private readonly Dictionary<SponsorType, ExhibitorsDynamicObservableData> Data;
		private readonly bool DontShowCategoryIfEmpty;

		public ExhibitorsDataWrapper( DynamicListData<SponsorType> sponsors,
			DynamicListData<Exhibitor> exhibitors, bool dontShowCategoryIfEmpty)
		{
			Data = new Dictionary<SponsorType, ExhibitorsDynamicObservableData> ();
			DontShowCategoryIfEmpty = dontShowCategoryIfEmpty;
			Sponsors = sponsors;
			Exhibitors = exhibitors;

			if (!Sponsors.IsEmpty ())
				OnSponsorTypesChanged (Sponsors.Items);
			
			Sponsors.CollectionChanged += OnSponsorTypesChanged;

			if (!Exhibitors.IsEmpty ())
				OnExhibitorsChanged (Exhibitors.Items);
				
			Exhibitors.CollectionChanged += OnExhibitorsChanged;
		}

		private void OnExhibitorsChanged (List<Exhibitor> exhibitors)
		{
			if (exhibitors == null)
				return;
            

            if (DontShowCategoryIfEmpty) 
			{
				var items = new Dictionary<SponsorType, List<Exhibitor>>();
				foreach (Exhibitor exhibitor in exhibitors) {
					SponsorType sponsor = Sponsors.Find (exhibitor.SponsorType.Id);
					if (!items.ContainsKey (sponsor))
						items.Add (sponsor, new List<Exhibitor> ());
					
					items [sponsor].Add (exhibitor);
				}

				Device.BeginInvokeOnMainThread (() => {
					foreach (SponsorType sponsorType in items.Keys) {

						if(!Data.ContainsKey(sponsorType))
						{
							var collection = new ExhibitorsDynamicObservableData(sponsorType);
							Data.Add(sponsorType, collection);

							Items.Add (collection);
							OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection));
						}
						Data[sponsorType].UpdateData(items[sponsorType]);
					}
				});
			} 
			else 
			{
				foreach (ExhibitorsDynamicObservableData data in Items) {
                    List<Exhibitor> lexh = new List<Exhibitor>();

                    foreach (Exhibitor exh in exhibitors)
                    {
                        if (exh.SponsorType.Type == data.Sponsor.Type)
                        {
                            lexh.Add(exh);
                        }
                    }
                    data.UpdateData(lexh);
                    //data.UpdateData(exhibitors.FindAll(temp => temp.SponsorTypeId.Equals(data.Sponsor.Type)));
                    //data.UpdateData (exhibitors.FindAll (temp => temp.SponsorType.Id.Equals (data.Sponsor.Id)));
                }
			}
		}

		private void OnSponsorTypesChanged (List<SponsorType> sponsorTypes)
		{
			if (Items.Count > 0)
				return;

			if (DontShowCategoryIfEmpty)
				return;

			Device.BeginInvokeOnMainThread (() => {
				foreach (SponsorType sponsorType in sponsorTypes) {
					var data = new ExhibitorsDynamicObservableData (sponsorType);
					Items.Add (data);

					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, data));
				}
			});
		}

	}
}

