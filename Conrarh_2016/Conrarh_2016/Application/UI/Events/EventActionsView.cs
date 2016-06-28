using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventActionsView:ContentPage
	{
		private EditorControl _questionItem;

		const int EditorHeight = 100;

		private UserEventActionsModel Model;

		public EventActionsView (UserEventActionsModel model)
		{
			Model = model;
			Title = "";

			var topItemLayout = new StackLayout { Padding = new Thickness (0, 5, 0, 5), BackgroundColor = AppResources.SpeecherBackColor};
			topItemLayout.Children.Add (GetBoxWithHeader (AppResources.EventsActionHeader, 25, 17));

			var layout = new StackLayout {BackgroundColor= AppResources.SpeecherBackColor};
			layout.Children.Add (topItemLayout);

			AddLikedItemView (Model.Items[0], layout);
			AddLikedItemView (Model.Items[1], layout);

			layout.Children.Add (new ContentView {
				Content = new Label { 
					Text = AppResources.Speakers, 
					FontSize = 20,
					TextColor = AppResources.AgendaCongressoColor
				},
				Padding = new Thickness(30, 10, 0, 0)
			});
			layout.Children.Add (GetSeparator(new Thickness(0, 14, 0, 0)));

			for(int i = 2; i < Model.Items.Count; i++)
				AddLikedItemView (Model.Items[i], layout);

			layout.Children.Add (CreateQuestionsBlock());

			Content = new ScrollView { Content = layout };

			UserController.Instance.UpdateEventUserVotes (Model);
		}

		private void AddLikedItemView(LikedItem dataModel, StackLayout parent)
		{
			LikedItemView result = new LikedItemView (dataModel);
			result.Clicked += OnLikedItemClicked;
			parent.Children.Add (result);
			parent.Children.Add (GetSeparator());
		}

		private void OnLikedItemClicked (LikedItem likedItem, bool newState)
		{
			UserController.Instance.PostVote (likedItem.VoteData, newState, likedItem);
		}

		private StackLayout CreateQuestionsBlock()
		{
			var bottomItemLayout = new StackLayout { 
				Padding = new Thickness (0, 0, 0, 20), 
				BackgroundColor = AppResources.SpeecherBackColor
			};

			_questionItem = new EditorControl(AppResources.EventsQuestionDefaultAnswer) {
				HeightRequest = EditorHeight,
				WidthRequest = AppProvider.Screen.Width - 60,
			};
			bottomItemLayout.Children.Add (_questionItem);

			float btnWidth = (AppProvider.Screen.Width - 60) / 2;
			var btnEnviar = new Button { 
				BackgroundColor = AppResources.AgendaPageBackgroundColor,
				WidthRequest = btnWidth,
				HeightRequest = 40,
				Text = AppResources.EventsActionPostQuestion,
				TextColor = Color.White,
				BorderRadius = 0
			};
			btnEnviar.Clicked += OnSendQuestionClicked;
			bottomItemLayout.Children.Add (btnEnviar);

			return bottomItemLayout;
		}
			
		private void OnSendQuestionClicked (object sender, System.EventArgs e)
		{
			UserController.Instance.PostQuestion (Model.Data, _questionItem.Text, OnSentDone);
		}

		private void OnSentDone()
		{
			_questionItem.Text = _questionItem.PlaceHolderText;
		}

		private View GetSeparator(Thickness? padding = null)
		{
			var absoluteLayout = new AbsoluteLayout { HorizontalOptions = LayoutOptions.Center};
			if (padding.HasValue)
				absoluteLayout.Padding = padding.Value;
			
			absoluteLayout.Children.Add (new BoxView {
				HeightRequest = 0.5f,
				WidthRequest = AppProvider.Screen.Width - 20,
				BackgroundColor = AppResources.SeparatorColor
			});
			return absoluteLayout;
		}

		private View GetBoxWithHeader(string text, int boxHeight, int fontSize)
		{
			return new ContentView{ 
				BackgroundColor = AppResources.AgendaCongressoColor,
				HorizontalOptions = LayoutOptions.Center, 
				VerticalOptions = LayoutOptions.Center,
				Content = new Label { 
					Text = text, 
					FontSize = fontSize,
					TextColor = Color.White,
					XAlign = TextAlignment.Center,
					WidthRequest = AppProvider.Screen.Width,
					YAlign = TextAlignment.Center
				}
			};
		}
	}
}

