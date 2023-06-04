using System;
using System.Collections.Generic;

namespace ProductSales.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PasswordHash { get; set; }

    public Guid? RoleId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Role? Role { get; set; }
}
