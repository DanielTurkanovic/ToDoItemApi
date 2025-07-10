namespace ToDoItemApi.ApplicationServices
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string email);
    }
}
