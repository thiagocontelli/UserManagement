namespace UserManagement.Models;

public class User(int id, string name, string lastName, int age, string address)
{
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string LastName { get; private set; } = lastName;
    public int Age { get; private set; } = age;
    public string Address { get; private set; } = address;
    public string FullName => $"{Name} {LastName}";
}
