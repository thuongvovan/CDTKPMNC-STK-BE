namespace CDTKPMNC_STK_BE.Utilities
{
    static public class RandomHelper
    {
        static public int RandomWithin(int min = 0, int max = int.MaxValue)
        {
            var random = new Random();
            int randomValue = random.Next(min, max+1);
            return randomValue;
        }
        
        /// <summary>
        /// Tạo mã RegisterOtp ngẫu nhiên
        /// </summary>
        /// <returns></returns>
        static public int GenerateOtp()
        {
            int otp = RandomWithin(100000, 999999);
            return otp;
        }

        static public T GetRandomInArray<T>(T[] array)
        {
            var random = new Random();
            T[] shuffledArray = array.OrderBy(x => random.Next()).ToArray();
            T randomT = shuffledArray[random.Next(shuffledArray.Length)];
            return randomT;
        }
    }
}
