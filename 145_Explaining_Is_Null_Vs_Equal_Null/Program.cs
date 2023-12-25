public static class PersonExtensions
{
    public static string FullName(this Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));

        return $"{person.GivenName} {person.Surname}";
    }
}

//with pattern matching
public static class PersonExtensions
{
    public static string FullName(this Person person)
    {
        if (person is null) throw new ArgumentNullException(nameof(person));

        return $"{person.GivenName} {person.Surname}";
    }
}


public class Person
{
    public string GivenName { get; set; }

    public string Surname { get; set; }

    public static bool operator ==(Person left, Person right) => false;

    public static bool operator !=(Person left, Person right) => !(left == right);
}

public static class PersonExtensions
{
    public static string FullName(this Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));

        return $"{person.GivenName} {person.Surname}";
    }
}

public static class PersonExtensions
{
    public static string FullName(this Person person)
    {
        if (ReferenceEquals(person, null)) throw new ArgumentNullException(nameof(person));

        return $"{person.GivenName} {person.Surname}";
    }
}

public class Person
{
    public string GivenName { get; set; }

    public string Surname { get; set; }

    public static bool operator ==(Person left, Person right) => false;

    public static bool operator !=(Person left, Person right) => !(left == right);
}

public static class PersonExtensions
{
    public static string FullName(this Person person)
    {
        if (person is null) throw new ArgumentNullException(nameof(person));

        return $"{person.GivenName} {person.Surname}";
    }
}