namespace LotteryServices.Interfaces
{
    public interface IEncriptado
    {
        public string Encrypt(string text);
        public string Decrypt(string cipherText);
    }
}
