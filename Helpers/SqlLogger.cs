using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;

namespace Kahveci.Helpers
{
    public static class SqlLogger
    {
        private static readonly string logFilePath = "Logs/sql_log.txt";
        private static readonly string hashFilePath = "Logs/sql_log_hashes.txt";

        public static void LogUniqueSqlCommand(string sqlCommand)
        {
            // SQL komutunun hash değerini hesapla
            string sqlHash = ComputeSHA256Hash(sqlCommand);

            // Eğer hash zaten kayıtlıysa, işlemi sonlandır (yani aynı işlem tekrar loglanmasın)
            if (IsHashAlreadyLogged(sqlHash))
            {
                return; // Aynı işlem daha önce yapıldığı için loglanmaz
            }

            // Logs klasörünü oluştur (eğer yoksa)
            EnsureLogsDirectoryExists();

            // Hash değerini kaydet
            File.AppendAllText(hashFilePath, sqlHash + Environment.NewLine);

            // SQL komutunu tarih ile log dosyasına ekle
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {sqlCommand}{Environment.NewLine}");
        }

        private static string ComputeSHA256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        private static bool IsHashAlreadyLogged(string hash)
        {
            // Eğer hash dosyası yoksa, henüz hiç işlem yapılmamış demektir
            if (!File.Exists(hashFilePath))
                return false;

            // Hash dosyasındaki tüm hash değerlerini oku
            var hashes = File.ReadAllLines(hashFilePath);
            
            // Eğer hash dosyasına daha önce aynı hash kaydedildiyse, o zaman sorgu yapılmış demektir
            return hashes.Contains(hash);
        }

        private static void EnsureLogsDirectoryExists()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }
    }
}
