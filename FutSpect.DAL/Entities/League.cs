using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities;

[Index(nameof(Name), nameof(CountryId), IsUnique = true)]
public class League
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(100)]
    public required string Name { get; set;}

    [Required]
    public required int CountryId { get; init; }

    [ForeignKey(nameof(CountryId))]
    public Country? Country { get; init; }
}