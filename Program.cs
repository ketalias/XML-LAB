using System;
using System.Collections.Generic;
using System.Xml;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Використання потокових класів для обробки Xml
            string xmlFile = @"/Users/roman07/Projects/XML_lab3/Shop.xml";

            using (XmlTextWriter writer = new XmlTextWriter(xmlFile, null))
            {
                writer.WriteStartDocument();
                writer.WriteComment("Створено @ " + DateTime.Now.ToString());

                writer.WriteStartElement("shopList");

                writer.WriteStartElement("product");
                writer.WriteElementString("productName", "Молоко");
                writer.WriteElementString("quantity", "10");
                writer.WriteElementString("pricePerUnit", "1.5");
                writer.WriteElementString("shelfLife", "0"); 
                writer.WriteEndElement();

                writer.WriteStartElement("product");
                writer.WriteElementString("productName", "Хліб");
                writer.WriteElementString("quantity", "20");
                writer.WriteElementString("pricePerUnit", "0.8");
                writer.WriteElementString("shelfLife", "-3"); 
                writer.WriteEndElement();

                writer.WriteStartElement("product");
                writer.WriteElementString("productName", "Яйця");
                writer.WriteElementString("quantity", "30");
                writer.WriteElementString("pricePerUnit", "0.2");
                writer.WriteElementString("shelfLife", "14"); 
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            var productList = new List<dynamic>();
            double totalCost = 0.0;
            DateTime currentDate = DateTime.Now;
            var totalExpiredQuantity = 0;

            using (XmlTextReader reader = new XmlTextReader(xmlFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "product")
                    {
                        reader.ReadStartElement("product");

                        var productName = reader.ReadElementString("productName");
                        var quantity = reader.ReadElementString("quantity");
                        var pricePerUnit = reader.ReadElementString("pricePerUnit");
                        var shelfLife = reader.ReadElementString("shelfLife");

                        int productQuantity = Convert.ToInt32(quantity);
                        double productPrice = Convert.ToDouble(pricePerUnit);
                        int productShelfLife = Convert.ToInt32(shelfLife);

                        productList.Add(new
                        {
                            productName,
                            quantity = productQuantity,
                            pricePerUnit = productPrice,
                            shelfLife = productShelfLife
                        });

                        totalCost += productQuantity * productPrice;

                    }
                }
            }

            Console.WriteLine("Продукти з Shop.xml:");

            foreach (var product in productList)
            {
                Console.WriteLine($"Назва товару: {product.productName}, Кількість: {product.quantity}, Ціна за одиницю: {product.pricePerUnit}, Термін придатності: {product.shelfLife} днів");
            }

            Console.WriteLine($"\nЗагальна вартість всіх продуктів у магазині: {totalCost} доларів");

            #endregion

            #region Додатковий приклад обробки XML в пам'яті
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);

            XmlNodeList productNodes = doc.GetElementsByTagName("product");

            foreach (XmlElement product in productNodes)
            {
                string name = product["productName"].InnerText;
                int quantity = int.Parse(product["quantity"].InnerText);
                double pricePerUnit = double.Parse(product["pricePerUnit"].InnerText);
                int shelfLife = int.Parse(product["shelfLife"].InnerText);

                double totalValue = quantity * pricePerUnit;
                Console.WriteLine($"Товар '{name}' - Загальна вартість: {totalValue} доларів");

                DateTime expirationDate = currentDate.AddDays(shelfLife);
                if (expirationDate < currentDate)
                {
                    Console.WriteLine($"Товар '{name}' протермінований з кількістю: {quantity}");
                }
            }
            #endregion

            Console.ReadKey();
        }
    }
}
