namespace website.Application.Services.DataProtection
{
    public interface IProtector
    {
        string Protect(string value);
        string Unprotect(string value);
        string GenerateKey(int length);
    }
}