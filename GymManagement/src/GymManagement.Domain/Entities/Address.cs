using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class Address
{
    private Address(
        string street,
        string city,
        string zipCode,
        string? region,
        string? country)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
        Region = region;
        Country = country;
    }

    // EF Core
    private Address()
    {
    }

    public string Street { get; } = string.Empty;
    public string City { get; } = string.Empty;
    public string ZipCode { get; } = string.Empty;
    public string? Region { get; }
    public string? Country { get; }

    public static Result<Address> Create(
        string street,
        string city,
        string zipCode,
        string region,
        string country)
    {
        if (string.IsNullOrWhiteSpace(street) || street.Length > 100)
            return Result.Failure<Address>(new Error("Address.InvalidStreet",
                "Улица должна быть от 1 до 100 символов"));

        if (string.IsNullOrWhiteSpace(city) || city.Length > 50)
            return Result.Failure<Address>(new Error("Address.InvalidCity", "Город должен быть от 1 до 50 символов"));

        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length > 20)
            return Result.Failure<Address>(
                new Error("Address.InvalidZipCode", "Индекс должен быть от 1 до 20 символов"));

        if (region.Length > 50)
            return Result.Failure<Address>(new Error("Address.InvalidRegion", "Регион должен быть до 50 символов"));

        if (country.Length > 50)
            return Result.Failure<Address>(new Error("Address.InvalidCountry", "Страна должна быть до 50 символов"));

        return Result.Success(new Address(
            street.Trim(),
            city.Trim(),
            zipCode.Trim(),
            region.Trim(),
            country.Trim()));
    }

    public override string ToString()
    {
        return string.Join(", ",
            new[] { Street, City, ZipCode, Region, Country }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}