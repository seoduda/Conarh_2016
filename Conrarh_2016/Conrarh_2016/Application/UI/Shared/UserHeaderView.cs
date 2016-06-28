using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Shared
{
    public sealed class UserHeaderView : ContentView
    {
        private const int ImageHeight = 60;

        public readonly UserModel Model;

        public event Action EditUserProfile;

        public UserHeaderView(UserModel userModel, bool showContacts = false)
        {
            Model = userModel;

            var stackLayout = new StackLayout
            {
                BackgroundColor = AppResources.UserBackgroundColor,
                Orientation = StackOrientation.Vertical
            };

            var tappableHeaderLayout = new StackLayout { Orientation = StackOrientation.Vertical };

            var topLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

            var photoLayout = new AbsoluteLayout { };
            photoLayout.Children.Add(new BoxView
            {
                WidthRequest = 10,
                HeightRequest = ImageHeight,
                Color = AppResources.AgendaExpoColor
            });

            var userImage = new DownloadedImage(AppResources.DefaultUserImage)
            {
                HeightRequest = ImageHeight,
                WidthRequest = ImageHeight,
                Aspect = Aspect.AspectFill
            };
            userImage.SetBinding(DownloadedImage.UpdateAtTimeProperty, UpdatedUniqueItem.UpdatedAtTimePropertyName);
            userImage.SetBinding(DownloadedImage.ServerImagePathProperty, User.ProfileImagePathPropertyName);
            photoLayout.Children.Add(userImage, new Point(10, 0));

            topLayout.Children.Add(photoLayout);

            var namesLayout = new StackLayout { Orientation = StackOrientation.Vertical, Padding = new Thickness(10, 10) };
            var nameLabel = new Label { TextColor = Color.White, FontSize = 17 };
            nameLabel.SetBinding(Label.TextProperty, User.NamePropertyName);

            var jobLabel = new Label { TextColor = Color.White, FontSize = 13 };
            jobLabel.SetBinding(Label.TextProperty, User.JobPropertyName);

            namesLayout.Children.Add(nameLabel);
            namesLayout.Children.Add(jobLabel);

            topLayout.Children.Add(namesLayout);
            tappableHeaderLayout.Children.Add(topLayout);

            if (showContacts)
            {
                var middleLayout = new StackLayout { Orientation = StackOrientation.Vertical, Padding = new Thickness(10, 10) };
                var contactsHeaderLabel = new Label
                {
                    TextColor = AppResources.ProfileContactsHeaderColor,
                    Text = AppResources.ProfileContactsHeader,
                    FontSize = 14
                };
                middleLayout.Children.Add(contactsHeaderLabel);

                var userEmailLabel = new Label { TextColor = Color.White, FontSize = 13 };
                userEmailLabel.SetBinding(Label.TextProperty, User.EmailPropertyName);
                middleLayout.Children.Add(userEmailLabel);

                var userCellphoneLabel = new Label { TextColor = Color.White, FontSize = 16 };
                userCellphoneLabel.SetBinding(Label.TextProperty, User.PhonePropertyName);
                middleLayout.Children.Add(userCellphoneLabel);

                tappableHeaderLayout.Children.Add(middleLayout);
            }

            TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnEditUserTapped;

            tappableHeaderLayout.GestureRecognizers.Add(tapRecognizer);

            stackLayout.Children.Add(tappableHeaderLayout);

            stackLayout.Children.Add(new BoxView
            {
                WidthRequest = AppProvider.Screen.Width,
                HeightRequest = 1,
                Color = Color.Gray
            });

            var pointsLayout = new StackLayout
            {
                Padding = new Thickness(30, 5),
                Spacing = 20,
                Orientation = StackOrientation.Horizontal
            };

            var pointsCountLabel = new Label
            {
                TextColor = AppResources.ProfilePointsColor,
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                YAlign = TextAlignment.Center
            };
            pointsCountLabel.SetBinding(Label.TextProperty, User.ScorePointsPropertyName);

            pointsLayout.Children.Add(pointsCountLabel);

            var pointsImage = new Image { HeightRequest = 70 };
            pointsImage.SetBinding(Image.SourceProperty, User.LevelImagePathPropertyName);
            pointsLayout.Children.Add(pointsImage);

            stackLayout.Children.Add(pointsLayout);

            stackLayout.Children.Add(new BoxView
            {
                WidthRequest = AppProvider.Screen.Width,
                HeightRequest = 1,
                Color = Color.Gray
            });

            Content = stackLayout;

            BindingContext = Model.User;
            Model.IsChanged += OnUserModelChanged;
        }

        private void OnUserModelChanged()
        {
            Device.BeginInvokeOnMainThread(() => BindingContext = Model.User);
        }

        private void OnEditUserTapped(object sender, EventArgs e)
        {
            if (EditUserProfile != null)
                EditUserProfile();
        }
    }
}