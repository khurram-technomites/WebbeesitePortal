using System;
using System.Collections.Generic;

namespace HelperClasses.DTOs.Authentication
{
    public class CustomeClaims
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
    public class Role
    {
        public string Name { get; set; }
    }
    public class UserAuthData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public AccessToken TokenInfo { get; set; }
        public Role UserRole { get; set; }
        public IList<CustomeClaims> Claims { get; set; }

        public UserAuthData()
        {
            Claims = new List<CustomeClaims>();
            UserRole = new Role();
        }
    }
}
