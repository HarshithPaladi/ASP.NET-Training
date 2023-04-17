namespace WebAPI.Models
{
    public class ProductModel
    {
        public string product_code { get; set; }
        public string product_name { get; set; }
        public float price { get; set; }
        public int quantity_sold { get; set; }
        public int quantity_remaining { get; set; }
    }


}