using System.Collections.Generic;

namespace CashRegister.Models
{
    class Product
    {
        public string  ProdCode  { get; set; }
        public string  Name      { get; set; }
        public decimal RegPrice  { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Weight    { get; set; }
        public bool    IsOnSale  { get; set; }

        public enum ChargeType { Unit, Weight };
        public ChargeType ChargeBy { get; set; }

        private static Dictionary<string, Product> _products = new Dictionary<string, Product>()
        {
            { "CHRO",
                new Product("CHRO", "Box of Cheerios", 6.99m, 5.99m, ChargeType.Unit) },
            { "RDBL",
                new Product("RDBL", "Can of Redbull", 3.99m, 2.50m, ChargeType.Unit) },
            { "APPL",
                new Product("APPL", "Apple", 2.49m, 1.99m, ChargeType.Weight, 0.25m) },
            { "CRRT",
                new Product("CRRT", "Ground Beef", 4.99m, 4.65m, ChargeType.Weight, 0.15m) }
        };

        public static Dictionary<string, Product> All { get { return _products; } }

        public Product(string prodCode, string name, decimal regPrice, decimal salePrice,
            ChargeType chargeBy, decimal weight = 0, bool isOnSale = false)
        {
            ProdCode  = prodCode;
            Name      = name;
            RegPrice  = regPrice;
            SalePrice = salePrice;
            ChargeBy  = chargeBy;
            Weight    = weight;
            IsOnSale  = isOnSale;
        }

        public static Product Get(string productId)
        {
            Product product;
            _products.TryGetValue(productId, out product);
            return product;
        }
    }
}
