namespace website.Application.Infrastructure.DataProtection
{
    public interface IDataProtector
    {
        string Protect(string value);
        string Unprotect(string value);
        string GenerateKey(int length);
    }
}