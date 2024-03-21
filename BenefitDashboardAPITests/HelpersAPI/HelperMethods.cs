using System.Text;

namespace BenefitDashboardAPITests.HelpersAPI
{
    public class HelperMethods
    {
        public static Random Random = new Random();
        public HelperMethods()
        {
           
        }
        // Generate random string of any length
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[Random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }

        // Generate random dependent between 0-32
        public int GenerateRandomDependent()
        {
            var lowerBound = 0;
            var upperBound = 32;
            return Random.Next(lowerBound, upperBound);
        }
    }
}
