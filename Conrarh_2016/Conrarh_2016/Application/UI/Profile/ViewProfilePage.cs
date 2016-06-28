using Xamarin.Forms;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ViewProfilePage : ContentPage
	{
		public readonly UserModel Model;

		public ViewProfilePage (UserModel model)
		{
			Model = model;
			Title = Model.User.Name;

			ProfileContentView.Parameters parameters = new ProfileContentView.Parameters {
				UserModel = Model
			};
			Content = new ProfileContentView(parameters);

			UserController.Instance.UpdateProfileData (Model, false);
		}
	}
}