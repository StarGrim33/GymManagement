﻿using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Users.CreateUser;

public record CreateUserCommand(
    FirstName FirstName,
    LastName LastName,
    Email Email,
    string Password,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsActive,
    Role Role,
    Address Address) : ICommand<Guid>;