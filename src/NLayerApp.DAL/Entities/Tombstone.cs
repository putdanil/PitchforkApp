using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NLayerApp.DAL.Entities;

[Table("tombstones")]
public partial class Tombstone
{
    [Column("review_tombstone_id")]
    [StringLength(208)]
    [Unicode(false)]
    public string? ReviewTombstoneId { get; set; }

    [Column("review_url")]
    [StringLength(206)]
    [Unicode(false)]
    public string? ReviewUrl { get; set; }

    [Column("picker_index")]
    public byte? PickerIndex { get; set; }

    [Column("title")]
    [StringLength(144)]
    [Unicode(false)]
    public string? Title { get; set; }

    [Column("score")]
    public double? Score { get; set; }

    [Column("best_new_music")]
    public bool? BestNewMusic { get; set; }

    [Column("best_new_reissue")]
    public bool? BestNewReissue { get; set; }

    [Key]
    [Column("ID")]
    public int Id { get; set; }
}
