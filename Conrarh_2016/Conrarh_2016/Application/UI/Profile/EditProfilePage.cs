using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using Conrarh_2016.Application.UI.Shared;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Profile
{
    public sealed class EditProfilePage : ContentPage
    {
        public event Action<CreateUserData> SaveProfileChanges;

        public readonly User Model;
        public readonly string ImagePath;

        public EditProfilePage(User currentUser)
        {
            Title = AppResources.EditProfile;
            Model = currentUser;
            ImagePath = null;
            if (!string.IsNullOrEmpty(Model.ProfileImagePath))
            {
                ImageData imageData = AppModel.Instance.Images.Items.Find(temp => temp.ServerPath.Equals(Model.ProfileImagePath));
                ImagePath = imageData != null ? imageData.ImagePath : null;
            }

            CreateUserData data = new CreateUserData
            {
                Name = currentUser.Name,
                Email = currentUser.Email,
                Password = null,
                UserType = currentUser.Id,
                Job = currentUser.Job,
                Phone = currentUser.Phone,
                ProfileImage = ImagePath
            };
            FillUserDataView fillData = new FillUserDataView(data, AppResources.SaveProfileButton, false);
            fillData.Apply += OnSaveUserProfile;
            var layout = new ContentView { Content = fillData, Padding = new Thickness(0, 10, 0, 0) };

            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, false, true);
            //Content = new ScrollView { Content = layout };
            //Content = new ScrollView { Content = bgLayout };

            Content = bgLayout;
        }

        private void OnSaveUserProfile(CreateUserData data)
        {
            if (SaveProfileChanges != null)
            {
                data.Name = Model.Name.Equals(data.Name) ? data.Name : data.Name;
                data.Email = Model.Email.Equals(data.Email) ? data.Email : data.Email;
                //data.Password = Model.Passphrase;
                data.UserType = Model.UserType;
                data.Job = Model.Job.Equals(data.Job) ? data.Job : data.Job;
                data.ProfileImage = ImagePath != null && ImagePath.Equals(data.ProfileImage) ? data.ProfileImage : data.ProfileImage;
                if (Model.Phone != null)
                {
                    if (String.IsNullOrEmpty(Model.Phone))
                    {
                        data.Phone = " ";
                    }
                    else
                    {
                        data.Phone = Model.Phone.Equals(data.Phone) ? data.Phone : data.Phone;
                    }
                }
                data.ScorePoints = Model.ScorePoints;

                /* TODO Validar se não mudou nada EditProfilePage */
                SaveProfileChanges(data);
                /*
                if (data.Name == Model.Name &&
                    data.Email == Model.Email &&
                    data.Job == Model.Job &&
                    data.ProfileImage == Model.p &&
                    data.Phone == null &&
                    data.ScorePoints == null)
                {
                    AppProvider.PopUpFactory.ShowMessage(AppResources.EditProfileNoChanges, string.Empty);
                }
                else
                {
                    SaveProfileChanges(data);
                }
                */
            }
        }
    }
}