using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BusinessDomainObject;
using BusinessLogic;
namespace ServiceInterfaceLayer
{
 // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class
//name "Service1" in both code and config file together.
 public class ProductService : IProductService
    {
        ProductLogic productLogic = new ProductLogic();
        public Product GetProduct(int id)
        {
            ProductBDO productBDO = null;
            try
            {
                productBDO = productLogic.GetProduct(id);
            }
            catch (Exception e)
            {
                string msg = e.Message;
                string reason = "GetProduct Exception";
                throw new FaultException<ProductFault>(new ProductFault(msg), reason);
            }
            if (productBDO == null)
            {
                string msg = string.Format("No product found for id {0}", id);
                string reason = "GetProduct Empty Product";
                throw new FaultException<ProductFault>(new ProductFault(msg), reason);
            }
            Product product = new Product();
            TranslateProductBDOToProductDTO(productBDO, product);
            return product;
        }
        public bool UpdateProduct(ref Product product, ref string message)
        {
            bool result = true;
            if (product.UnitPrice <= 0)
            {
                message = "Price cannot be <=0";
                result = false;
            }
            else if (string.IsNullOrEmpty(product.ProductName))
            {
                message = "Product name cannot be empty";
                result = false;
            }
            else if (string.IsNullOrEmpty(product.QuantityPerUnit))
            {
                message = "Quantity cannot be empty";
                result = false;
            }
            else
            {
                try
                {
                    ProductBDO productBDO = null;
                    TranslateProductDTOToProductBDO(product, productBDO);
                    result = productLogic.UpdatedProduct(ref productBDO, ref message);
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                    throw new FaultException<ProductFault>
                     (new ProductFault(msg), msg);
                }
            }
            return result;
        }
        private void TranslateProductBDOToProductDTO(ProductBDO productBDO, Product
       product)
        {
            product.ProductID = productBDO.Product_ID;
            product.ProductName = productBDO.Product_Name;
            product.QuantityPerUnit = productBDO.Quantity_Per_Unit;
            product.UnitPrice = productBDO.Unit_Price;
            product.Discontinued = productBDO.Discontinued;
        }
        private void TranslateProductDTOToProductBDO(Product product, ProductBDO
       productBDO)
        {
            productBDO.Product_ID = product.ProductID;
            productBDO.Product_Name = product.ProductName;
            productBDO.Quantity_Per_Unit = product.QuantityPerUnit;
            productBDO.Unit_Price = product.UnitPrice;
            productBDO.Discontinued = product.Discontinued;
        }
    }
}