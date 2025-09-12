using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;

namespace GymManagement.Domain.Entities.Gyms;

public sealed class Gym : Entity
{
    private readonly List<GymAmenity> _gymAmenities = [];
    private readonly List<Trainer> _trainers = [];
    private readonly List<GymEquipment> _equipment = [];
    private readonly List<Membership> _memberships = [];
    private readonly List<TrainingSession> _trainingSessions = [];

    private Gym(
        Guid id, 
        string name, 
        string description, 
        string street,
        string city,
        string zipCode,
        string schedule) 
        : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
    }

    // Требуется для EF Core
    private Gym() { }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string Schedule { get; private set; } = string.Empty;

    public IReadOnlyList<GymAmenity> GymAmenities => _gymAmenities.AsReadOnly();
    public IReadOnlyList<Trainer> Trainers => _trainers.AsReadOnly();
    public IReadOnlyList<GymEquipment> Equipment => _equipment.AsReadOnly();
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();
    public IReadOnlyList<TrainingSession> TrainingSessions => _trainingSessions.AsReadOnly();

    public IReadOnlyCollection<Amenity> Amenities => 
        _gymAmenities.Select(ga => ga.Amenity).ToList().AsReadOnly();

    public static Result<Gym> Create(
        string name, 
        string description, 
        string street,
        string city,
        string zipCode,
        string schedule)
    {
        // Простая валидация
        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            return Result.Failure<Gym>(new Error("Gym.InvalidName", "Название должно быть от 1 до 100 символов"));
            
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Gym>(new Error("Gym.InvalidDescription", "Описание обязательно"));
            
        if (string.IsNullOrWhiteSpace(street) || street.Length > 100)
            return Result.Failure<Gym>(new Error("Gym.InvalidStreet", "Улица должна быть от 1 до 100 символов"));
            
        if (string.IsNullOrWhiteSpace(city) || city.Length > 50)
            return Result.Failure<Gym>(new Error("Gym.InvalidCity", "Город должен быть от 1 до 50 символов"));
            
        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length > 20)
            return Result.Failure<Gym>(new Error("Gym.InvalidZipCode", "Индекс должен быть от 1 до 20 символов"));
            
        if (string.IsNullOrWhiteSpace(schedule))
            return Result.Failure<Gym>(new Error("Gym.InvalidSchedule", "Расписание обязательно"));

        var gym = new Gym(
            Guid.CreateVersion7(),
            name.Trim(),
            description.Trim(),
            street.Trim(),
            city.Trim(),
            zipCode.Trim(),
            schedule.Trim());

        return Result.Success(gym);
    }

    public Result Update(
        string name,
        string description,
        string street,
        string city,
        string zipCode,
        string schedule)
    {
        // Валидация аналогична Create
        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            return Result.Failure(new Error("Gym.InvalidName", "Название должно быть от 1 до 100 символов"));
            
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(new Error("Gym.InvalidDescription", "Описание обязательно"));
            
        if (string.IsNullOrWhiteSpace(street) || street.Length > 100)
            return Result.Failure(new Error("Gym.InvalidStreet", "Улица должна быть от 1 до 100 символов"));
            
        if (string.IsNullOrWhiteSpace(city) || city.Length > 50)
            return Result.Failure(new Error("Gym.InvalidCity", "Город должен быть от 1 до 50 символов"));
            
        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length > 20)
            return Result.Failure(new Error("Gym.InvalidZipCode", "Индекс должен быть от 1 до 20 символов"));
            
        if (string.IsNullOrWhiteSpace(schedule))
            return Result.Failure(new Error("Gym.InvalidSchedule", "Расписание обязательно"));

        Name = name.Trim();
        Description = description.Trim();
        Street = street.Trim();
        City = city.Trim();
        ZipCode = zipCode.Trim();
        Schedule = schedule.Trim();

        return Result.Success();
    }

    public Result AddAmenity(GymAmenity amenity)
    {
        if (amenity == null)
            return Result.Failure(new Error("Gym.NullAmenity", "Удобство не может быть null"));

        if (_gymAmenities.Any(ga => ga.Amenity == amenity.Amenity))
            return Result.Failure(new Error("Gym.AmenityExists", "Удобство уже существует"));

        _gymAmenities.Add(amenity);
        return Result.Success();
    }

    public Result RemoveAmenity(Amenity amenity)
    {
        var gymAmenity = _gymAmenities.FirstOrDefault(ga => ga.Amenity == amenity);
        if (gymAmenity == null)
            return Result.Failure(new Error("Gym.AmenityNotFound", "Удобство не найдено"));

        _gymAmenities.Remove(gymAmenity);
        return Result.Success();
    }

    public Result AddTrainer(Trainer trainer)
    {
        if (trainer == null)
            return Result.Failure(new Error("Gym.NullTrainer", "Тренер не может быть null"));

        if (_trainers.Any(t => t.Id == trainer.Id))
            return Result.Failure(new Error("Gym.TrainerExists", "Тренер уже работает в спортзале"));

        _trainers.Add(trainer);
        return Result.Success();
    }

    public Result RemoveTrainer(Guid trainerId)
    {
        var trainer = _trainers.FirstOrDefault(t => t.Id == trainerId);
        if (trainer == null)
            return Result.Failure(new Error("Gym.TrainerNotFound", "Тренер не найден"));

        // Проверяем активные тренировки
        var hasActiveTrainingSessions = _trainingSessions
            .Any(ts => ts.TrainerId == trainerId && ts.StartTime > DateTime.UtcNow && !ts.IsCanceled);

        if (hasActiveTrainingSessions)
            return Result.Failure(new Error("Gym.TrainerHasActiveSessions", "У тренера есть активные тренировки"));

        _trainers.Remove(trainer);
        return Result.Success();
    }

    public Result AddEquipment(GymEquipment equipment)
    {
        if (equipment == null)
            return Result.Failure(new Error("Gym.NullEquipment", "Оборудование не может быть null"));

        if (_equipment.Any(e => e.Id == equipment.Id))
            return Result.Failure(new Error("Gym.EquipmentExists", "Оборудование уже существует"));

        _equipment.Add(equipment);
        return Result.Success();
    }

    public Result AddMembership(Membership membership)
    {
        if (membership == null)
            return Result.Failure(new Error("Gym.NullMembership", "Абонемент не может быть null"));

        if (_memberships.Any(m => m.Id == membership.Id))
            return Result.Failure(new Error("Gym.MembershipExists", "Абонемент уже существует"));

        _memberships.Add(membership);
        return Result.Success();
    }

    // Удобные методы для работы с данными
    public string GetFullAddress() => $"{Street}, {City}, {ZipCode}";
    
    public bool HasActiveTrainer(Guid trainerId) =>
        _trainers.Any(t => t.Id == trainerId && t.IsActive);

    public bool HasAvailableEquipment(EquipmentType equipmentType) =>
        _equipment.Any(e => e.EquipmentType == equipmentType && e.IsAvailable);

    public int GetActiveTrainersCount() =>
        _trainers.Count(t => t.IsActive);

    public int GetActiveMembershipsCount() =>
        _memberships.Count(m => m.IsActive);
}