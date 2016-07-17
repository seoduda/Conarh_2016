using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Shared
{
    public sealed class UserHeaderView : ContentView
    {
        private const int ImageHeight = 80;

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


            /**
            Tirei o click do header inteiro e passei para o btEdit
            var tappableHeaderLayout = new StackLayout { Orientation = StackOrientation.Vertical };
            */

            var headerLayout = new StackLayout { Orientation = StackOrientation.Vertical };

            var topLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Padding = new  Thickness (20, 5, 5, 0) };


            var photoLayout = new AbsoluteLayout { };
            photoLayout.Children.Add(new BoxView
            {
                WidthRequest = 10,
                HeightRequest = ImageHeight,
                Color = AppResources.MenuColor
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
            var nameLabel = new Label { TextColor = AppResources.UserHeaderNameTextColor, FontSize = 17 };
            nameLabel.SetBinding(Label.TextProperty, User.NamePropertyName);

            var jobLabel = new Label { TextColor = AppResources.UserHeaderJobTextColor, FontSize = 14 };
            jobLabel.SetBinding(Label.TextProperty, User.JobPropertyName);

            namesLayout.Children.Add(nameLabel);
            namesLayout.Children.Add(jobLabel);

            topLayout.Children.Add(namesLayout);
            Image btEdit = new Image()
            {
                Source = ImageLoader.Instance.GetImage(AppResources.DefaultBtEdit, false),
                HeightRequest = ImageHeight/2,
                WidthRequest = ImageHeight/2,
            };

            TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnEditUserTapped;

            btEdit.GestureRecognizers.Add(tapRecognizer);



            topLayout.Children.Add(btEdit);

            headerLayout.Children.Add(topLayout);

            if (showContacts)
            {
                var middleLayout = new StackLayout { Orientation = StackOrientation.Vertical, Padding = new Thickness(10, 10) };
                var contactsHeaderLabel = new Label
                {
                    //TextColor = AppResources.ProfileContactsHeaderColor,
                    TextColor = Color.Purple,
                    Text = AppResources.ProfileContactsHeader,
                    FontSize = 14
                };
                middleLayout.Children.Add(contactsHeaderLabel);

                var userEmailLabel = new Label { TextColor = AppResources.UserHeaderNameTextColor, FontSize = 14 };
                userEmailLabel.SetBinding(Label.TextProperty, User.EmailPropertyName);
                middleLayout.Children.Add(userEmailLabel);

                var userCellphoneLabel = new Label { TextColor = AppResources.UserHeaderNameTextColor, FontSize = 14 };
                userCellphoneLabel.SetBinding(Label.TextProperty, User.PhonePropertyName);
                middleLayout.Children.Add(userCellphoneLabel);

                headerLayout.Children.Add(middleLayout);
            }

            stackLayout.Children.Add(headerLayout);


            /* Implementação com Progressbar */

            var pointsLayout = new AbsoluteLayout
            {
                Padding = new Thickness(10, 5),
            };
            int pgImagelabelHeigth = 69;
            int pointsCountBoxHeigth = 51;
            int boxYpos = pgImagelabelHeigth - (pointsCountBoxHeigth / 3);

            var pointsCountLabel = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = AppResources.ProfilePointsColor,
                FontSize = 18,
                //FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                HeightRequest = pointsCountBoxHeigth - 12,
                WidthRequest = pointsCountBoxHeigth - 2,
            };
            
            //pointsCountLabel.SetBinding(Label.TextProperty, User.ScorePointsPropertyName);
            pointsCountLabel.SetBinding(Label.TextProperty, User.ScorePointsProperty);

            var pointsCountText = new Label
            {
                Text = AppResources.ProfilePointsLabelText,
                TextColor = AppResources.ProfilePointsColor,
                FontSize = 9,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                //HeightRequest = 10,
                WidthRequest = pointsCountBoxHeigth - 2,
            };

            var pointsCountBox = new Button
            {
                BackgroundColor = AppResources.UserHeaderNameTextColor,
                IsEnabled = false,
                BorderRadius = 25,
                HeightRequest = pointsCountBoxHeigth,
                WidthRequest = pointsCountBoxHeigth
            };
            pointsLayout.Children.Add(pointsCountBox, new Point(0, boxYpos));
            pointsLayout.Children.Add(pointsCountLabel, new Point(2, boxYpos + 8));
            pointsLayout.Children.Add(pointsCountText, new Point(2, boxYpos + 30));

            var progbar = new ProgressBar()
            {
                HeightRequest = 17,
                //BackgroundColor = AppResources.ProfilePointsColor,
                WidthRequest = 300,
                Progress = AppModel.Instance.CurrentUser.User.ScorePointsProgression
            };

            Image progbarImageLabel = new Image()
            {
                Source = ImageLoader.Instance.GetImage(AppResources.ProfileLevelProgressBarImageLabel, false),
                HeightRequest = pgImagelabelHeigth,
                WidthRequest = 300,

            };

            pointsLayout.Children.Add(progbar, new Point(pointsCountBoxHeigth, pgImagelabelHeigth));
            pointsLayout.Children.Add(progbarImageLabel, new Point(pointsCountBoxHeigth, 0));
            // progbar.SetBinding(ProgressBar.ProgressProperty, User.ScorePointsProgressionProperty);

            stackLayout.Children.Add(pointsLayout);

/*
            stackLayout.Children.Add(new BoxView
            {
                WidthRequest = AppProvider.Screen.Width,
                HeightRequest = 1,
                Color = Color.Gray
            });

    */

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