public interface IMediator
{
    public void SendMessage(string message, int chatId);
    public void RegisterChatUser(IUser user);
}
public interface IUser
{
    int ChatId { get; }
    void SendMessage(string message);
    void RecieveMessage(string message);
}
public class User : IUser
{
    private readonly IMediator _mediator;
    private string _username;
    private int _chatId;
    public User(IMediator mediator, string username, int chatId)
    {
        _mediator = mediator;
        _username = username;
        _chatId = chatId;
    }

    public int ChatId => _chatId;

    public void RecieveMessage(string message)
    {
        Console.WriteLine($"{_username} отримав(-ла) повідомлення: {message}");
    }

    public void SendMessage(string message)
    {
        Console.WriteLine($"{_username} відправив(-ла) повідомлення: {message}");
        _mediator.SendMessage(message, _chatId);
    }
}
public class ChatMediator : IMediator
{
    private readonly Dictionary<int, IUser> _users;
    public ChatMediator()
    {
        _users = new Dictionary<int, IUser>();
    }

    public void RegisterChatUser(IUser user)
    {
        if (!_users.ContainsKey(user.ChatId))
        {
            _users.Add(user.ChatId, user);
        }
    }

    public void SendMessage(string message, int chatId)
    {
        foreach (var user in _users.Where(x=>x.Key != chatId))
        {
            user.Value.RecieveMessage(message);
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        var chatApp = new ChatMediator();

        var user1 = new User(chatApp, "Женя", 1);
        var user2 = new User(chatApp, "Юля", 2);
        var user3 = new User(chatApp, "Богдан", 3);

        chatApp.RegisterChatUser(user1);
        chatApp.RegisterChatUser(user2);
        chatApp.RegisterChatUser(user3);

        user1.SendMessage("Всім привіт!");
        user2.SendMessage("Давайте погуляємо сьогодні");
        user3.SendMessage("Я не проти");
        PrintTimeStamp("");
    }
    static void PrintTimeStamp(string info)
    {
        Console.WriteLine($"Author: Yevgeniya Skubiy");
        Console.WriteLine($"Time: {DateTime.Now}");
    }

}