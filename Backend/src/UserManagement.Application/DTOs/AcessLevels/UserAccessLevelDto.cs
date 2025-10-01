namespace UserManagement.Application.DTOs.AcessLevels;

public record UserAccessLevelDto(
    int Id,
    int UserId,
    int AccessLevelId,
    string AccessLevelName,
    DateTime AssignedAt,
    DateTime? RevokedAt,
    bool IsActive
);