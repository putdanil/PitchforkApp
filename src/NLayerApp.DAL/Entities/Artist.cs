using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NLayerApp.DAL.Entities;

[Keyless]
[Table("artists")]
public partial class Artist
{
    [Column("artist_id")]
    [StringLength(100)]
    public string? ArtistId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("artist_url")]
    [StringLength(100)]
    public string? ArtistUrl { get; set; }
}
