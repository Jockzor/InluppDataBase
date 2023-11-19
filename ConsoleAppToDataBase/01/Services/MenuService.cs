
using _01.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace _01.Services;

internal class MenuService
{
    private readonly PersonService _personService;
    private readonly ProductService _productService;
    public MenuService(PersonService personService, ProductService productService )
    {
        _personService = personService;
        _productService = productService;
    }

    public async Task MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("1. Create person\n2. Read persons\n3. Update person\n4. Delete person\n5. Add Product\n6. Show all products\n");
            Console.Write("Enter option: ");
            string option = Console.ReadLine()!;

            switch (option)
            {
                case "1":
                    await CreatePersonMenu();
                    break;
                case "2":
                   await ShowAllMenu();
                    break;

                case "3":
                   await UpdatePersonMenu();
                    break;
                case "4":
                  await  DeletePersonMenu();
                    break;
                case "5":
                   await AddProductMenu();
                    break;
                case "6":
                  await ShowAllProducts();
                    break;                  
            }
            Console.ReadKey();
        }
    }

    public async Task  CreatePersonMenu()
    {
        Console.Clear();

        var person = new PersonEntity();

        Console.Write("Firstname: ");
        person.FirstName = Console.ReadLine()!;
        Console.Write("Lastname: ");
        person.LastName = Console.ReadLine()!;
        Console.Write("Email: ");
        person.Email = Console.ReadLine()!;

       person.Address = new AddressEntity();
        Console.Write("Streetname: ");
        person.Address.StreetName = Console.ReadLine()!;
        Console.Write("Streetnumber: ");
        person.Address.StreetNumber = Console.ReadLine()!;   

        person.Role = new RoleEntity();
        Console.Write("User or Admin?: ");
        person.Role.Name = Console.ReadLine()!;
        if (await _personService.CreateAsync(person))
            Console.WriteLine("Person was created successfully");
        else Console.WriteLine($"A personen with email <{person.Email}> already exists, try again.");

    }

    public async Task ShowAllMenu()
    {
        Console.Clear();

        try
        {
            var people = await _personService.ReadAsync();

            if (people.Any())
            {
                Console.WriteLine("People in the database:");
                foreach (var person in people)
                {
                    Console.WriteLine($"ID: {person.PersonId}, Name: {person.FirstName} {person.LastName}");

                    if (person.Address != null)
                    {
                        Console.WriteLine($"Address: {person.Address.StreetName} {person.Address.StreetNumber}");
                    }
                    else
                    {
                        Console.WriteLine("No address information available");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No people in the database");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }




    public async Task UpdatePersonMenu()
    {
        Console.Clear();
        Console.Write("Enter the ID of the person to update: ");
        if (int.TryParse(Console.ReadLine(), out int personId))
        {
            var personToUpdate = await _personService.GetPersonByIdAsync(personId);
                

            if (personToUpdate != null)
            {
                Console.WriteLine($"Updating person with ID {personId}.");
                Console.Write("Enter new first name (press Enter to keep current): ");
                string newFirstName = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(newFirstName))
                {
                    personToUpdate.FirstName = newFirstName;
                }

                Console.Write("Enter new last name (press Enter to keep current): ");
                string newLastName = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(newLastName))
                {
                    personToUpdate.LastName = newLastName;
                }

                Console.Write("Enter new email (press Enter to keep current): ");
                string newEmail = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(newEmail))
                {
                    personToUpdate.Email = newEmail;
                }

                Console.Write("Update address? (Y/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                {
                    Console.Write("Enter new street name (press Enter to keep current): ");
                    string newStreetName = Console.ReadLine()!;
                    Console.Write("Enter new street number (press Enter to keep current): ");
                    string newStreetNumber = Console.ReadLine()!;

                    if (!string.IsNullOrEmpty(newStreetName) || !string.IsNullOrEmpty(newStreetNumber))
                    {
                        if (personToUpdate.Address == null)
                        {
                            personToUpdate.Address = new AddressEntity();
                        }

                        if (!string.IsNullOrEmpty(newStreetName))
                        {
                            personToUpdate.Address.StreetName = newStreetName;
                        }

                        if (!string.IsNullOrEmpty(newStreetNumber))
                        {
                            personToUpdate.Address.StreetNumber = newStreetNumber;
                        }
                    }
                }

                if (await _personService.UpdateAsync(personToUpdate))
                    Console.WriteLine($"Person with ID {personToUpdate.PersonId} updated successfully.");
            }
            else
            {
                Console.WriteLine($"Person with ID {personId} not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid person ID.");
        }
    }


public async Task DeletePersonMenu()
    {
        Console.Clear();
        Console.Write("Enter email of the person to be deleted: ");
        string email = Console.ReadLine()!;

        var personToBeDeleted = await _personService.DeleteAsync(email);
        if (personToBeDeleted != null)
        {
            Console.WriteLine($"{personToBeDeleted.FirstName} {personToBeDeleted.LastName} was removed.");
        }
        else  Console.WriteLine("Person was not found");     
    }

    public async Task AddProductMenu()
    {
        Console.Clear();
        var product = new ProductEntity();

        Console.Write("Productname: ");
        product.Name = Console.ReadLine()!;

        Console.Write("Product description: ");
        product.Description = Console.ReadLine()!;

        product.Category = new CategoryEntity();
        Console.WriteLine("Categoryname: ");
        product.Category.Name = Console.ReadLine()!;

        if (await _productService.AddProductAsync(product))
            Console.WriteLine("Product was added");
        else
            Console.WriteLine("Product already exists, product was not added");
    }

    public async Task ShowAllProducts()
    {
        Console.Clear();

        var products = await _productService.GetAllAsync();
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name} ({product.Category.Name})");
            Console.WriteLine("");
        }

    }


}
