﻿using SocialNetworkHT.Models.Users;
using System;
using System.Collections.Generic;

namespace SocialNetworkHT.ViewModels.Account
{
    public class UserViewModel
    {
        public User User { get; set; }

        public UserViewModel(User user)
        {
            User = user;
        }

        public List<User> Friends { get; set; }
    }
}