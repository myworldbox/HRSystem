using AutoMapper;
using HRSystem.Application.Dtos;
using HRSystem.Application.Interfaces;
using HRSystem.Domain.Entities;
using HRSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using static HRSystem.Domain.Enums;

namespace HRSystem.Application.Services;
public class StaffService : IStaffService
{
    private readonly IStaffRepository _repo;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public StaffService(IStaffRepository repo, UserManager<IdentityUser> userManager, IMapper mapper)
        => (_repo, _userManager, _mapper) = (repo, userManager, mapper);

    public async Task<IEnumerable<StaffDto>> GetAccessibleStaffAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var claims = await _userManager.GetClaimsAsync(user);
        var accessibleNos = claims.Where(c => c.Type == "AccessStaff").Select(c => int.Parse(c.Value));
        var staff = await _repo.GetAllAsync();
        var staffs = staff.Where(s => accessibleNos.Contains(s.StaffNo)).Select(s => s);
        return _mapper.Map<IEnumerable<StaffDto>>(staffs);
    }

    public async Task<StaffDto> GetStaffByNoAsync(int staffNo)
        => _mapper.Map<StaffDto>(await _repo.GetByIdAsync(staffNo));

    public async Task CreateStaffAsync(StaffDto dto)
    {
        var staff = _mapper.Map<Staff>(dto);
        await _repo.AddAsync(staff);
        await _repo.SaveChangesAsync();
    }

    public async Task UpdateStaffAsync(StaffDto dto)
    {
        var staff = await _repo.GetByIdAsync(dto.StaffNo);
        if (staff != null)
        {
            var newStaff = _mapper.Map<Staff>(dto);
            await _repo.UpdateAsync(newStaff);
            await _repo.SaveChangesAsync();
        }
    }

    public async Task<bool> ConfirmStaffAsync(int staffNo, string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (!(await _userManager.GetClaimsAsync(user)).Any(c => c.Type == "AccessStaff" && c.Value == staffNo.ToString())) return false;
        var staff = await _repo.GetByIdAsync(staffNo);
        if (staff == null) return false;
        staff.Confirm();
        await _repo.UpdateAsync(staff);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteStaffAsync(int staffNo, string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (!(await _userManager.GetClaimsAsync(user)).Any(c => c.Type == "AccessStaff" && c.Value == staffNo.ToString())) return false;
        var staff = await _repo.GetByIdAsync(staffNo);
        if (staff == null) return false;
        staff.Delete();
        await _repo.UpdateAsync(staff);
        await _repo.SaveChangesAsync();
        return true;
    }
}