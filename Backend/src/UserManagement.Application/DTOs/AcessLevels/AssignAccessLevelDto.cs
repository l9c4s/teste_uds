namespace UserManagement.Application.DTOs.AcessLevels;


public record AssignAccessLevelDto(
    int UserId,
    int AccessLevelId
);