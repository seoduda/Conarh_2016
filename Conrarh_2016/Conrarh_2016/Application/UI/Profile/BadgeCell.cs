using Xamarin.Forms;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class BadgeCell: ContentView
	{
		private readonly DownloadedImage _badgeImage;
		private bool? _currentState;

		public readonly UserModel UserModel;

		public BadgeCell (UserModel userModel)
		{
			UserModel = userModel;

			_badgeImage = new DownloadedImage (AppResources.DefaultBadgeImage) {
				RequiredSize = 200,
				Aspect = Aspect.AspectFill,
				BackgroundColor = AppResources.ProfileBadgeBackgroundColor};
			
			Content = new ContentView {Content = _badgeImage};

			UserModel.BadgeActions.CollectionChanged += OnBadgesChanged;;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			OnBadgesChanged (null);
		}

		void OnBadgesChanged (List<BadgeAction> obj)
		{
			Device.BeginInvokeOnMainThread( () => {
				BadgeType badgeType = BindingContext as BadgeType;

				if(badgeType != null)
				{
					bool newState = UserModel.GetBadgeState(badgeType);

					if(!_currentState.HasValue || newState != _currentState.Value)
					{
						_currentState = newState;
						_badgeImage.UpdateAtTime = badgeType.UpdatedAtTime;
						_badgeImage.ServerImagePath = _currentState.Value ? badgeType.BadgeEnableImage : badgeType.BadgeDisableImage;
					}
				}
			});
		}
	}
}
