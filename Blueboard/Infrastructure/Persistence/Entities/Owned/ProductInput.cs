using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Entities.Owned;

[Owned]
public class ProductInput
{
    [Required] public ProductInputType Type { get; set; }

    [Required] public string Identifier { get; set; }

    [Required] public string Label { get; set; }
}

public enum ProductInputType
{
    Boolean,
    Textbox
}