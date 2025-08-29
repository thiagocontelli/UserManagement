namespace UserManagement.Models;

public class User(Guid id, string name, string lastName, int age, string address)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string LastName { get; private set; } = lastName;
    public int Age { get; private set; } = age;
    public string Address { get; private set; } = address;
    public string FullName => $"{Name} {LastName}";

    public void Validate()
    {
        if (String.IsNullOrEmpty(Name))
        {
            throw new Exception("Name is required");
        }
        if (String.IsNullOrEmpty(LastName))
        {
            throw new Exception("Last Name is required");
        }
        if (String.IsNullOrEmpty(Address))
        {
            throw new Exception("Address is required");
        }
    }
}
