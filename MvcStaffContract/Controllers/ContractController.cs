using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MvcStaffContract.Models;
using MvcStaffContract.Repositories;
using MvcStaffContract.ViewModels;
using System.Text.Json;
using static MvcStaffContract.Cores.Enums;

namespace MvcStaffContract.Controllers;

[OutputCache(PolicyName = "OutputCacheWithVaryByQuery")]
public class ContractController : Controller
{
    private readonly ILogger<ContractController> _logger;
    private readonly IContractRepository _contractRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    [TempData]
    public string? _dialog { get; set; }

    public ContractController(
        ILogger<ContractController> logger,
        IContractRepository contractRepository,
        IStaffRepository staffRepository,
        IMapper mapper)
    {
        _logger = logger;
        _contractRepository = contractRepository;
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    [OutputCache(Duration = 60)]
    public async Task<IActionResult> Index(string searchString = "", int page = 1)
    {
        DialogViewModel? dialogViewModel = !string.IsNullOrEmpty(_dialog)
            ? JsonSerializer.Deserialize<DialogViewModel>(_dialog)
            : null;

        var query = (await _contractRepository.GetAllAsync()).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(c => c.StaffNo.ToString().Contains(searchString) || c.Position.Contains(searchString));
        }
        var contracts = query.Skip((page - 1) * 10).Take(10);

        var data = _mapper.Map<IEnumerable<ContractViewModel>>(contracts);
        var tuple = (data, dialogViewModel);
        return View(tuple);
    }

    public async Task<IActionResult> Details(int id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null) return NotFound();

        var viewModel = _mapper.Map<ContractViewModel>(contract);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
    {
        var viewModel = new ContractViewModel
        {
            StaffNo = id,
            Status = Status.ContractDraft,
            ListStaffNo = new SelectList(await _staffRepository.GetAllAsync(), "StaffNo", "StaffNo", id)
        };

        return View(viewModel);
    }


    [HttpPost, ActionName("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContractViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", viewModel);
        }

        try
        {
            var contract = _mapper.Map<ContractModel>(viewModel);
            contract.Id = null;
            await _contractRepository.AddAsync(contract);
            await _contractRepository.SaveChangesAsync();
            _logger.LogInformation($"Created contract {contract.ContractNumber}");
            var dialogViewModel = new DialogViewModel { dialog = Dialog.primary, message = $"Created contract {contract.ContractNumber}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contract");
            var dialogViewModel = new DialogViewModel { dialog = Dialog.danger, message = $"Error:\n\n{ex.InnerException}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            viewModel.ListStaffNo = new SelectList(await _staffRepository.GetAllAsync(), "StaffNo", "StaffNo", viewModel.StaffNo);
            return View("Create", viewModel);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null) return NotFound();

        var viewModel = _mapper.Map<ContractViewModel>(contract);
        viewModel.ListStaffNo = new SelectList(await _staffRepository.GetAllAsync(), "StaffNo", "StaffNo", viewModel.StaffNo);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContractViewModel viewModel)
    {
        if (id != viewModel.Id) return NotFound();
        ModelState.Remove("StartDate");
        if (!ModelState.IsValid)
        {
            viewModel.ListStaffNo = new SelectList(await _staffRepository.GetAllAsync(), "StaffNo", "StaffNo", viewModel.StaffNo);
            return View("Edit", viewModel);
        }

        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) return NotFound();

            _mapper.Map(viewModel, contract);
            await _contractRepository.UpdateAsync(contract);
            await _contractRepository.SaveChangesAsync();
            var dialogViewModel = new DialogViewModel { dialog = Dialog.primary, message = $"Edited contract {contract.ContractNumber}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _contractRepository.GetByIdAsync(viewModel.Id) == null) return NotFound();
            throw;
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null) return NotFound();

        var viewModel = _mapper.Map<ContractViewModel>(contract);
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) return NotFound();

            await _contractRepository.DeleteAsync(id);
            await _contractRepository.SaveChangesAsync();
            var dialogViewModel = new DialogViewModel { dialog = Dialog.primary, message = $"Deleted contract {contract.ContractNumber}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contract");
            var dialogViewModel = new DialogViewModel { dialog = Dialog.danger, message = $"Error:\n\n{ex.InnerException}" };
            _dialog = JsonSerializer.Serialize(dialogViewModel);
        }
        return RedirectToAction(nameof(Index));
    }
}