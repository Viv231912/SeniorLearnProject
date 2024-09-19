using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;

namespace UnitTestProject
{
    public class TestDatabaseFixture : IDisposable
    {
        //set up connection string 
        private const string ConnectionString = "Data Source=VIV\\SQLEXPRESS;Initial Catalog=SeniorLearn.WebApp;Integrated Security=True;Trust Server Certificate=True ";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;
        private ApplicationDbContext? _context;
        
        public TestDatabaseFixture()
        {
            lock (_lock) 
            {
                if (!_databaseInitialized) 
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureCreated();   
                    }
                    _databaseInitialized = true;
                }
            }
        }
        //Creat new db context
        public ApplicationDbContext CreateContext() 
        {
            //dispose if already created
            _context?.Dispose();
            //create a new instance and store in the private variable
            _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(ConnectionString).Options);
            return _context;      
        }
        public void Dispose()
        {
            //Dispose context when done
            _context?.Dispose();
        }
    }


}