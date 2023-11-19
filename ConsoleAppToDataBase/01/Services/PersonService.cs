using _01.Context;
using _01.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace _01.Services;

internal class PersonService
{

    private readonly ApplicationDbContext _context;
    public PersonService()
    {
        _context = new ApplicationDbContext();
    }

    public async Task<bool> CreateAsync(PersonEntity person)
    {
        try
        {
            var existingPerson = await _context.People.FirstOrDefaultAsync(p => p.Email == person.Email);

            if (existingPerson == null)
            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return true;
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return false;
    }


    public async Task<IEnumerable<PersonEntity>> ReadAsync()
    {
        try
        {
            return await _context.People.Include(p => p.Address).ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return null!;
    }




    public async Task<bool> UpdateAsync(PersonEntity person)
    {
        try
        {
            var existingPerson = await _context.People.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);

            if (existingPerson != null)
            {
                existingPerson.FirstName = person.FirstName;
                existingPerson.LastName = person.LastName;
                existingPerson.Email = person.Email;


                if (existingPerson.Address != null && person.Address != null)
                {
                    existingPerson.Address.StreetName = person.Address.StreetName;
                    existingPerson.Address.StreetNumber = person.Address.StreetNumber;
                }

                await _context.SaveChangesAsync();
                return true;
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return false;
    }



    public async Task<PersonEntity> DeleteAsync(string email)
    {
        // Delete
        try
        {
            var personToDelete = await _context.People.Include(p => p.Address).FirstOrDefaultAsync(p => p.Email == email);

            if (personToDelete != null)
            {
                _context.Addresses.RemoveRange(personToDelete.Address);
                _context.People.Remove(personToDelete);

                await _context.SaveChangesAsync();
                return personToDelete;
            }
           
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return null!;
    }

    public async Task<PersonEntity> GetPersonByIdAsync(int personId)
    {
        return await _context.People
            .Include(p => p.Address)
            .Include(p => p.Role)
            .FirstOrDefaultAsync(p => p.PersonId == personId);
    }

}
