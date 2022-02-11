namespace Arbitr.MassTransit
{
    public class RabbitMqSettings
    {
        public const string KEY = "RabbitMq";

        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
