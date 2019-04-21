namespace Website_Verstegen.Models
{
    public class SpotlightContent
    {
        public int Id { get; set; }
        public SpotLightConnector SpotlightConnector { get; set; }
        public int SpotlightConnectorId { get; set; }
        public string SpType { get; set; }
        public int SpId { get; set; }
    }
}
