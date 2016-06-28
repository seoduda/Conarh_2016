using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfileContentView : ContentView
	{
		public class Parameters
		{
			public UserModel UserModel;
			public bool IsBtnEnabled = false;
			public EventHandler OnRating = null;
			public EventHandler OnContactList = null;
			public Action OnSaveChanges = null;
		}
		
		public readonly UserModel Model;

		public ProfileContentView (Parameters parameters )
		{
			Model = parameters.UserModel;
			var layout = new StackLayout {VerticalOptions = LayoutOptions.FillAndExpand};
			var userHeaderView = new UserHeaderView (Model, true);
			if(parameters.OnSaveChanges != null)
				userHeaderView.EditUserProfile += parameters.OnSaveChanges;
			
			layout.Children.Add (userHeaderView);

			if (parameters.IsBtnEnabled) {
				var btnLayout = new StackLayout { Padding = new Thickness (0),
					Orientation = StackOrientation.Horizontal};

				btnLayout.Children.Add (GetButton (AppResources.ProfileRatingBtnColor, 
					AppResources.ProfileRatingBtnHeader, parameters.OnRating, new Thickness(20, 0, 10, 0)) );
				
				btnLayout.Children.Add (GetButton (AppResources.ProfileContactListBtnColor, 
					AppResources.ProfileContactListBtnHeader, parameters.OnContactList, new Thickness(10, 0, 20, 0)));

				layout.Children.Add (btnLayout);
			}

			var badgeView = new BadgeGridView(Model);
			layout.Children.Add (badgeView);

			if (parameters.IsBtnEnabled) 
			{
				var historyView = new ProfileHistoryView (AppModel.Instance.CurrentContactListWrapper);
				layout.Children.Add (historyView);
			}
			Content = new ScrollView { Content = layout, BackgroundColor = AppResources.UserBackgroundColor };
		}

		private ContentView GetButton(Color btnColor, string name, EventHandler onClicked, Thickness padding)
		{
			var btn = new Button { Text = name, FontAttributes = FontAttributes.Bold,
				FontSize = 17, BackgroundColor = btnColor, TextColor = Color.White, BorderRadius = 0};

			if(onClicked != null)
				btn.Clicked += onClicked;
			
			return new ContentView {Content = btn, Padding = padding,  WidthRequest = AppProvider.Screen.Width / 2};
		}
	}
}