using AutoTask.Domain.Model;

namespace AutoTask.UI.MVVM.Model.Interface
{
    public interface IAccount
    {
        public User User { get; set; }
        public bool IsLoggedIn { get; set; }
        public string JwtToken { get; set; }
    }
}
