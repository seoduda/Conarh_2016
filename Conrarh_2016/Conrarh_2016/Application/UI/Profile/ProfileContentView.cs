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
            var userHeaderView = new UserHeaderView(Model, false);
            if (parameters.OnSaveChanges != null)
				userHeaderView.EditUserProfile += parameters.OnSaveChanges;
			
			layout.Children.Add (userHeaderView);

            var badgeView = new BadgeGridView(Model);
			layout.Children.Add (badgeView);

            /*
			if (parameters.IsBtnEnabled) 
			{
				var historyView = new ProfileHistoryView (AppModel.Instance.CurrentContactListWrapper);
				layout.Children.Add (historyView);
			}
            */

            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            // Content = new ScrollView { Content = layout, BackgroundColor = AppResources.UserBackgroundColor };
            //Content = new ScrollView { Content = bgLayout };
            Content = new ContentView { Content = bgLayout };
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