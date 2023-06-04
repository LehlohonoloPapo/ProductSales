using System;
using System.Collections.Generic;

namespace ProductSales.Models;

public partial class Province
{
    public Guid ProvinceId { get; set; }

    public string ProvinceName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
