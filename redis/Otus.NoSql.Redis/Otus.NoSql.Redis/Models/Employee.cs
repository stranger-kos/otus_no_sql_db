namespace Otus.NoSql.Redis.Models
{
    internal class Employee
    {
        private static Random _random = new Random();

        public long Id { get; set; }

        public int Salary { get; set; }

        public static Employee Get(long id)
        {
            return new Employee
            {
                Id = id,
                Salary = _random.Next()
            };
        }
    }
}
