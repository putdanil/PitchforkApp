using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Models;

[Keyless]
[Table("reviews")]
public partial class Review
{
    [Column("review_url")]
    public string ReviewUrl { get; set; } = null!;

    [Column("is_standard_review")]
    public bool IsStandardReview { get; set; }

    [Column("pub_date")]
    public DateTime PubDate { get; set; }

    [Column("body")]
    public string Body { get; set; } = null!;
}
