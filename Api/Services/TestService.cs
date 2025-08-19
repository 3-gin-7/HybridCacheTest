using Api.Context;
using Api.Dto;
using Api.Interfaces;
using Api.Models;

namespace Api.Services;

public class TestService : ITestService
{
    private readonly AppDbContext _context;

    public TestService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Data> CreateDataAsync(DataDto data)
    {
        var newData = new Data
        {
            Id = Guid.NewGuid(),
            DataValue = data.DataValue
        };
        _context.DataEntries.Add(newData);
        await _context.SaveChangesAsync();

        return await Task.FromResult(newData);
    }

    public async Task<ReadDataDto?> GetDataAsync(Guid id)
    {
        var data = _context.DataEntries.FirstOrDefault(_ => _.Id == id);
        if (data == null)
        {
            return null;
        }

        return await Task.FromResult(new ReadDataDto()
        {
            Id = data.Id,
            DataValue = data.DataValue
        });
    }

    public async Task<List<ReadDataDto>> GetAllDataAsync()
    {
        var data = _context.DataEntries.Select(_ => new ReadDataDto
        {
            Id = _.Id,
            DataValue = _.DataValue
        }).ToList();

        return await Task.FromResult(data);
    }

    public async Task<Data?> UpdateDataAsync(Guid id, DataDto data)
    {
        var ogData = _context.DataEntries.FirstOrDefault(d => d.Id == id);
        if (ogData == null)
        {
            return null;
        }

        ogData.DataValue = data.DataValue;

        return await Task.FromResult(ogData);
    }

    public async Task<Data?> DeleteDataAsync(Guid id)
    {
        var data = _context.DataEntries.FirstOrDefault(d => d.Id == id);

        if (data == null)
        {
            return null;
        }

        _context.DataEntries.Remove(data);
        await _context.SaveChangesAsync();
        return await Task.FromResult(data);
    }

}