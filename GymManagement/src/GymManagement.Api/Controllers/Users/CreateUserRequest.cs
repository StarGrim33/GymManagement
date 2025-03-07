﻿using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Api.Controllers.Users;

public record CreateUserRequest(FirstName FirstName,
    LastName LastName,
    Email Email,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsActive,
    Roles Role,
    Address Address);