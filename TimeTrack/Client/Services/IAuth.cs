using TimeTrack.Shared;
using TimeTrack.Shared.ViewModels;
namespace TimeTrack.Client.Services
{
    public interface IAuth
    {
        public Task Login(LoginForm model);
        public Task<User> CurrentUser();
    }
}
