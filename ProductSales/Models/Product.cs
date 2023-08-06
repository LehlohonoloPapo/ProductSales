﻿using System;
using System.Collections.Generic;

namespace ProductSales.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public string? ImageUrls { get; set; }

    public bool? IsOnSale { get; set; }

    public decimal? SalePercentage { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
