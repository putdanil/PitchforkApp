using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Models;

[Keyless]
[Table("author_review_map")]
public partial class AuthorReviewMap
{
    [Column("review_url")]
    public string ReviewUrl { get; set; } = null!;

    [Column("author")]
    public string Author { get; set; } = null!;
}
