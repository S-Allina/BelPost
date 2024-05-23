using BelPost.Models;

namespace BelPost.Service
{
    static public class Services
    {
        private static string IdСurrentUser;

        public static void SetIdCurrentUser (string id)
        {
            IdСurrentUser= id;
        }
        public static string GetIdCurrentUser ()
        {
            return IdСurrentUser;
        }
        public static byte[] ConvertPictureForDb(IFormFile formFile) {
            
            byte[] imageData = null;

            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(formFile.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)formFile.Length);
            }
            // установка массива байтов
            return imageData;
        }
    }
}
