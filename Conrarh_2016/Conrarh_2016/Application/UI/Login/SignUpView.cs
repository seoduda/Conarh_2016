using Xamarin.Forms;
using System;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using Conrarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Login
{
	public sealed class SignUpView : ContentPage
	{
		public event Action<CreateUserData> SignUp;
		public int LeftBorder = 30;

		public SignUpView ():base()
		{
			Title = AppResources.LoginCreateUser;
			NavigationPage.SetHasNavigationBar (this, false);
            BackgroundColor = AppResources.SignUpBgColor;


            var layout = new StackLayout { Padding = new Thickness(0, 20, 0, 0) };

			//layout.Children.Add (new Image { Source = ImageLoader.Instance.GetImage(AppResources.SignUpHeaderImage, false) });

			FillUserDataView fillDataView = new FillUserDataView (new CreateUserData (), AppResources.LoginCreateUser, true);
			fillDataView.Apply += OnApplyData;
			layout.Children.Add (fillDataView);

            BGLayoutView bgLayout = new BGLayoutView(AppResources.SignUpBgImage, layout, false, true);
            //Content = new ScrollView {Content = bgLayout };
            Content = new ContentView { Content = bgLayout };
        }

		void OnApplyData (CreateUserData data)
		{
			if (SignUp != null)
				SignUp (data);
		}

	}

}