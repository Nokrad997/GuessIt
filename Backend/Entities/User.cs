using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Entities.Interfaces;

namespace Backend.Entities;
[Table("users")]
public class User : IHasTimeStamp
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(24)]
    [Column("username")]
    [Index(IsUnique = true)]
    public string Username { get; set; }
    
    [Required]
    [MaxLength(24)]
    [Column("email")]
    [Index(IsUnique = true)]
    public string Email { get; set; }
    
    [Required]
    [Column("password")]
    public string Password { get; set; }
    
    [Column("verified")]
    [DefaultValue(false)]
    public bool Verified { get; set; }

    [Column("is_admin")] 
    [DefaultValue(false)]
    public bool isAdmin { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Statistics> Statistics { get; set; }
    public ICollection<Friends> Friends { get; set; }
    public ICollection<Game> Games { get; set; }
    public ICollection<Leaderboard> Leaderboards { get; set; }
    public ICollection<UserAchievements> UserAchievements { get; set; }
}