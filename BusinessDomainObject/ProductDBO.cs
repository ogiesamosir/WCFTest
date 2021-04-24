using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomainObject
{
    public class ProductBDO
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Quantity_Per_Unit { get; set; }
        public decimal Unit_Price { get; set; }
        public int Units_In_Stok { get; set; }
        public int Reorder_Level { get; set; }
        public int Units_On_Order { get; set; }
        public bool Discontinued { get; set; }
        public ProductBDO GetProduct(int id)
        {
            ProductBDO productBDO = null;
            using (var NWEntities = new NorthwindEntities())
            {
                Product product = (from p in NWEntities.Products
                                   where p.ProductID == id
                                   select p).FirstOrDefault();
                if (product != null)
                    productBDO = new ProductBDO()
                    {
                        Product_ID = product.ProductID,
                        Product_Name = product.ProductName,
                        Quantity_Per_Unit = product.QuantityPerUnit,
                        Unit_Price = (decimal)product.UnitPrice,
                        Units_In_Stok = (int)product.UnitsInStock,
                        Reorder_Level = (int)product.ReorderLevel,
                        Units_On_Order = (int)product.UnitsOnOrder,
                        Discontinued = product.Discontinued
                    };
            }
            return productBDO;
        }
        public bool UpdateProduct(ref ProductBDO productBDO, ref string message)
        {
            message = "product updated successfully";
            bool ret = true;
            using (var NWEntities = new NorthwindEntities())
            {
                var productID = productBDO.Product_ID;
                Product productInDB = (from p in NWEntities.Products
                                       where p.ProductID == productID
                                       select p).FirstOrDefault();
                if (productInDB == null)
                {
                    throw new Exception("No product with ID " + productBDO.Product_ID);
                }
                NWEntities.Products.Remove(productInDB);
                productInDB.ProductName = productBDO.Product_Name;
                productInDB.QuantityPerUnit = productBDO.Quantity_Per_Unit;
                productInDB.UnitPrice = productBDO.Unit_Price;
                productInDB.Discontinued = productBDO.Discontinued;
                NWEntities.Products.Attach(productInDB);
                NWEntities.Entry(productInDB).State =
               System.Data.Entity.EntityState.Modified;
                NWEntities.SaveChanges();
            }
            return ret;
        }
    }
}
