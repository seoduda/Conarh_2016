﻿using Xamarin.Forms;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using System;

namespace Conarh_2016.Application.UI.Connect
{

    public enum ContactListType
    {
        All,
        Pending,
        Ranking
    }

    public sealed class ConnectListWrapper: PullRefreshListWrapper
	{
		public ConnectionsDataWrapper CurrentModel;
		private int progress = 0;

		private bool IsSearching = false;
		private ConnectionsDataWrapper SearchDataWrapper;
		private string Pattern = string.Empty;

		public ConnectListWrapper(ConnectionsDataWrapper currentModel)
		{
			CurrentModel = currentModel;
		}

		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			progress = 0;

			if (IsSearching)
				AppController.Instance.SearchUsers (SearchDataWrapper.SearchUsers, Pattern, Increase);
			else
				AppController.Instance.DownloadAllUsers (Increase);

			UserController.Instance.UpdateProfileData (CurrentModel.LoginedUser, true, Increase);
		}

		public void Increase()
		{
			progress += 1;
			if (progress == 2)
				Done ();
		}


		public void Search(ConnectionsDataWrapper dataWrapper, string pattern)
		{
			IsBusy = false;

			IsSearching = true;
			Pattern = pattern;
			SearchDataWrapper = dataWrapper;
			OnAction ();
		}

		public void ClearSearch()
		{
			IsBusy = false;
			IsSearching = false;

			OnAction ();
		}
	}

	public sealed class ConnectionPage : ContentPage
	{
        public ContactListType selectedContactedList = ContactListType.All;
        public ConnectionsDataWrapper CurrentModel { private set; get; }
		public readonly ListView UserListView;

		private readonly ConnectListWrapper _wrapper;

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (AppProvider.ImageCache != null)
				AppProvider.ImageCache.Clear ();

			if (AppProvider.FastCellCache != null)
				AppProvider.FastCellCache.FlushAllCaches ();
		}

		public ConnectionPage ()
		{
			Title = AppResources.Connect;
			BackgroundColor = Color.Transparent;

            CurrentModel = AppModel.Instance.CurrentConnectionsWrapper;

            UserController.Instance.UpdateProfileData(CurrentModel.LoginedUser);

            var searchBarView = new SearchBarView ();
			searchBarView.Clear += OnSearchClear;
			searchBarView.Search += OnSearch;

			_wrapper = new ConnectListWrapper (CurrentModel);
			UserListView = new ListView {
				RefreshCommand = _wrapper.RefreshCommand,
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(ConnectionCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				IsPullToRefreshEnabled = true,
				ItemsSource = CurrentModel,
				BindingContext = _wrapper
			};
			UserListView.SetBinding<ConnectListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);


            var buttonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.Fill,
                Children =  {
                    new ContentView {Content = GetButton(AppResources.ProfileAllContacsBtnHeader, AppResources.MenuColor, OnAllContactsClicked), WidthRequest = AppProvider.Screen.Width / 3},
                    new ContentView {Content = GetButton (AppResources.ProfilePendingContacsBtnHeader, AppResources.AgendaCongressoColor, OnPendingContactsClicked), WidthRequest = AppProvider.Screen.Width / 3},
                    new ContentView {Content = GetButton(AppResources.ProfileRatingBtnHeader, AppResources.AgendaExpoColor, OnRankingClicked), WidthRequest = AppProvider.Screen.Width / 3},
                },
                Padding = new Thickness(0)
            };


            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new UserHeaderView (CurrentModel.LoginedUser, false, false),
                    buttonLayout,
                    searchBarView,
                    UserListView
                }
            };

            //BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, false, true);

            Content = bgLayout;

        }

        private void OnRankingClicked(object sender, EventArgs e)
        {
            if (selectedContactedList == ContactListType.Ranking)
                return;

            selectedContactedList = ContactListType.Ranking;
        }

        private void OnPendingContactsClicked(object sender, EventArgs e)
        {
            if (selectedContactedList == ContactListType.Pending)
                return;

            selectedContactedList = ContactListType.Pending;

        }

        private void OnAllContactsClicked(object sender, EventArgs e)
        {
            if (selectedContactedList == ContactListType.All)
                return;

            selectedContactedList = ContactListType.All;
        }

        protected override void OnAppearing ()
		{
			base.OnAppearing ();
			UserListView.BeginRefresh ();
		}


        private Button GetButton(string title, Color color, EventHandler handler)
        {
            var btn = new Button()
            {
                BorderRadius = 0,
                Text = title.ToUpper(),
                BackgroundColor = color,
                TextColor = Color.White,
                FontSize = 15,
            };
            btn.Clicked += handler;
            return btn;
        }


        private string previousPattern = string.Empty;
		void OnSearch (string pattern)
		{
			if (previousPattern.Equals (pattern))
				return;

			previousPattern = pattern;
			CurrentModel = new ConnectionsDataWrapper(AppModel.Instance.CurrentUser,
				new DynamicListData<User>(), AppModel.Instance.UsersModelsWrapper);
			
			UserListView.ItemsSource = CurrentModel;

			_wrapper.Search (CurrentModel, previousPattern);
		}

		void OnSearchClear ()
		{
			previousPattern = string.Empty;
			CurrentModel = AppModel.Instance.CurrentConnectionsWrapper;
			UserListView.ItemsSource = CurrentModel;

			_wrapper.ClearSearch ();
		}
	}

}