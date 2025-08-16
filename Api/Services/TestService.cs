using Api.Dto;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Caching.Hybrid;

namespace Api.Services;

public class TestService : ITestService
{
    private readonly List<Data> _datastore = new();

    public TestService() {}

    public async Task<Data> CreateDataAsync(DataDto data)
    {
        var newData = new Data
        {
            Id = Guid.NewGuid(),
            DataValue = data.DataValue
        };
        _datastore.Add(newData);

        return await Task.FromResult(newData);
    }

    public async Task<ReadDataDto?> GetDataAsync(Guid id)
    {
        var data = _datastore.FirstOrDefault(d => d.Id == id);
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
        var data = _datastore.Select(_ => new ReadDataDto
        {
            Id = _.Id,
            DataValue = _.DataValue
        }).ToList();

        return await Task.FromResult(data);
    }

    public async Task<Data?> UpdateDataAsync(Guid id, DataDto data)
    {
        var ogData = _datastore.FirstOrDefault(d => d.Id == id);
        if (ogData == null)
        {
            return null;
        }

        ogData.DataValue = data.DataValue;

        return await Task.FromResult(ogData);
    }

    public async Task<Data?> DeleteDataAsync(Guid id)
    {
        var data = _datastore.FirstOrDefault(d => d.Id == id);

        if (data == null)
        {
            return null;
        }

        _datastore.Remove(data);
        return await Task.FromResult(data);
    }

}