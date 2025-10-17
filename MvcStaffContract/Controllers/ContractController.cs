using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MvcStaffContract.Models;
using MvcStaffContract.Repositories;
using MvcStaffContract.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using static MvcStaffContract.Cores.Enums;

namespace MvcStaffContract.Controllers;

[OutputCache(PolicyName = "OutputCacheWithVaryByQuery")]
public class ContractController : Controller
{
    private readonly ILogger<ContractController> _logger;
    private readonly IContractRepository _contractRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;

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
    public async Task<IActionResult> Index(DialogViewModel? dialogViewModel, string searchString = "", int page = 1)
    {
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
            var dialogViewModel = new DialogViewModel { dialog = Dialog.info, message = $"Created contract {contract.ContractNumber}" };
            return RedirectToAction(nameof(Index), dialogViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contract");
            ModelState.AddModelError("", "Unable to save changes.");
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
            return View("Edit", viewModel);
        }

        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) return NotFound();

            _mapper.Map(viewModel, contract);
            await _contractRepository.UpdateAsync(contract);
            await _contractRepository.SaveChangesAsync();
            var dialogViewModel = new DialogViewModel { dialog = Dialog.info, message = $"Edited contract {contract.ContractNumber}" };
            return RedirectToAction(nameof(Index), dialogViewModel);
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
        try {

        }
        catch (Exception e) {
        }
        var contract = await _contractRepository.GetByIdAsync(id);
        await _contractRepository.DeleteAsync(id);
        await _contractRepository.SaveChangesAsync();
        var dialogViewModel = new DialogViewModel { dialog = Dialog.info, message = $"Deleted contract {contract.ContractNumber}" };
        return RedirectToAction(nameof(Index), dialogViewModel);
    }
}