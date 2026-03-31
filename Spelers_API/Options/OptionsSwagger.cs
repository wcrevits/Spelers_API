namespace Spelers_API.Options
{
    public class OptionsSwagger
    {
        public string JsonRoute { get; set; } = "swagger/{documentName}/swagger.json";
        public string UiEndpoint { get; set; } = "/swagger/v1/swagger.json";
        public string Description { get; set; } = "Spelers API V1";
    }
}