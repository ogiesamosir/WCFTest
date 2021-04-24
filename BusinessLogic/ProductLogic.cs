using BusinessDomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessLogic
{
    public class ProductLogic
    {
        ProductBDO productBDO = new ProductBDO();
        public ProductBDO GetProduct(int id)
        {
            return productBDO.GetProduct(id);
        }
        public bool UpdatedProduct(ref ProductBDO productbdo, ref string message)
        {
            var productInDB = GetProduct(productbdo.Product_ID);
            if (productInDB == null)
            {
                message = "Cannot get product for this ID";
                return false;
            }
            if (productBDO.Discontinued == true && productInDB.Units_On_Order > 0)
            {
                message = "cannot discontinue this product";
                return false;
            }
            else
            {
                return productBDO.UpdateProduct(ref productbdo, ref message);
            }
        }
    }
}