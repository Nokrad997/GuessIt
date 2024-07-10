using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

[Table("friends")]
public class Friends
{
    [Key]
    [Required]
    [Column("friends_id")]
    public int FriendsId { get; set; }
    
    [Required]
    [Column("user_id")]
    [ForeignKey("UserId")]
    public virtual Users UserId { get; set; }
    
    [Required]
    [Column("friend_id")]
    [ForeignKey("UserId")]
    public virtual Users FriendId { get; set; }
    
    [Required]
    [Column("status")]
    [DefaultValue("Pending")]
    [AllowedValues("Pending", "Accepted", "Rejected", "Blocked")]
    public string Status { get; set; }
    
    [Required]
    [Column("created_at")]
    [Timestamp, DataType("timestamp")]
    public byte[] CreatedAt { get; set; }
}