using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

[Table("friends")]
public class Friends : IHasTimeStamp
{
    [Key]
    [Required]
    [Column("friends_id")]
    public int FriendsId { get; set; }
    
    [Required]
    [ForeignKey("UserIdFk")]
    [Column("user_id")]
    public int UserIdFk { get; set; }
    
    public User User { get; set; }
    
    [Required]
    [ForeignKey("FriendIdFk")]
    [Column("friend_id")]
    public int FriendIdFk { get; set; }
    
    public User Friend { get; set; }
    
    [Required]
    [Column("status")]
    [DefaultValue("Pending")]
    [AllowedValues("Pending", "Accepted", "Rejected", "Blocked")]
    public string Status { get; set; }
}