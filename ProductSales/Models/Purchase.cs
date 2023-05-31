using System;
using System.Collections.Generic;

namespace ProductSales.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public int? PurchaseQuantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
