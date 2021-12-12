using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {

            Product product = new Product();
            product.ProductId = 6;
            product.ProductName = "Deneme";
            product.CategoryId = 2;
            product.UnitPrice = 100;
            product.UnitsInStock = 2;

            Product product2 = new Product();
            product2.ProductId = 7;
            product2.ProductName = "Deneme2";
            product2.CategoryId = 22;
            product2.UnitPrice = 100;
            product2.UnitsInStock = 2;

            InMemoryProductDal inMemoryProductDal = new InMemoryProductDal();
            inMemoryProductDal.Add(product);
            inMemoryProductDal.Add(product2);

            foreach (var item in inMemoryProductDal.GetAll())
            {
                Console.WriteLine(item.ProductName);
            }

            Console.WriteLine("---------------------------------");

            inMemoryProductDal.Delete(product2);

            foreach (var item in inMemoryProductDal.GetAll())
            {
                Console.WriteLine(item.ProductName);
            }

            Console.WriteLine("---------------------------------");

            product.ProductName = "Bilgisayar";
            inMemoryProductDal.Update(product);

            foreach (var item in inMemoryProductDal.GetAll())
            {
                Console.WriteLine(item.ProductName);
            }

            Console.ReadLine();
        }
    }
}
