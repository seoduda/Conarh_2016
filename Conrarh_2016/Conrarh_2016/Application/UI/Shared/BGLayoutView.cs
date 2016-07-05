using Conarh_2016.Application;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;

/**
 * Envolve o layout com BG e banner
 */

namespace Conrarh_2016.Application.UI.Shared
{
    internal class BGLayoutView : RelativeLayout
    {
        private double posY;
        private readonly double bannerHeight = 50;
        private Image _backgroundImage;
        
        public BGLayoutView(String backgroundImage, Layout layout, bool showBanner, bool hasNavigationBar) : base()
        {
            WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width);
            posY = hasNavigationBar ? 0 : 0;
            _backgroundImage = new Image()
            {
                Source = ImageLoader.Instance.GetImage(backgroundImage, false),
                Aspect = Aspect.Fill
            };

            Children.Add(_backgroundImage,
            Constraint.Constant(0),
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => { return parent.Width; }),
            Constraint.RelativeToParent((parent) => { return (parent.Height); }));


            if (showBanner)
            {
                Children.Add(layout,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height - bannerHeight; }));

                Children.Add(new SponsorBannerView(),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return parent.Height - bannerHeight; }),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.Constant(bannerHeight)
                    );
            }else
            {
                Children.Add(layout,
                  Constraint.Constant(0),
                  Constraint.Constant(0),
                  Constraint.RelativeToParent((parent) => { return parent.Width; }),
                  Constraint.RelativeToParent((parent) => { return parent.Height; }));

            }
        }
    }
}