using Conarh_2016.Application;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Connect
{
    public class HowToPlayPage : CarouselPage
    {

        public event Action ClosePage;

        private List<ContentPage> pages;

        public HowToPlayPage()
        {
            //this.Title = AppResources.HowToPlayPageTitle;
            this.Title = "";

            pages = new List<ContentPage>(0);

            this.addH2PCarouselPage(new HowToPlayCarouselPage(AppResources.HowToPlayCarouselPageImage1, true, false));
            this.addH2PCarouselPage(new HowToPlayCarouselPage(AppResources.HowToPlayCarouselPageImage2, false, false));
            this.addH2PCarouselPage(new HowToPlayCarouselPage(AppResources.HowToPlayCarouselPageImage3, false, false));
            this.addH2PCarouselPage(new HowToPlayCarouselPage(AppResources.HowToPlayCarouselPageImage4, false, true));
                      
        }

        private void addH2PCarouselPage(HowToPlayCarouselPage cPage)
        {
            cPage.ClosePage += Closepage;
            cPage.PrevPage += Prevpage;
            cPage.NextPage += Nextpage;
            this.Children.Add(cPage);
        }

        private void Closepage()
        {
            if (ClosePage != null)
                ClosePage();
        }

        private void Prevpage()
        {
            //TODO - implement this
            //this.CurrentPage.
            //Carrossel.nexr
            //Navigation.PopModalAsync();
        }

        private void Nextpage()
        {
            //TODO - implement this
            //Navigation.PopModalAsync();
        }

    }
}
