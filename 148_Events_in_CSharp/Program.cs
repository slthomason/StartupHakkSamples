//Overview of Events in C#

public class AlarmClock
{
    public event Action AlarmSounded;
    public void Start()
    {
        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
        AlarmSounded?.Invoke();
    }
}

public class Person
{
    public void WakeUp() => Console.WriteLine("Waking Up!");
}

public class MainProgram
{
    public static void Main()
    {
        AlarmClock alarmClock = new AlarmClock();
        Person person = new Person();
        alarmClock.AlarmSounded += person.WakeUp;
        alarmClock.Start();
    }
}


//What is an Event Handler in C#

public class Button
{
    public event EventHandler Clicked;
    public void PressButton()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

public class User
{
    public void PerformAction()
    {
        Console.WriteLine("Action Performed!");
    }
}
public class MainProgram
{
    public static void Main()
    {
        Button button = new Button();
        User user = new User();
        button.Clicked += user.PerformAction;
        button.PressButton();
    }
}



//Real-World C# Event Example

// step 1: define a delegate
public delegate void TrafficLightChangedHandler(string color);

public class TrafficLight
{
    // step 2: declare an event
    public event TrafficLightChangedHandler TrafficLightChanged;
    public void ChangeLight(string color)
    {
        // step 3: method that raises the event
        Console.WriteLine($"The traffic light is {color}.");
        TrafficLightChanged?.Invoke(color);
    }
}
public class Car
{
    public void ReactToLight(string lightColor)
    {
        // step 4: event handling method
        if (lightColor == "Red")
        {
            Console.WriteLine("Car stops.");
        }
        else if (lightColor == "Green")
        {
            Console.WriteLine("Car starts moving.");
        }
    }
}
public class MainProgram
{
    public static void Main(string[] args)
    {
        TrafficLight light = new TrafficLight();
        Car car = new Car();
        // step 5: subscribe to the event
        light.TrafficLightChanged += car.ReactToLight;
        light.ChangeLight("Green");
        light.ChangeLight("Red");
    }
}

public class Recipient
{
    public void OnAcknowledge(string recipient)
    {
        Console.WriteLine($"Email is being sent to {recipient}");
    }
}
public class MainProgram
{
    public static void Main(string[] args)
    {
        EmailAcknowledge emailAck = new EmailAcknowledge();
        Recipient recipient = new Recipient();
        emailAck.AcknowledgeSending += recipient.OnAcknowledge;
        emailAck.Acknowledge("Bob");
        emailAck.Acknowledge("Alice");
    }
}

//Understanding C# Event Args

public class SongPlayedEventArgs : EventArgs
{
    public string PlayedSong { get; set; }
}

public class MusicPlayer
{
    public event EventHandler<SongPlayedEventArgs> SongPlayed;
    public void PlaySong(string song)
    {
        SongPlayed?.Invoke(this, new SongPlayedEventArgs() { PlayedSong = song });
    }
}

public class Listener
{
    public void OnSongPlayed(object sender, SongPlayedEventArgs e)
    {
        Console.WriteLine($"'{e.PlayedSong}' has been played.");
    }
}


//Safe Event Invocation

public class Publisher
{
    public event EventHandler<EventArgs> OnPublish;
    public void Publish()
    {
        OnPublish?.Invoke(this, EventArgs.Empty);
    }
}


//Passing Data Using Custom EventArgs


public class NewBookEventArgs : EventArgs
{
    public string Title { get; set; }
    public string Author { get; set; }
}

public class Library
{
    public event EventHandler<NewBookEventArgs> OnNewBookArrived;
    public void AddBook(string title, string author)
    {
        OnNewBookArrived?.Invoke(this, new NewBookEventArgs { Title = title, Author = author });
    }
}

public class Member
{
    public void OnNewBookArrived(object sender, NewBookEventArgs e)
    {
        Console.WriteLine($"New book arrived! Title: {e.Title}, Author: {e.Author}");
    }
}

//Unsubscribing from Events
public class Publisher
{
    public event Action OnPublish;
}

public class Subscriber
{
    public void OnPublishing()
    {
        Console.WriteLine("Something has been published!");
    }
    public void Register(Publisher publisher)
    {
        publisher.OnPublish += OnPublishing;
    }
    public void Unregister(Publisher publisher)
    {
        publisher.OnPublish -= OnPublishing;
    }
}

public static class Program
{
    public static void Main()
    {
        Publisher publisher = new Publisher();
        Subscriber subscriber = new Subscriber();
        subscriber.Register(publisher);  // Subscribing to the event
        publisher.OnPublish?.Invoke();  
        subscriber.Unregister(publisher); // Unsubscribing from the event
        publisher.OnPublish?.Invoke(); 
    }
}