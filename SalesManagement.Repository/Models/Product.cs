﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using SalesManagement.Repository.Abstractions;

namespace SalesManagement.Repositories.Models;

public partial class Product : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? CategoryId { get; set; }

    public string Description { get; set; }
    
    public string ImageFile { get; set; }

    public decimal Price { get; set; }

    public string Ingredients { get; set; }

    public string UsageInstructions { get; set; }

    public int StockQuantity { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public string LastModifiedBy { get; set; }
    
    public virtual Category Category { get; set; }
}