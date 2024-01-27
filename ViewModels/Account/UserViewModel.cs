using SocialNetworkHT.Models.Users;
using System;

namespace SocialNetworkHT.ViewModels.Account
{
    public class UserViewModel
    {
        public User User { get; set; }

        public UserViewModel(User user)
        {
            User = user;
        }
    }
}
