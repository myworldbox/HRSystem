using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static HRSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using HRSystem.Domain.Entities;
using HRSystem.Infrastructure.Repositories;
using HRSystem.Application.ViewModels;

namespace HRSystem.Web.Controllers;

[Authorize]
public class StaffController : Controller
{
    private readonly ILogger<StaffController> _logger;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    [TempData]
    public string? _dialog { get; set; }

    public StaffController(
        ILogger<StaffController> logger,
        IStaffRepository staffRepository,
        IMapper mapper)
    {
        _logger = logger;
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        DialogDto? dialogViewModel = !string.IsNullOrEmpty(_dialog)
            ? JsonSerializer.Deserialize<DialogDto>(_dialog)
            : null;

        var staff = await _staffRepository.GetAllAsync();
        var data = _mapper.Map<IEnumerable<StaffViewModel>>(staff);
        var tuple = (data, dialogViewModel);
        return View(tuple);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var staff = await _staffRepository.GetByIdAsync(id.Value);
        if (staff == null) return NotFound();

        var viewModel = _mapper.Map<StaffViewModel>(staff);
        return View(viewModel);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StaffViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            var staff = _mapper.Map<Staff>(viewModel);
            await _staffRepository.AddAsync(staff);
            await _staffRepository.SaveChangesAsync();
            _logger.LogInformation($"Created staff {staff.StaffNo}");
            var dialogViewModel = new DialogDto { dialog = Dialog.primary, message = $"Created staff {staff.StaffNo}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating staff");
            var dialogViewModel = new DialogDto { dialog = Dialog.danger, message = $"Error:\n\n{ex.InnerException}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            return View(viewModel);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var staff = await _staffRepository.GetByIdAsync(id.Value);
        if (staff == null) return NotFound();

        var viewModel = _mapper.Map<StaffViewModel>(staff);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StaffViewModel viewModel)
    {
        if (id != viewModel.StaffNo) return NotFound();
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null) return NotFound();

            _mapper.Map(viewModel, staff);
            await _staffRepository.UpdateAsync(staff);
            await _staffRepository.SaveChangesAsync();
            _logger.LogInformation($"Edited staff {id}");
            var dialogViewModel = new DialogDto { dialog = Dialog.primary, message = $"Edited staff {staff.StaffNo}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing staff");
            var dialogViewModel = new DialogDto { dialog = Dialog.danger, message = $"Error:\n\n{ex.InnerException}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            return View(viewModel);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var staff = await _staffRepository.GetByIdAsync(id.Value);
        if (staff == null) return NotFound();

        var viewModel = _mapper.Map<StaffViewModel>(staff);
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null) return NotFound();

            await _staffRepository.DeleteAsync(id);
            await _staffRepository.SaveChangesAsync();
            _logger.LogInformation($"Deleted staff {staff.StaffNo}");
            var dialogViewModel = new DialogDto { dialog = Dialog.primary, message = $"Deleted staff {staff.StaffNo}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting staff");
            var dialogViewModel = new DialogDto { dialog = Dialog.danger, message = $"Error:\n\n{ex.InnerException}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
        }
        return RedirectToAction(nameof(Index));
    }
}