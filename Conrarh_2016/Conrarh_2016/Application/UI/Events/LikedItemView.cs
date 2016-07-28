using Conarh_2016.Application.BackgroundTasks.GetData;
using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Events
{
    public class LikedItem
    {
        public readonly UserVoteData VoteData;

        public LikedItem(UserVoteData voteData)
        {
            VoteData = voteData;
        }

        public int UI_NameFontSize;
        public int UI_ImageHeight;
        public int UI_TextYPosition;
        public int UI_LikeXPosition;
        public int UI_DislikeXPosition;

        public event Action IsChanged;

        public string Name
        {
            get
            {
                string s = VoteData.Subject;
                if (VoteData.UserVoteType == UserVoteType.Speaker)
                {
                    
                    if (AppModel.Instance.Speakers.Items.Count > 0)
                        return AppModel.Instance.Speakers.Find(VoteData.Subject).Name;
                    else
                    {
                        var downloadSpeakersTask = new DownloadSpeakersBackgroundTask();
                        List<Speaker> speakers = downloadSpeakersTask.Execute();
                        Speaker _speaker = speakers.Find(temp => temp.Id.Equals(VoteData.Subject));
                        AppModel.Instance.Speakers.UpdateData(speakers);
                        s = _speaker.Name;
                    }

                }
                return s;
            }
        }

        public string LikeImage
        {
            get { return VoteData.IsLike.HasValue && VoteData.IsLike.Value ? AppResources.EventLikeActiveImage : AppResources.EventLikeNormalImage; }
        }

        public string DislikeImage
        {
            get { return VoteData.IsLike.HasValue && !VoteData.IsLike.Value ? AppResources.EventDislikeActiveImage : AppResources.EventDislikeNormalImage; }
        }

        public void Update()
        {
            if (IsChanged != null)
                IsChanged();
        }
    }

    public sealed class LikedItemView : ContentView
    {
        public readonly LikedItem Data;
        private Image _likeImage;
        private Image _dislikeImage;

        public event Action<LikedItem, bool> Clicked;

        public LikedItemView(LikedItem item)
        {
            Data = item;
            Data.IsChanged += Data_IsChanged;
            var layout = new AbsoluteLayout { Padding = new Thickness(0, 5, 0, 5) };

            layout.Children.Add(new Label
            {
                Text = Data.Name,
                FontSize = Data.UI_NameFontSize,
                TextColor = AppResources.LikedItemTextColor
            }, new Point(40, Data.UI_TextYPosition));

            _likeImage = new Image
            {
                Source = ImageLoader.Instance.GetImage(Data.LikeImage, true),
                HeightRequest = Data.UI_ImageHeight
            };
            TapGestureRecognizer likeRecognizer = new TapGestureRecognizer();
            likeRecognizer.Tapped += OnLikeTapped;
            _likeImage.GestureRecognizers.Add(likeRecognizer);

            _dislikeImage = new Image
            {
                Source = ImageLoader.Instance.GetImage(Data.DislikeImage, true),
                HeightRequest = Data.UI_ImageHeight
            };
            TapGestureRecognizer dislikeRecognizer = new TapGestureRecognizer();
            dislikeRecognizer.Tapped += OnDislikeTapped;
            _dislikeImage.GestureRecognizers.Add(dislikeRecognizer);

            layout.Children.Add(_likeImage, new Point(Data.UI_LikeXPosition, 0));
            layout.Children.Add(_dislikeImage, new Point(Data.UI_DislikeXPosition, 0));

            Content = layout;
        }

        private void Data_IsChanged()
        {
            _likeImage.Source = ImageLoader.Instance.GetImage(Data.LikeImage, true);
            _dislikeImage.Source = ImageLoader.Instance.GetImage(Data.DislikeImage, true);
        }

        private void OnLikeTapped(object sender, EventArgs e)
        {
            RaiseClicked(true);
        }

        private void OnDislikeTapped(object sender, EventArgs e)
        {
            RaiseClicked(false);
        }

        private void RaiseClicked(bool isLike)
        {
            if (Clicked != null)
                Clicked(Data, isLike);
        }
    }
}