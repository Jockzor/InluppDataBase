using _01.Context;
using _01.Entities;
using _01.Services;
using Microsoft.Identity.Client;

namespace _01
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            var personService = new PersonService();
            var productService = new ProductService();
            var menuService = new MenuService(personService, productService);

          await menuService.MainMenu();
           
        }

        

       
       

       
    }
}