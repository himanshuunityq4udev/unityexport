using System;
using System.Security.Cryptography;

public static class KeyGenerator
{

    public static string GenerateKey(int size)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[size];
            rng.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}


