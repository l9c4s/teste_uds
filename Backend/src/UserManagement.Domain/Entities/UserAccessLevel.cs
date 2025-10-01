
namespace UserManagement.Domain.Entities;

public class UserAccessLevel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AccessLevelId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

  

    



    //teste
    public virtual User User { get; set; } = null!;
    public virtual AccessLevel AccessLevel { get; set; } = null!;
}