using Dapper;
using Bogus;
using GymManagement.Application.Abstractions.Data;

namespace GymManagement.Api.Extensions
{
    public static class SeedDataExtension
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

            using var connection = sqlConnectionFactory.CreateConnection();

            var faker = new Faker();

            var gyms = new List<object>();

            for (int i = 0; i <= 3; i++)
            {
                gyms.Add(CreateFakeGym(faker));
            }

            const string sql = """
                                 INSERT INTO public.gym
                                (id,
                                 name,
                                 description,
                                 street,
                                 city,
                                 zip_code,
                                 schedule)
                                  VALUES(@Id,
                                  @Name,
                                  @Description,
                                  @Street,
                                  @City,
                                  @ZipCode,
                                  @Schedule)
                               """;
            try
            {
                connection.Execute(sql, gyms);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static object CreateFakeGym(Faker faker)
        {
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Description = faker.Lorem.Paragraph(),
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode(),
                Schedule = faker.Lorem.Sentences(3)
            };
        }
    }
}