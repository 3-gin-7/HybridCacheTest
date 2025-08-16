using Api.Dto;
using Api.Models;

namespace Api.Interfaces;

public interface ITestService
{
    Task<Data> CreateDataAsync(DataDto data);
    Task<ReadDataDto?> GetDataAsync(Guid id);
    Task<List<ReadDataDto>> GetAllDataAsync();
    Task<Data?> UpdateDataAsync(Guid id, DataDto data);
    Task<Data?> DeleteDataAsync(Guid id);
}