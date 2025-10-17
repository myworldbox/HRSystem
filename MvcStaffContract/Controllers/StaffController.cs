using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MvcStaffContract.Controllers;
using MvcStaffContract.Models;
using MvcStaffContract.Repositories;
using MvcStaffContract.ViewModels;
using static MvcStaffContract.Cores.Enums;

public class StaffController : Controller
{
    private readonly ILogger<StaffController> _logger;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    public StaffController(
        ILogger<StaffController> logger,
        IStaffRepository staffRepository,
        IMapper mapper)
    {
        _logger = logger;
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(DialogViewModel? dialogViewModel)
    {
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

        var viewModels = _mapper.Map<StaffViewModel>(staff);
        return View(viewModels);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StaffViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);
        var staff = _mapper.Map<StaffModel>(viewModel);

        await _staffRepository.AddAsync(staff);
        await _staffRepository.SaveChangesAsync();
        var dialogViewModel = new DialogViewModel { dialog = Dialog.info, message = $"Created staff {staff.StaffNo}" };
        return RedirectToAction(nameof(Index), dialogViewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var staff = await _staffRepository.GetByIdAsync(id.Value);
        if (staff == null) return NotFound();

        var viewModels = _mapper.Map<StaffViewModel>(staff);
        return View(viewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StaffViewModel viewModel)
    {
        var staff = _mapper.Map<StaffModel>(viewModel);
        if (id != staff.StaffNo) return NotFound();
        if (!ModelState.IsValid) return View(viewModel);

        await _staffRepository.UpdateAsync(staff);
        await _staffRepository.SaveChangesAsync();
        var dialogViewModel = new DialogViewModel { dialog = Dialog.info, message = $"Edited staff {id}" };
        return RedirectToAction(nameof(Index), dialogViewModel);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var staff = await _staffRepository.GetByIdAsync(id.Value);
        if (staff == null) return NotFound();

        var viewModels = _mapper.Map<StaffViewModel>(staff);
        return View(viewModels);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _staffRepository.DeleteAsync(id);
        await _staffRepository.SaveChangesAsync();
        var dialogViewModel = new DialogViewModel { dialog = Dialog.info, message = $"Deleted staff {id}" };
        return RedirectToAction(nameof(Index), dialogViewModel);
    }
}
