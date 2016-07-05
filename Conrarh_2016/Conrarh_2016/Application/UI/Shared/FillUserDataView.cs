using Xamarin.Forms;
using System;
using Conarh_2016.Core;
using System.Collections.Generic;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.UI.Shared;
using System.IO;
using Conarh_2016.Application.UI.Login;

namespace Conarh_2016.Application.UI.Shared
{
	public sealed class FillUserDataView : ContentView
	{
		private InputFieldView _emailEntry;
		private InputFieldView _passwordEntry;
		private InputFieldView _nameEntry;
		private InputFieldView _surnameEntry;
		private InputFieldView _jobEntry;
		private InputFieldView _phoneEntry;

		public int LeftBorder = 30;

		private Image _profileImage;

		private bool _isProfileImageChanged = false;

		public event Action<CreateUserData> Apply;

		public CreateUserData Model;

		private List<InputFieldView> _requiredFields;

		private string _fakeProfileImagePath;

		public FillUserDataView (CreateUserData userData, string buttonName, bool isPasswordEnabled)
		{
			_fakeProfileImagePath = Path.Combine(AppProvider.IOManager.DocumentPath, "fakeProfileImage.jpeg");
			Model = userData;
			var layout = new StackLayout {
				BackgroundColor = Color.Transparent,
				Padding = new Thickness(0, 0, 10, 0)
			};

			var topLayout = new StackLayout { 
				Orientation = StackOrientation.Horizontal, 
				Padding = new Thickness(10, 0, 0, 0) 
			};

			string imagePath = string.IsNullOrEmpty (userData.ProfileImage) ? AppResources.DefaultUserImage : userData.ProfileImage;
			var imageLayout =  new StackLayout {};
			_profileImage = new Image {
				WidthRequest = 100,
				HeightRequest = 100,
				Source = ImageLoader.Instance.GetImage(imagePath, false)
			};

			imageLayout.Children.Add (_profileImage);

			var btnUpdatePicture = new Button {
				WidthRequest = 100,
				FontSize = 15,
				Text = AppResources.SignUpChooseImage,
				BackgroundColor = Color.Transparent,
				TextColor = AppResources.SignUpChooseImageButtonColor,
				BorderRadius = 0,
				BorderWidth = 2,
				BorderColor = AppResources.SignUpChooseImageButtonColor
			};

			#if __ANDROID__
				btnUpdatePicture.TextColor = Color.White;
				btnUpdatePicture.BackgroundColor = AppResources.SignUpChooseImageButtonColor;
			#endif

			btnUpdatePicture.Clicked += OnImageClicked;
			imageLayout.Children.Add (btnUpdatePicture);

			topLayout.Children.Add (imageLayout);

			var nameSurnameLayout = new StackLayout();
			nameSurnameLayout.Children.Add (GetEntry (Keyboard.Create(KeyboardFlags.CapitalizeSentence), AppResources.GetName(Model.Name), AppResources.LoginNameDefaultEntry, 20, false, out _nameEntry, 140, 10));
			nameSurnameLayout.Children.Add (GetEntry (Keyboard.Create(KeyboardFlags.CapitalizeSentence), AppResources.GetSurname(Model.Name), AppResources.LoginSurnameDefaultEntry, 15, false, out _surnameEntry, 140, 10));

			topLayout.Children.Add (nameSurnameLayout);

			layout.Children.Add (topLayout);

			layout.Children.Add (GetEntry (Keyboard.Email, Model.Email, AppResources.LoginEmailDefaultEntry, 10, false, out _emailEntry));

			if(isPasswordEnabled)
				layout.Children.Add (GetEntry (Keyboard.Text, Model.Password, AppResources.LoginPasswordDefaultEntry, 10, true, out _passwordEntry));

			layout.Children.Add (GetEntry (Keyboard.Create(KeyboardFlags.CapitalizeSentence), Model.Job, AppResources.LoginJobDefaultEntry, 15, false, out _jobEntry));
			layout.Children.Add (GetEntry (Keyboard.Telephone, Model.Phone, AppResources.LoginPhoneDefaultEntry, 10, false, out _phoneEntry));

			var applyBtn = new Button {
				BorderRadius = 5,
				WidthRequest = AppProvider.Screen.Width,
				FontSize = 14,
				TextColor = Color.White,
				BackgroundColor = AppResources.LoginButtonColor,
				Text = buttonName
			};
			applyBtn.Clicked += OnApplyClicked;
			layout.Children.Add (new ContentView {Padding = new Thickness(LeftBorder, 10, LeftBorder, 0), HorizontalOptions = LayoutOptions.Center, Content = applyBtn});

			var fs = new FormattedString ();

			string firstPart = "Ao clicar em cadastrar, você está de acordo com os";
			string secondPart = " Termos de Uso ";
			string thirdPart = "e a";
			string fourthPart = " Política de Privacidade";

			fs.Spans.Add (new Span { Text = firstPart, ForegroundColor = Color.Black, FontSize = 14});
			fs.Spans.Add (new Span { Text= secondPart, ForegroundColor = Color.Blue, FontSize = 14});
			fs.Spans.Add (new Span { Text= thirdPart, ForegroundColor = Color.Black, FontSize = 14 });
			fs.Spans.Add (new Span { Text= fourthPart, ForegroundColor = Color.Blue, FontSize = 14});

			var labelTerms = new Label {
				FormattedText = fs
			};
			TapGestureRecognizer tap = new TapGestureRecognizer ();
			tap.Tapped += OnClicked;
			labelTerms.GestureRecognizers.Add (tap);

			layout.Children.Add (new ContentView { Content = labelTerms, Padding = new Thickness(10)});

			Content = new ScrollView {Content = layout};

			_requiredFields = new List<InputFieldView> {
				_jobEntry,
				_emailEntry,
				_nameEntry,
				_passwordEntry,
				_phoneEntry,
				_surnameEntry
			};
		}

		void OnClicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new TermsPage ());
		}


		void OnImageClicked (object sender, System.EventArgs e)
		{
			AppController.Instance.AddImage (_fakeProfileImagePath, OnImageChanged, 100);
		}

		private void OnImageChanged()
		{
			Device.BeginInvokeOnMainThread (() => {
				_isProfileImageChanged = true;
				_profileImage.Source = ImageLoader.Instance.GetImage(_fakeProfileImagePath, false);
			});
		}

		private void OnApplyClicked (object sender, EventArgs e)
		{
			if (IsRequiredFieldsFilled (_requiredFields)) 
			{
				if (AppModel.Instance.IsEmail (_emailEntry.Text)) {
					if (Apply != null) {
						Model.Name = string.Format ("{0} {1}", _nameEntry.Text, _surnameEntry.Text);
						Model.Email = _emailEntry.Text;
						Model.Job = _jobEntry.Text;
						Model.Phone = _phoneEntry.Text;
						Model.Email = _emailEntry.Text;
						Model.ProfileImage = _isProfileImageChanged ? _fakeProfileImagePath : Model.ProfileImage;
						if (_passwordEntry != null)
							Model.Password = _passwordEntry.Text;

						Apply (Model);
					}
				} else {
					AppProvider.PopUpFactory.ShowMessage (AppResources.SignUpWrongEmailFormat, AppResources.Error);
				}
			} 
			else 
			{
				AppProvider.PopUpFactory.ShowMessage (AppResources.SignUpRequiredFields, AppResources.Warning);
				MarkEmptyFields (_requiredFields);
			}
		}

		private bool CheckIsEntryEmpty(InputFieldView entry)
		{
			return string.IsNullOrEmpty(entry.Text);
		}

		private bool IsRequiredFieldsFilled(List<InputFieldView> required)
		{
			foreach (InputFieldView entry in required) 
			{
				if(entry != null)
				{
					if (CheckIsEntryEmpty (entry)) 
						return false;
				}
			}

			return true;
		}

		private void MarkEmptyFields(List<InputFieldView> required)
		{
			foreach (InputFieldView entry in required) 
			{
				if (entry != null)
				{
					if (CheckIsEntryEmpty (entry))
						entry.BackgroundColor = AppResources.AgendaCongressoColor.MultiplyAlpha (0.5f);
					else
						entry.BackgroundColor = Color.Transparent;
				}
			}
		}


		private View GetEntry(Keyboard keyBoard, string currentValue, string placeholdertValue, int topPadding, bool isPassword, out InputFieldView textEntry, float addionalBorder = 0, float leftBorder =  20)
		{
			textEntry = new InputFieldView (new InputFieldView.Parameters {
				Keyboard = keyBoard,
				CurrentValue = currentValue,
				PlaceholderValue = placeholdertValue,
				TopPadding = topPadding,
				IsPassword = isPassword,
				AddionalBorder = addionalBorder,
				LeftBorder = leftBorder
			});
			return textEntry;
		}
	}

}