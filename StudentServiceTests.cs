using Xunit;
using StudentCrudApi.Models;
using StudentCrudApi.Services;
using System.Linq;

namespace StudentCrudApi.Tests.Services
{
    public class StudentServiceTests
    {
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            // Reset static list before each test (optional in your implementation)
            typeof(StudentService)
                .GetField("_students", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new System.Collections.Generic.List<Student>());

            _service = new StudentService();
        }

        [Fact]
        public void Add_ShouldAssignId_AndAddStudent()
        {
            var student = new Student { Name = "Alice", School = "Alpha School" };

            var result = _service.Add(student);

            Assert.Equal(1, result.Id);
            Assert.Single(_service.GetAll());
        }

        [Fact]
        public void GetAll_ShouldReturnAllStudents()
        {
            _service.Add(new Student { Name = "A", School = "S1" });
            _service.Add(new Student { Name = "B", School = "S2" });

            var students = _service.GetAll();

            Assert.Equal(2, students.Count());
        }

        [Fact]
        public void GetById_ShouldReturnCorrectStudent()
        {
            var student = _service.Add(new Student { Name = "A", School = "S1" });

            var result = _service.GetById(student.Id);

            Assert.Equal(student.Name, result.Name);
        }

        [Fact]
        public void Update_ShouldModifyStudent_IfExists()
        {
            var student = _service.Add(new Student { Name = "Old", School = "OldSchool" });
            var updated = new Student { Name = "New", School = "NewSchool" };

            var success = _service.Update(student.Id, updated);
            var result = _service.GetById(student.Id);

            Assert.True(success);
            Assert.Equal("New", result.Name);
        }

        [Fact]
        public void Update_ShouldReturnFalse_IfNotExists()
        {
            var updated = new Student { Name = "New", School = "NewSchool" };
            var success = _service.Update(999, updated);

            Assert.False(success);
        }

        [Fact]
        public void Delete_ShouldRemoveStudent()
        {
            var student = _service.Add(new Student { Name = "ToDelete", School = "X" });

            var success = _service.Delete(student.Id);

            Assert.True(success);
            Assert.Null(_service.GetById(student.Id));
        }

        [Fact]
        public void Delete_ShouldReturnFalse_IfNotExists()
        {
            var success = _service.Delete(999);

            Assert.False(success);
        }
    }
}
