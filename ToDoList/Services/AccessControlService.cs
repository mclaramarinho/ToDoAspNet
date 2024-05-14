using System.Diagnostics.Contracts;

namespace ToDoList.Services
{
    public interface IAccessControlService
    {
        int? LoggedUser {  get; }
        bool VerifyIfUserLoggedIn();
        bool Login(int uid);
        void Logout();

    }
    public class AccessControlService : IAccessControlService
    {
        private readonly ISession _session = new HttpContextAccessor().HttpContext.Session;
        public int? LoggedUser { get; private set; }

        public bool VerifyIfUserLoggedIn()
        {
            this.LoggedUser = _session.GetInt32("loggedUser");
            return this.LoggedUser != null;
        }
        public bool Login(int uid)
        {
            if(this.LoggedUser != null)
            {
                this.Logout();
            }

            try
            {
                _session.SetInt32("loggedUser", uid);
                this.LoggedUser = uid;
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
        public void Logout()
        {
            this.LoggedUser = null;
            _session.Remove("loggedUser");
        }

    }
}
