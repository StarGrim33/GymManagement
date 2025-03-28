﻿using FluentValidation;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.Create.Validators;

public class ScheduleValidator : AbstractValidator<Schedule>
{
    public ScheduleValidator()
    {
        RuleFor(s => s.Value)
            .NotEmpty().WithMessage("Schedule is required.");
    }
}