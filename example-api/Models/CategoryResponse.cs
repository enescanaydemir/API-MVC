using System;

namespace example_api.Models;

public class CategoryResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public List<string>? Categories { get; set; }
}
