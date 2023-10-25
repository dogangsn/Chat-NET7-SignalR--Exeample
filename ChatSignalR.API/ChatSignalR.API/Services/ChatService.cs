namespace ChatSignalR.API.Services
{
    public class ChatService
    {
        private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();

        public bool AddUserToList(string userToAdd)
        {
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if ((user.Key.ToLower()) == userToAdd.ToLower())
                    {
                        return false;
                    }
                }
                Users.Add(userToAdd, null);
                return true;
            }
        }

        public void AddUserConnectinId(string user, string connectinId)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectinId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            }
        }

        public string GetConnectionIdUserBy(string user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        public void RemoveUsersFromList(string user)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users.Remove(user);
                }
            }
        }

        public string[] GetOnLineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }




    }
}
