using System.Security.Cryptography;

class ProvablyFairProtocol
{
    public static int GenerateRandomNumber(int n)
    {
        return RandomNumberGenerator.GetInt32(0, n);
    }
    public static string ComputeHMAC(string key, int number)
    {
        using (var hmac = new HMACSHA3_256(Convert.FromBase64String(key)))
        {
            byte[] hash = hmac.ComputeHash(BitConverter.GetBytes(number));
            return Convert.ToHexString(hash);
        }
    }

    public static bool VerifyHMAC(string key, int number, string hmac)
    {
        string computedHMAC = ComputeHMAC(key, number);
        return computedHMAC.Equals(hmac, StringComparison.OrdinalIgnoreCase);
    }

    public static int GenerateFairNumber(int m, int r, int n)
    {
        return (m + r) % n;
    }
}
