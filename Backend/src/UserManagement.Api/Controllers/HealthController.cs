using Microsoft.AspNetCore.Mvc;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Testar conexão com banco
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return StatusCode(503, new { status = "unhealthy", message = "Cannot connect to database" });
            }

            return Ok(new 
            { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                database = "connected",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { status = "unhealthy", error = ex.Message });
        }
    }
}