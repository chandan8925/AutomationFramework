using BenefitsDashboardAPITests.APICalls;
using Microsoft.Extensions.Configuration;
using System.Text;

public class APITestFactory
{
    private IConfiguration _config;
    public string BaseUrl;
    public string Token;
    public static Random Random = new Random();
    public EmployeeAPICalls EmployeeAPICalls;

    [SetUp]
    public void Setup()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json");

        _config = builder.Build();
        BaseUrl = _config["BaseUrl"]!;
        Token = _config["Token"]!;
        EmployeeAPICalls = new EmployeeAPICalls(BaseUrl, Token);
    }

    // Generate random string of any length
    public static string GenerateRandomString(int length)
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
    public static int GenerateRandomDependent()
    {
        var lowerBound = 0;
        var upperBound = 32;
        return Random.Next(lowerBound, upperBound);
    }
}
