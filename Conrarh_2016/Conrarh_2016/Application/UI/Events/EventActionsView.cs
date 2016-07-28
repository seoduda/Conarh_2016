using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;
using Conrarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventActionsView:ContentPage
	{
		private EditorControl _questionItem;

		const int EditorHeight = 80;

		private UserEventActionsModel Model;

		public EventActionsView (UserEventActionsModel model)
		{
			Model = model;
			Title = AppResources.EventsActionHeader;
            
            /*
            var topItemLayout = new StackLayout { Padding = new Thickness (0, 5, 0, 5), BackgroundColor = AppResources.SpeecherBgColor};
			topItemLayout.Children.Add (GetBoxWithHeader (AppResources.EventsActionHeader, 25, 17));
            */


			var layout = new StackLayout {BackgroundColor= AppResources.SpeecherBgColor};
			//layout.Children.Add (topItemLayout);

			AddLikedItemView (Model.Items[0], layout);
			AddLikedItemView (Model.Items[1], layout);

			layout.Children.Add (new ContentView {
				Content = new Label { 
					Text = AppResources.Speakers, 
					FontSize = 20,
					TextColor = AppResources.EventActionsTextColor
                },
				Padding = new Thickness(40, 10, 0, 0)
			});
			layout.Children.Add (GetSeparator(new Thickness(0, 8, 0, 0)));

			for(int i = 2; i < Model.Items.Count; i++)
				AddLikedItemView (Model.Items[i], layout);

			layout.Children.Add (CreateQuestionsBlock());

            //BGLayoutView bgLayout = new BGLayoutView(AppResources.InteractBgImage, layout, true, true);
            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            //Content = new ScrollView {Content = bgLayout };
            Content = new ContentView { Content = bgLayout };

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
            
            var questionBoxLayout = new StackLayout
            {
                Padding = new Thickness(15, 5, 15, 5),
                BackgroundColor = AppResources.EventActionsQuestionsBlockColor,
                Opacity = 0.45,
                Spacing =0,
            };
            /*
            Button questionsBlockTitle = new Button
            {
                //Text = AppResources.EventsQuestionHeader,
                //FontSize = 20,
                HeightRequest = 25,
                WidthRequest = AppProvider.Screen.Width - 50,
                BackgroundColor = AppResources.EventActionsQuestionsBlockTitleColor,
                Text = "Jurema",
            //    TextColor = AppResources.EventActionsQuestionsBlockTitleTextColor,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Center,
                BorderRadius = 0
                //IsEnabled = false
            };
            
            questionBoxLayout.Children.Add(questionsBlockTitle);
            */
            //_questionItem = new EditorControl(AppResources.EventsQuestionDefaultAnswer)
            _questionItem = new EditorControl(AppResources.EventsQuestionDefaultAnswer)
            {
                HeightRequest = EditorHeight,
                //WidthRequest = AppProvider.Screen.Width - 60,
                WidthRequest = AppProvider.Screen.Width - 50,
            };

            questionBoxLayout.Children.Add(_questionItem);

            Button btnEnviar = new Button
            {
                BackgroundColor = AppResources.EventActionsQuestionsBlockBtnBgColor,
                HeightRequest = 40,
                //WidthRequest = AppProvider.Screen.Width - 50,
                Text = AppResources.EventsActionPostQuestion,
                TextColor = Color.White,
                BorderRadius = 0
            };
            btnEnviar.Clicked += OnSendQuestionClicked;

            questionBoxLayout.Children.Add(btnEnviar);
            


            return questionBoxLayout;
		}
			
		private void OnSendQuestionClicked (object sender, System.EventArgs e)
		{
            /**TODO Ativar envio de pergunta OnSendQuestionClicked  EventActionsView
			UserController.Instance.PostQuestion (Model.Data, _questionItem.Text, OnSentDone);
            */
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

        /*
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
        */
	}
}

