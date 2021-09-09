using AutoFixture;
using CaloriesSmartCalulator.Data;
using CaloriesSmartCalulator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CaloriesSmartCalulator.DAL.Tests
{
    public abstract class RepositoryTestsBase<T> where T : IEntity
    {
        protected readonly CaloriesCalulatorDBContext _context;
        protected IRepository<T> _repository;
        protected readonly IFixture _fixture;

        public RepositoryTestsBase()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var options = new DbContextOptionsBuilder<CaloriesCalulatorDBContext>()
                              .UseInMemoryDatabase(databaseName: "Database")
                              .Options;

            _context = new CaloriesCalulatorDBContext(options);
        }
    }
}
