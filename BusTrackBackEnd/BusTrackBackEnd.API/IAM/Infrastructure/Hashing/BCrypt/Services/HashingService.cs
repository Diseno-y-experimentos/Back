using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
// Agregamos este alias para evitar escribir "global::" en cada línea y limpiar el código
using BCryptLib = BCrypt.Net.BCrypt;
namespace BusTrackBackEnd.API.IAM.Infrastructure.Hashing.BCrypt.Services;

public class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        return BCryptLib.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCryptLib.Verify(password, hash);
    }
}