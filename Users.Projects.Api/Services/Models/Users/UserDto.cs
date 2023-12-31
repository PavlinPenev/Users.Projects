﻿namespace Users.Projects.Api.Services.Models.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
