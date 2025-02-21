﻿using Microsoft.AspNetCore.Identity;

namespace TaskPlannerAPI.Models;

/// <summary>
/// Custom identity user model.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public ICollection<UserBoard> UserBoards { get; set; }
}