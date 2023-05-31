using System;
using System.Collections.Generic;

namespace ProductSales.Models;

public partial class Address
{
    public Guid AddressId { get; set; }

    public string? HouseNumber { get; set; }

    public Guid? UserId { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public virtual User? User { get; set; }
}
