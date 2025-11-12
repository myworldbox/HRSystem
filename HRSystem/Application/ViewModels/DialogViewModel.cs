using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static HRSystem.Domain.Enums;
using static HRSystem.Application.Helpers.ValidationHelper;

namespace HRSystem.Application.ViewModels;

public record DialogViewModel
{
    public Dialog dialog { get; set; }
    public string message { get; set; }
}
