using Gitpilot.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Repositories
{
    public abstract class BaseRepository
    {
        private readonly SQLiteAsyncConnection _database;
        public bool IsInitialized { get; set; } = false;

        public BaseRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }
        public async Task CreateTableAsync<T>() where T : new()
        {
            await _database.CreateTableAsync<T>();
        }
        public async Task<List<T>> GetItemsAsync<T>() where T : new()
        {
            await CheckToInitTables();
            return await _database.Table<T>().ToListAsync();
        }
        public async Task<int> SaveItemAsync<T>(T item) where T : new()
        {
            await CheckToInitTables();
            return await _database.InsertAsync(item);
        }
        public async Task<int> UpdateItemAsync<T>(T item) where T : new()
        {
            await CheckToInitTables();
            return await _database.UpdateAsync(item);
        }
        private async Task CheckToInitTables()
        {
            if (!IsInitialized)
            {
                await InitializeTablesAsync();
                IsInitialized = true;
            }
        }
        public abstract Task InitializeTablesAsync();
    }
}
