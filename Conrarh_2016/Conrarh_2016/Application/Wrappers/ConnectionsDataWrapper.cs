using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace Conarh_2016.Application.Wrappers
{
    public sealed class ConnectionsDataWrapper : DynamicObservableData<ConnectionModel>
    {
        public readonly UserModelsWrapper UsersWrapper;
        public readonly UserModel LoginedUser;

        public readonly Dictionary<string, ConnectionModel> Models = new Dictionary<string, ConnectionModel>();

        // search connectons data
        public DynamicListData<User> SearchUsers;

        public new void ClearData()
        {
            LoginedUser.Connections.CollectionChanged -= OnRequestsChanged;
            UsersWrapper.AddUsers -= OnUsersAddedChanged;

            if (SearchUsers != null)
                SearchUsers.CollectionChanged -= OnSearchCollectionChanged;
        }

        public ConnectionsDataWrapper(UserModel loginedUser, UserModelsWrapper usersWrapper) : base(true)
        {
            LoginedUser = loginedUser;
            UsersWrapper = usersWrapper;

            //refresh by data from db
            if (!UsersWrapper.UsersModels.IsEmpty())
            {
                OnUsersAddedChanged(UsersWrapper.UsersModels.Items);

                if (!LoginedUser.Connections.IsEmpty())
                    OnRequestsChanged(LoginedUser.Connections.Items);
            }

            LoginedUser.Connections.CollectionChanged += OnRequestsChanged;
            UsersWrapper.AddUsers += OnUsersAddedChanged;
        }

        private void OnRequestsChanged(List<ConnectRequest> requests)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (ConnectRequest request in requests)
                {
                    /*TODO Validar
					string connectedUserId = request.Requester.Id.Equals(LoginedUser.User.Id) ?
						request.Responder.Id : request.Requester.Id;
                        */
                    string connectedUserId = request.RequesterId.Equals(LoginedUser.User.Id) ?
                        request.ResponderId : request.RequesterId;

                    if (Models.ContainsKey(connectedUserId))
                        RefreshRequest(Models[connectedUserId], request);
                }
            });
        }

        private void RefreshRequest(ConnectionModel model, ConnectRequest request)
        {
            if (model != null && request != null)
                model.ApplyConnectRequest(request);
        }

        public ConnectionModel GetModel(User user, UserModel userModel)
        {
            bool isNewUser;
            userModel = userModel ?? UsersWrapper.UsersModels.Find(user.Id);
            if (userModel == null)
                userModel = UsersWrapper.AddModel(user, out isNewUser);

            ConnectionModel model;
            Models.TryGetValue(userModel.User.Id, out model);

            if (model == null)
            {
                model = new ConnectionModel(userModel);

                Models.Add(userModel.User.Id, model);
                InsertItem(0, model);

                UpdateWithRequest(model);
            }

            return model;
        }

        private void OnUsersAddedChanged(List<UserModel> addedModels)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (UserModel newUser in addedModels)
                {
                    if (!newUser.Id.Equals(LoginedUser.User.Id))
                    {
                        ConnectionModel model = GetModel(newUser.User, newUser);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, model));
                    }
                }
            });
        }

        private void UpdateWithRequest(ConnectionModel model)
        {
            /*TODO Validar
            var request = LoginedUser.Connections.Items.Find(
                temp => temp.Requester.Id.Equals(model.UserModel.User.Id) ||
                temp.Responder.Id.Equals(model.UserModel.User.Id));
                */
            var request = LoginedUser.Connections.Items.Find(
                temp => temp.RequesterId.Equals(model.UserModel.User.Id) ||
                temp.ResponderId.Equals(model.UserModel.User.Id));

            RefreshRequest(model, request);
        }

        public ConnectionsDataWrapper(UserModel loginedUser, DynamicListData<User> users,
            UserModelsWrapper usersWrapper) : base(true)
        {
            LoginedUser = loginedUser;
            UsersWrapper = usersWrapper;

            SearchUsers = users;
            SearchUsers.CollectionChanged += OnSearchCollectionChanged;

            LoginedUser.Connections.CollectionChanged += OnRequestsChanged;
        }

        private void OnSearchCollectionChanged(List<User> searched)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (User user in searched)
                {
                    if (!user.Id.Equals(LoginedUser.User.Id))
                    {
                        ConnectionModel model = GetModel(user, null);
                        model.UserModel.UpdateUser(user);
                    }
                }

                OnRequestsChanged(LoginedUser.Connections.Items);
            });
        }
    }
}