namespace CDTKPMNC_STK_BE.Utilities
{
    static public class OTPHelper
    {
        /// <summary>
        /// Tạo mã RegisterOtp ngẫu nhiên
        /// </summary>
        /// <returns></returns>
        static public int GenerateOtp()
        {
            var random = new Random();
            int otp = random.Next(100000, 999999);
            return otp;
        }
    }
}
