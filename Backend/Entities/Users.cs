using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;
[Table("users")]
public class Users
{
    [Key]
    [Required]
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
    
    [Required]
    [Column("verified")]
    public bool Verified { get; set; }
    
    [Timestamp, DataType("timestamp")]
    [Column("updated_at")]
    public byte[] CreatedAt { get; set; }
    
    [Timestamp]
    [Column("created_at")]
    public byte[] UpdatedAt { get; set; }

    public ICollection<Statistics> Statistics { get; set; }
    public ICollection<Friends> Friends { get; set; }
    public ICollection<Games> Games { get; set; }
    public ICollection<Leaderboard> Leaderboards { get; set; }
    public ICollection<UserAchievements> UserAchievements { get; set; }
}