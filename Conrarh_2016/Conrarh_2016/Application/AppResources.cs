using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Conarh_2016.Application
{

    public enum AppUserType
    {
        Common = 0,
        Speecher = 1,
        Stand = 2,
        Promoter = 3
    }

    public enum AppBadgeType
    {
        ShareApp = 0, //559d1875657f6f052ffd70fa
        WallPost = 1, //559d1576657f6f052ffd70e3
        ConnectTo3Users = 2, //559d13dd657f6f052ffd70de
        ConnectToUncommonUser = 3, //559d136d657f6f052ffd70ce
        ConnectTo10Users = 4, //557746be657f6f66f9ee8a78
        ConnectTo50Users = 5, //557746b6657f6f66f9ee8a75
    }


    class AppResources
    {
        public static string AlreadySentRequestError = "Você já enviou o pedido. Aguarde!";
        public static string AlreadySentRequestToError = "O usuário já lhe enviou um convite. Aguarde o pedido chegar!";
        public static string ContactListIsEmpty = "Você ainda não possui contatos!";

        public static string Agenda = "Eventos";
        public static string AgendaCongresso = "Agenda Congresso";
        public static string AgendaExpo = "Agenda Expo";
        public static string Exhibitors = "Expositores";
        public static string Map = "Mapa";
        public static string Wall = "Mural";
        public static string Connect = "Conecte-se!";
        public static string Profile = "Perfil";
        public static string Congresso = "Congresso";
        public static string Expo = "Expo";
        public static string Speakers = "Palestrantes";
        public static string EventsActionBtnHeader = "Interaja com a palestra";
        public static string EventDetailsHeader = "Detalhes do Evento";
        public static string BioHeader = "BIO";
        public static string EventsActionHeader = "Curtiu ou não curtiu?";
        public static string EventsQuestionHeader = "Faça uma pergunta ao vivo";
        public static string EventsQuestionDefaultAnswer = "Faça sua pergunta aqui.";
        public static string EventsActionPostQuestion = "Enviar";
        public static string EventsActionConteudo = "Conteúdo";
        public static string EventsActionAplicabilidade = "Aplicabilidade";
        public static string LoginSkip = "Pular Login";
        public static string LoginForgetPassword = "Esqueceu a senha?";
        public static string LoginForgetPasswordEnterEmail = "Digite seu email cadastrado";
        public static string LoginCreateUser = "Cadastrar";
        public static string LoginEmailDefaultEntry = "Email";
        public static string LoginPasswordDefaultEntry = "Senha";
        public static string Login = "Login";
        public static string LogOut = "Logout";
        public static string LoginJobDefaultEntry = "Cargo";
        public static string LoginPhoneDefaultEntry = "Celular";
        public static string LoginNameDefaultEntry = "Nome";
        public static string LoginSurnameDefaultEntry = "Sobrenome";
        public static string HowToPlay = "Como Jogar";
        public static string HowToPlayPageTitle = "Como Jogar";

       

        public static string LoginFirstMessage = "Você precisa se logar primeiro!";
        public static string Warning = "Aviso!";
        public static string LoadingCreatingUser = "Criando usuário…";
        public static string LoadingLoginUser = "Login...";
        public static string LoadingLogoutUser = "Logout...";
        public static string LoadingSendingQuestion = "Enviando pergunta…";
        public static string LoadingEventsData = "Carregando a Agenda…";
        public static string SignUpRequiredFields = "Você precisa preencher todos os campos obrigatórios para cadastrar.";
        public static string LoadingSendingVote = "Enviando voto…";
        public static string FailedServer = "Ops! Aconteceu algum erro inesperado. Tente novamente mais tarde!";
        public static string SuccessfulPostMessage = "Mensagem enviada!";
        public static string SuccessfulPostVote = "Seu voto foi computado!";
        public static string Error = "Erro!";
        public static string WallLikes = "Likes";
        public static string LoadingSendingLike = "Enviando like…";
        public static string YesButtonName = "Sim";
        public static string NoButtonName = "Não";
        public static string PostOnWall = "Publicar no Mural";
        public static string WallCreatePost = "Publicar no Mural";
        public static string WallPostButtonPost = "Publicar";
        public static string WallPostButtonUpload = "Adicionar foto";
        public static string LoadingSendingWallPost = "Publicando no Mural…";
        public static string SuccessfulCreateWallPost = "Publicado!";
        public static string AlreadyLikePostMessage = "Você já curtiu esta publicação!";
        public static string WallPostAddSomeTextMessage = "Você precisa adicionar algum texto na publicação.";
        public static string LikePostMessageQuestion = "Deseja curtir esta publicação?";
        public static string LoadingSearchExhibitors = "Procurando Expositores…";
        public static string ExhibitorsSearchText = "Busca";
        public static string SignUpChooseImage = "Editar";
        public static string LoadingFavouriteActions = "Atualizando…";
        public static string LoadingUpdateEventVotes = "Atualizando…";
        public static string ProfileContactsHeader = "Meus contatos";
        public static string SaveProfileButton = "Salvar";
        public static string EditProfile = "Editar perfil";
        //public static string ConnectRequestNotSent = "Conecte-se";
        public static string ConnectRequestNotSent = "Conectar";
        public static string ConnectRequestSent = "Enviado";
        public static string ConnectAcceptRequest = "Aceitar";
        public static string LoadingSendingRequest = "Enviando solicitação…";
        public static string LoadingSendingAcceptRequest = "Enviando…";
        public static string SuccessfulRequestConnection = "Solicitação enviada!";
        public static string SuccessfulAcceptRequestConnection = "Contato adicionado!";
        public static string ProfileRatingBtnHeader = "Ranking";
        public static string ProfileContactListBtnHeader = "Contatos";
        public static string LoadingRanking = "Carregando o Ranking…";
        public static string LoadingHistory = "Atualizando dados…";
        public static string ContactList = "Contatos";
        public static string LoadingResetPassword = "Enviando…";
        public static string SuccessfulResetPassword = "Enviado! Verifique seu email.";
        public static string ResetPasswordIsEmpty = "Email inválido!";
        public static string RequestEnterPassphrase = "Digite a palavra-chave para conectar";
        public static string RequestEnterPassphraseDefault = "Palavra-chave";
        public static string RequestEnterPassphraseWrong = "Palavra-chave está errada!";
        public static string ProfileHistoryItemHeaderFormat = "Conectou-se com {0}";
        public static string ProfileHistoryItemPointsFormat = "pontos";
        public static string ProfilePointsLabelText = "pontos";
        public static string LoadingWallPostLikes = "Carregando lista de likes…";
        public static string ProfileHistoryHeader = "Histórico de pontuação";
        public static string ProfileHistoryClickHeader = "Ir para Histórico";
        public static string LoadingSavingProfileChanges = "Atualizando perfil…";
        public static string EditProfileNoChanges = "Nenhuma alteração realizada!";
        public static string SignUpWrongEmailFormat = "Email inválido!";
        public static string SignUpWrongPhoneFormat = "Telefone inválido!";
        public static string UploadImageFromGallery = "Escolher da Galeria";
        public static string TakePicture = "Tirar uma Foto";
        public static string Cancel = "Cancelar";

        public static string Facebook = "Facebook";
        public static string Twitter = "Twitter";

        public static string SuccessfullFavAdd = "Favorito Adicionado!";

        public static string LoadingPushNotification = "Carregando o Push Notifications…";

        public static Color DefaultLikeColor = Color.FromHex("bfa06e");
        public static Color SeparatorColor = Color.FromHex("567675");

        public static Color LoginBackgroundColor = Color.FromHex("abbbba");
        public static Color LoginActiveTextColor = Color.Black;
        public static Color LoginNormalTextColor = Color.FromHex("2b3b3b");
        public static Color LoginButtonColor = Color.FromHex("567675");
        public static Color LoginBottomButtonColor = Color.FromHex("f4d657");
        public static Color LoginBottomButtonTextColor = Color.FromHex("ca9e67");
        public static Color LoginSkipButtonColor = Color.FromHex("3597d3");

        public static Color MenuColor = Color.FromHex("567675");
        public static Color MenuTitleTextColor = Color.FromHex("B0D7E3");

        public static Color Yellow = Color.FromHex("eec22e");
        public static Color Green = Color.FromHex("009933");

        public static Color LikedItemTextColor = Color.FromHex("567675");

        public static Color EventActionsTextColor = Color.FromHex("567675");
        public static Color EventActionsQuestionsBlockColor = Color.FromHex("B0D5E1");
        public static Color EventActionsQuestionsBlockTitleColor = Color.FromHex("4398B6");
        public static Color EventActionsQuestionsBlockTitleTextColor = Color.White;
        public static Color EventActionsQuestionsBlockBtnBgColor = Color.FromHex("567675");



        public static Color AgendaPageBackgroundColor = Color.White;
        public static Color AgendaExpoColor = Color.FromHex("AAB9B8");
        public static Color AgendaCongressoColor = Color.FromHex("B8CA4F");
        public static Color AgendaDataBGColor = Color.FromHex("E2EAB7");

        public static Color SpeecherBgColor = Color.Transparent;
        public static Color SpeecherTextColor = Color.FromHex("567675");

        public static Color ExhibitorNameTextColor = Color.FromHex("95a5a6");
        public static Color ExhibitorDescTextColor = Color.FromHex("0079c2");

        public static Color WallPostCreatedDateColor = Color.FromHex("d2b583");
        public static Color WallPostBackgroundColor = Color.FromHex("f7f0db");
        public static Color WallPageBackgroundColor = Color.Transparent;

        public static Color ExhibitorsSearchBarColor = Color.FromHex("e9d9a2");
        public static Color ExhibitorsSearchBarTextColor = Color.FromHex("d2b582");

        public static Color SignUpChooseImageButtonColor = Color.FromHex("AAB9B8");
        public static Color SignUpBgColor = Color.Transparent;

        public static Color UserBackgroundColor = Color.Transparent;
        public static Color UserHeaderNameTextColor = Color.FromHex("567675");
        public static Color UserHeaderJobTextColor = Color.FromHex("819894");


        //public static Color UserHeaderJobTextColor = Color.FromHex("819894");



        public static Color ProfileContactsHeaderColor = Color.White;
        public static Color ProfilePendingContactsHeaderColor = Color.FromHex("567675");
        public static Color ProfileRankingHeaderColor = Color.FromHex("AAB9B8"); 
        public static Color ProfileContactsTextColor = Color.FromHex("567675");



        public static Color ProfilePointsColor = Color.White;
        public static Color ConnectRequestNotSentColor = Color.FromHex("e9daa2");

        public static Color ConnectRequestNotSentColorText = Color.FromHex("B99F5B");

        public static Color ConnectAcceptRequestColor = Color.FromHex("5ea96e");
        public static Color ConnectRequestSentColor = Color.FromHex("f08222");
        public static Color ProfileRatingBtnColor = Color.FromHex("e9daa2");
        public static Color ProfileContactListBtnColor = Color.FromHex("f08222");
        public static Color ProfileRankingPointsColor = Color.FromHex("f1c40d");
        public static Color ProfileBadgeBackgroundColor = Color.FromHex("f0e5d4");
        public static Color ProfileHistoryItemLightColor = Color.FromHex("d2b582");
        public static Color ProfileHistoryItemDarkColor = Color.FromHex("ca9e67");

        public static string DefaultUserImage = "defaultUserIcon.png";
        public static string DefaultEventImage = "defaultEventImage.png";
        public static string DefaultPostImage = "defaultPostImage.png";
        public static string DefaultExhibitorImage = "defaultExhibitorImage.png";
        public static string DefaultSponsorImage = "defaultSponsorImage.png";
        public static string DefaultPointsImage = "defaultPointsImage.png";
        public static string DefaultBgImage = "defaultBgImage.png";

        public static string CloseIcon = "icon_close.png";
        public static string LeftIcon = "icon_left.png";
        public static string RightIcon = "icon_right.png";


        public static string EventButtonActionImage = "eventButtonActionImage.png";
        public static string EventClockImage = "eventClockImage.png";
        public static string EventFavouriteImage = "eventFavouriteImage.png";
        public static string EventNoFavouriteImage = "eventNoFavouriteImage.png";
        public static string EventLikeActiveImage = "eventLikeActiveImage.png";
        public static string EventLikeNormalImage = "eventLikeNormalImage.png";
        public static string EventDislikeActiveImage = "eventDislikeActiveImage.png";
        public static string EventDislikeNormalImage = "eventDislikeNormalImage.png";

        public static string InteractBgImage = "InteractBgImage.png";


        public static string LoginBgImage = "defaultBgImage.png";
        public static string LoginBtLinkedin = "login_bt_linkedin";
        

        public static string LoginEmailImage = "loginEmailImage.png";
        public static string LoginPasswordImage = "loginPasswordImage.png";

        public static string HowToPlayCarouselPageImage1 = "carousel_1.png";
        public static string HowToPlayCarouselPageImage2 = "carousel_2.png";
        public static string HowToPlayCarouselPageImage3 = "carousel_3.png";
        public static string HowToPlayCarouselPageImage4 = "carousel_4.png";

        
        public static string ProfileLevelProgressBarImageLabel = "levelProgressBarImageLabel.png";
        public static string DefaultBtEdit = "editBtImage.png";


        public static string SignUpHeaderImage = "signUpHeaderImage.png";
        public static string SignUpBgImage = "signupBgImage.png";
        public static string SignUpButtonImage = "signUpButtonImage.png";
        
        public static string SettingsImage = "settings.png";

        public static string SponsorBanner = "healthways.png";
        public static string SponsorUri = "http://www.healthways.com.br/";

        public static string I9acaoLogo = "i9acao_logo.png";

        public static string WallPostLikeImage = "wallPostLikeImage.png";

        public static string MapImage = "eventmap.jpg";

        public static string ExhibitorsSearchImage = "exhibitorsSearchImage.png";

        public static string DefaultBadgeImage = "defaultBadgeImage.png";

        public static string ShareBtnImage = "shareBtn.png";

        public static string ShareLink = "https://www.facebook.com/ABRHNacional";
        public static string ShareMessage = "Curta o #conarh2016 !";
        public static string ShareTitle = "CONARH 2016";

        public static string ApiLinkedinClientId = "77c1rz71bj79xe";
        public static string ApiLinkedinClientSecret = "ixk8ykW2zSTqYLes";
        public static string ApiLinkedinScope= "r_basicprofile r_emailaddress";
        public static string ApiLinkedinAuthorizeUrl = "https://www.linkedin.com/uas/oauth2/authorization";
        public static string ApiLinkedinRedirectUrl = "http://www.i9acao.com.br/";
        public static string ApiLinkedinAccessTokenUrl = "https://www.linkedin.com/uas/oauth2/accessToken";

        public static ImageSource GetImageSource(int id)
        {
            return ImageLoader.Instance.GetImage(string.Format("event_{0}.png", id), true);
        }

        public static Dictionary<AppUserType, int> ScorePoints = new Dictionary<AppUserType, int> {
            { AppUserType.Promoter, 100}, // promoter
			{ AppUserType.Stand, 50}, // stand
			{ AppUserType.Common, 10}, // common
			{ AppUserType.Speecher, 10}, // speecher
		};

        public static Dictionary<string, AppUserType> UserTypes = new Dictionary<string, AppUserType> {
            { "5575ce4f657f6f66f9ee5715", AppUserType.Promoter}, // promoter
			{ "5575ce3a657f6f66f9ee5712", AppUserType.Stand}, // stand
			{ "5575ce28657f6f66f9ee570f", AppUserType.Common}, // common
			{ "5575ce22657f6f66f9ee570c", AppUserType.Speecher}, // speecher
		};

        public static Dictionary<string, AppBadgeType> BadgeTypes = new Dictionary<string, AppBadgeType> {
            { "578c5b08473a7545691bb87b", AppBadgeType.ShareApp},
            { "578c5b08ca583eb4094819ca", AppBadgeType.WallPost},
            { "578c5b09236eb67474c09025", AppBadgeType.ConnectTo3Users},
            { "578c5b09473a7545691bb87c", AppBadgeType.ConnectToUncommonUser},
            { "578c5b0a228ada731db736c4", AppBadgeType.ConnectTo10Users},
            { "578c5b0c22cd80e204d99edf", AppBadgeType.ConnectTo50Users},
        };

        public static string GetBadgeIdByType(AppBadgeType type)
        {
            foreach (string id in BadgeTypes.Keys)
            {
                if (BadgeTypes[id] == type)
                    return id;
            }
            return null;
        }

        public static Dictionary<AppBadgeType, int> BadgeActionCount = new Dictionary<AppBadgeType, int> {
            { AppBadgeType.ShareApp, 1},
            { AppBadgeType.WallPost, 1},
            { AppBadgeType.ConnectTo3Users, 3},
            { AppBadgeType.ConnectToUncommonUser, 1},
            { AppBadgeType.ConnectTo10Users, 10},
            { AppBadgeType.ConnectTo50Users, 50}
        };

        public static int GetPointsEarned(string userType)
        {
            if (UserTypes.ContainsKey(userType))
                return ScorePoints[UserTypes[userType]];
            return 0;
        }

        public static bool IsUserCommon(string userTypeId)
        {
            return UserTypes.ContainsKey(userTypeId) &&
                (UserTypes[userTypeId] == AppUserType.Common || UserTypes[userTypeId] == AppUserType.Speecher);
        }

        public static string GetLocalizedError(string msg)
        {
            if (msg.Equals("InvalidCredentials"))
                return "Usuário ou Senha incorretos";
            else if (msg.Equals("UserAlreadyExists"))
                return "O Usuário ja está em uso";

            return msg;
        }

        public static List<int> ScorePointsByLevel = new List<int> { 0, 199, 399, 399, 999 };

        public static string GetLevelImageByPoints(int points)
        {
            int level = 1;
            if (points < ScorePointsByLevel[ScorePointsByLevel.Count - 1])
            {
                for (int index = 0; index < ScorePointsByLevel.Count - 1; index++)
                {

                    if (points > ScorePointsByLevel[index] && points <= ScorePointsByLevel[index + 1])
                    {
                        level = index + 1;
                        break;
                    }
                }
            }
            else
                level = 5;

            return string.Format("level{0}.png", level);
        }

        public static string GetName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return null;

            if (fullName.IndexOf(" ") == -1)
                return fullName;

            return fullName.Substring(0, fullName.IndexOf(" "));
        }

        public static string GetSurname(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return null;

            if (fullName.IndexOf(" ") == -1)
                return string.Empty;

            return fullName.Substring(fullName.IndexOf(" ") + 1);
        }

        public static bool IsBadgeEnabled(string badgeId, int actionCount)
        {
            bool result = false;

            if (BadgeTypes.ContainsKey(badgeId))
            {
                if (BadgeActionCount.ContainsKey(BadgeTypes[badgeId]))
                    result = actionCount >= BadgeActionCount[BadgeTypes[badgeId]];
            }

            return result;
        }
    }
}



