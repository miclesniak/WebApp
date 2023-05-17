namespace WebApp.Models
{
    public class ApiBeer
    {
        public string name { get; set; }
        public string first_brewed { get; set; }
        public float abv { get; set; }
        public Volume boil_volume { get; set; }

        public class Volume
        {
            public float value { get; set; }
        }
    }
}
