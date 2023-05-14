﻿using FluentValidation;
using FSCode.Application.DTOs.ReminderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.Validators.ReminderValidators
{
    public class ReminderEditDtoValidator : AbstractValidator<ReminderEditDto>
    {
        public ReminderEditDtoValidator()
        {

            RuleFor(x => x.Id)
             .NotEmpty().NotNull();

            RuleFor(x => x.Id)
             .NotEmpty().NotNull();

            RuleFor(x => x.SendAt)
                .NotEmpty().NotNull()
                .Must(x => x.Ticks > DateTime.UtcNow.AddHours(4).Ticks);

            RuleFor(x => x.Content)
             .NotEmpty().NotNull()
             .MaximumLength(500);

            RuleFor(x => x.Method)
             .NotEmpty().NotNull();
        }
    }
}
