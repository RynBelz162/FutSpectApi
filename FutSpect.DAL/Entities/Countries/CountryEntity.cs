using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Countries;

[Index(nameof(Name), IsUnique = true)]
public class CountryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [Required]
    [MaxLength(75)]
    public required string Name { get; init; }

    [Required]
    [MaxLength(2)]
    public required string Abbreviation { get; init; }
}