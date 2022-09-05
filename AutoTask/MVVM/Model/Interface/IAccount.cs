using AutoTask.Domain.Model;

namespace AutoTask.UI.MVVM.Model.Interface
{
    public interface IAccount
    {
        public User User { get; set; }
        public bool IsLoggedIn { get; set; }
        public string JwtToken { get; set; }

        /// <summary>
        /// Logs out current user
        /// </summary>
        public void LogOut();

        /// <summary>
        /// Logs in current user
        /// </summary>
        /// <param name="email">Email of current user</param>
        /// <param name="password">Password of current user</param>
        public void LogIn(string email, string password);

        /// <summary>
        /// Updates information about current user
        /// </summary>
        /// <param name="editedUser">User that contains new information</param>
        public void UpdateInfo(User editedUser);

        /// <summary>
        /// Deletes current user
        /// </summary>
        public void DeleteAccount();
    }
}
