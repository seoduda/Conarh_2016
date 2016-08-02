using Conarh_2016.Application.UI.Shared;
using System;

using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Connect
{
    public class HowToPlayCarouselPage : ContentPage
    {
        public event Action ClosePage;

        public event Action NextPage;

        public event Action PrevPage;

        //private Image iconNext;
        //private Image iconPrev;
        //private Image iconClose;
        private Image img;

        public HowToPlayCarouselPage(string _image, bool first, bool last) : base()
        {
            var layout = new StackLayout()
            {
                //Padding = new Thickness(0, 5, 5, 5),
                BackgroundColor = Color.White,
                Opacity = 0.6
            };
            var innerLayout = new RelativeLayout()
            {
            };
            img = new Image()
            {
                Source = _image,
                Aspect = Aspect.Fill
            };

            innerLayout.Children.Add(img,
            Constraint.Constant(0),
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => { return parent.Width; }),
            Constraint.RelativeToParent((parent) => { return (parent.Height); }));
            /*
            iconClose = new Image()
            {
                Source = AppResources.CloseIcon,
                
            };
            innerLayout.Children.Add(iconClose,
                Constraint.RelativeToParent((parent) => { return parent.Width - 35; }),
                Constraint.Constant(35),
                Constraint.Constant(25),
                Constraint.Constant(25));

            var closeIconLinkedInRecognizer = new TapGestureRecognizer();
            closeIconLinkedInRecognizer.Tapped += closeIcon_Tapped;

            iconClose.GestureRecognizers.Add(closeIconLinkedInRecognizer);
            */

          /*  
            if (!last)
            {
                iconNext = new Image()
                {
                    Source = AppResources.RightIcon,
                };

                innerLayout.Children.Add(iconNext,
                    Constraint.RelativeToParent((parent) => { return parent.Width - 30; }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height * 7) / 8; }),
                    Constraint.Constant(25),
                    Constraint.Constant(25));

                var iconNextLinkedInRecognizer = new TapGestureRecognizer();
                iconNextLinkedInRecognizer.Tapped += iconNext_Tapped;
                iconNext.GestureRecognizers.Add(iconNextLinkedInRecognizer);
            }
            if (!first)
            {
                iconPrev = new Image()
                {
                    Source = AppResources.LeftIcon,
                };

                innerLayout.Children.Add(iconPrev,
                    Constraint.Constant(5),
                    Constraint.RelativeToParent((parent) => { return (parent.Height * 7) / 8; }),
                    Constraint.Constant(25),
                    Constraint.Constant(25));

                var iconPrevLinkedInRecognizer = new TapGestureRecognizer();
                //Binding events
                iconPrevLinkedInRecognizer.Tapped += iconPrev_Tapped;
                //Associating tap events to the image buttons
                iconPrev.GestureRecognizers.Add(iconPrevLinkedInRecognizer);
            }
            */
            
            layout.Children.Add(innerLayout);
            Content = layout;
            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBadgeImage, layout, true, true);
            //Content = new ScrollView {Content = bgLayout };
            Content = new ContentView { Content = bgLayout };

        }

        private void iconPrev_Tapped(object sender, EventArgs e)
        {
            if (PrevPage != null)
                PrevPage();
        }

        private void iconNext_Tapped(object sender, EventArgs e)
        {
            if (NextPage != null)
                NextPage();
        }

        private void closeIcon_Tapped(object sender, EventArgs e)
        {
            if (ClosePage != null)
                ClosePage();
        }
    }
}