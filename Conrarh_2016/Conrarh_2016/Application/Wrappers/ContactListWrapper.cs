using System.Collections.Generic;
using Conarh_2016.Application.Domain;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class ContactListWrapper:DynamicObservableData<ConnectionModel>
	{
		public readonly UserModel LoginedUser;
		public readonly ConnectionsDataWrapper ConnectionsCollection;

		public readonly Dictionary<string, ConnectionModel> DefinedItems = new Dictionary<string, ConnectionModel> ();

		public bool IsEmpty
		{
			get {
				return DefinedItems.Count == 0;
			}
		}

		public ContactListWrapper(UserModel loginedUser, ConnectionsDataWrapper wrapper):base(true)
		{
			LoginedUser = loginedUser;
			ConnectionsCollection = wrapper;

			LoginedUser.Connections.CollectionChanged += OnConnectionsChanged;

			if (!LoginedUser.Connections.IsEmpty ())
				OnConnectionsChanged (LoginedUser.Connections.Items);
		}

		private void OnConnectionsChanged (List<ConnectRequest> connections)
		{

			foreach (ConnectRequest connectRequest in connections) {
				connectRequest.IsChanged -= OnConnectionStateChanged;

				if (connectRequest.Accepted)
					RefreshUserModel (connectRequest);
				else
					connectRequest.IsChanged += OnConnectionStateChanged;
			}

		}

		private void RefreshUserModel(ConnectRequest request)
		{
            /* Todo Validar
			User connectedUser = request.Requester.Id.Equals(LoginedUser.User.Id) ? 
				request.Responder : request.Requester;
                */

            User connectedUser = request.RequesterId.Equals(LoginedUser.User.Id) ?
                AppModel.Instance.Users.Find(request.ResponderId) :
                AppModel.Instance.Users.Find(request.RequesterId);
                 


            if (!DefinedItems.ContainsKey (connectedUser.Id)) 
			{
				Device.BeginInvokeOnMainThread (() => {

					if(!DefinedItems.ContainsKey(connectedUser.Id))
					{
						var model = ConnectionsCollection.GetModel(connectedUser, null);
						InsertItem (0, model);

						DefinedItems.Add (connectedUser.Id, model);

						OnCollectionChanged (new NotifyCollectionChangedEventArgs (
							NotifyCollectionChangedAction.Add, model));
					}
				});
			}
		}

		private void OnConnectionStateChanged (ConnectRequest request)
		{
			if (request.Accepted)
				RefreshUserModel (request);
			request.IsChanged -= OnConnectionStateChanged;

		}
	}
}

