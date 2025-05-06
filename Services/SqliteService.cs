namespace PPMGChronicalWebAsm.Services
{
    using Microsoft.JSInterop;
    using PPMGChronicalWebAsm.Models;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class SqliteService
    {
        private readonly IJSRuntime _jsRuntime;

        public SqliteService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string[][]> ExecuteQueryAsync(string query)
        {
            try
            {
                Console.WriteLine($"Executing query: {query}");

                // Get raw JSON data first
                object rawResult = await _jsRuntime.InvokeAsync<object>("executeSqlQuery", query);
                Console.WriteLine($"Raw result type: {rawResult?.GetType().FullName}");

                // Convert the raw data to string[][]
                if (rawResult == null)
                    return Array.Empty<string[]>();

                // Use JsonSerializer to safely convert between JSON and .NET types
                string jsonString = JsonSerializer.Serialize(rawResult);
                Console.WriteLine($"JSON result: {jsonString}");

                string[][] typedResult = JsonSerializer.Deserialize<string[][]>(jsonString);
                return typedResult ?? Array.Empty<string[]>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Query execution error: {ex}");
                throw new Exception($"Query execution failed: {ex.Message}", ex);
            }
        }

        public async Task InitializeDatabaseAsync(string dbPath)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("initSqlDb", dbPath);
                Console.WriteLine("Database initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex}");
                throw new Exception($"Failed to initialize database: {ex.Message}", ex);
            }
        }

        public async Task<string[][]> GetColumnNames(string table)
        {
            try
            {
                Console.WriteLine($"Getting column names for table: {table}");

                // Get raw JSON data first
                Object rawResult = await _jsRuntime.InvokeAsync<Object>("getColumnNames", table);
                Console.WriteLine($"Raw column data type: {rawResult?.GetType().FullName}");

                // Convert the raw data to string[]
                if (rawResult == null)
                    return Array.Empty<string[]>();

                // Use JsonSerializer to safely convert between JSON and .NET types
                string jsonString = JsonSerializer.Serialize(rawResult);
                Console.WriteLine($"JSON column data: {jsonString}");

                string[][] typedResult = JsonSerializer.Deserialize<string[][]>(jsonString);
                return typedResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get column names error: {ex}");
                throw new Exception($"Failed to get column names: {ex.Message}", ex);
            }
        }

        public async Task CreateCompetitionAsync(Competition competition)
        {
            await _jsRuntime.InvokeVoidAsync("createCompetition", competition);
        }

        public async Task<string[][]> ReadCompetitionsAsync()
        {
            return await _jsRuntime.InvokeAsync<string[][]>("readCompetitions");
        }

        public async Task<List<Competition>> GetCompetitionsAsync()
        {
            var rawResults = await ReadCompetitionsAsync();
            Console.WriteLine(rawResults);

            // Map string[][] to List<Competition>
            var competitions = rawResults.Select(row => new Competition
            {
                id = row[6],
                name = row[0],
                description = row[1],
                subject = row[2],
                period_from = row[3],
                period_to = row[4],
                tag = row[5],
                school_year = row[7]
            }).ToList();

            return competitions;
        }


        public async Task UpdateCompetitionAsync(Competition competition)
        {
            await _jsRuntime.InvokeVoidAsync("updateCompetition", competition);
        }

        public async Task DeleteCompetitionAsync(int id)
        {
            await _jsRuntime.InvokeVoidAsync("deleteCompetition", id);
        }

    }
}
