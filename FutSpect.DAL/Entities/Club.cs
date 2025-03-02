using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities;

public class Club
{
    [Key]
    public int Id { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(100)]
    public required string Name { get; init; }
}