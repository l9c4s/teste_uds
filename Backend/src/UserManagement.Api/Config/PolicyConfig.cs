using Microsoft.AspNetCore.Authorization;
using UserManagement.Domain.Enums;

namespace UserManagement.Api.Config;

public class MinimumAccessLevelRequirement : IAuthorizationRequirement
{
    public AccessLevel MinimumLevel { get; }

    public MinimumAccessLevelRequirement(AccessLevel minimumLevel)
    {
        MinimumLevel = minimumLevel;
    }
}

public class MinimumAccessLevelHandler : AuthorizationHandler<MinimumAccessLevelRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAccessLevelRequirement requirement)
    {
        // Busca todas as claims de AccessLevel do token
        var accessLevelClaims = context.User.FindAll("AccessLevel").Select(c => c.Value).ToList();

        // Se não há claims de AccessLevel, nega o acesso
        if (!accessLevelClaims.Any())
        {
            return Task.CompletedTask;
        }
        // Verifica se algum dos níveis de acesso do usuário atende ao requisito mínimo
        foreach (var accessLevelClaim in accessLevelClaims)
        {
            // Mapeamento dos nomes do banco para o enum
            var mappedLevel = MapDatabaseNameToEnum(accessLevelClaim);

            if (mappedLevel.HasValue)
            {
                // Lógica da hierarquia: um valor de enum MENOR significa um privilégio MAIOR
                // Se o nível do usuário for menor ou igual ao nível mínimo exigido, ele tem acesso
                if ((int)mappedLevel.Value <= (int)requirement.MinimumLevel)
                {
                    context.Succeed(requirement);
                    break;
                }
            }
        }

        return Task.CompletedTask;
    }

    private AccessLevel? MapDatabaseNameToEnum(string databaseName)
    {
        return databaseName switch
        {
            "Administrator" => AccessLevel.Administrator,
            "Manager" => AccessLevel.Manager,
            "User" => AccessLevel.Common,
            "Viewer" => AccessLevel.Viewer,
            _ => null
        };
    }
}