using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRSystem.Cores;
using HRSystem.Helpers;
using static HRSystem.Cores.Enums;
using static HRSystem.Helpers.ValidationHelper;

namespace HRSystem.ViewModels;

public record DialogViewModel
{
    public Dialog dialog { get; set; }
    public string message { get; set; }
}
