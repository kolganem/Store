﻿namespace StoreProject.Models;

public class UserRole
{
    public User User { get; set; }

    public IEnumerable<string> Roles { get; set; }
}