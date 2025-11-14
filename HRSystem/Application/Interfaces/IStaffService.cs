using HRSystem.Application.Dtos;

namespace HRSystem.Application.Interfaces;
public interface IStaffService
{
    Task<IEnumerable<StaffDto>> GetAccessibleStaffAsync(string username);
    Task<StaffDto> GetStaffByNoAsync(int staffNo);
    Task CreateStaffAsync(StaffDto dto);
    Task UpdateStaffAsync(StaffDto dto);
    Task<bool> ConfirmStaffAsync(int staffNo, string username);
    Task<bool> DeleteStaffAsync(int staffNo, string username);
}