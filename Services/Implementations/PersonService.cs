using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;


namespace Services.Implementations
{
    public class PersonService(IPersonRepository PersonRepository, ILogger<PersonService> logger):IService<Person>
    {
        public async Task<List<Person>> GetAllAsync()
        {
            try
            {
                var Persons = await PersonRepository.GetAllAsync();
                logger.LogInformation("Retrieved {PersonCount} Persons", Persons.Count());
                return Persons.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving Persons: {Message}", ex.Message);
                throw new Exception("Error retrieving Persons", ex);
            }
        }

        public async Task<Person?> GetByIdAsync(long id)
        {
            try
            {
                var Person = await PersonRepository.GetByIdAsync(id);
                if (Person != null)
                {
                    logger.LogInformation("Retrieved Person with ID {PersonId}", id);
                }
                else
                {
                    logger.LogWarning("Person with ID {PersonId} not found", id);
                }
                return Person;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving Person with ID {PersonId}: {Message}", id, ex.Message);
                throw new Exception("Error retrieving Person", ex);
            }
        }

        public async Task AddAsync(Person Person)
        {
            try
            {
                await PersonRepository.AddAsync(Person);
                logger.LogInformation("Added Person with ID {PersonId}", Person.Id);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                logger.LogWarning("Person already exists: {Email} {Surname}", Person.Name, Person.Surname);
                throw new InvalidOperationException("A Person with this name already exists.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding Person: {Message}", ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                return sqlEx.Number == 2601 || sqlEx.Number == 2627;
            }
            return false;
        }
        public Task UpdateAsync(Person entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
