using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Beer
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime RelaseDate { get; set; }
        public float Volume { get; set; }
        public float Voltage { get; set; }
        public decimal Price { get; set; }
    }
}
