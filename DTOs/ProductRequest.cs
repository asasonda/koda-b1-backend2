using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.DTOs;

public record ProductRequest(
    [Required(ErrorMessage = "Code is required")]
    string ProductCode, 

    [Required(ErrorMessage = "Name is required")]
    string Name, 
    decimal Price, 
    int Stock, 
    string Category
);