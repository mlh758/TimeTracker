﻿using System.Net.Http.Json;
using TimeTrack.Shared;
using TimeTrack.Shared.ViewModels;
namespace TimeTrack.Client.Services
{
    public class Auth : IAuth
    {
        private readonly HttpClient _httpClient;
        public Auth(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Login(LoginForm form)
        {
            var result = await _httpClient.PostAsJsonAsync("login", form);
            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegistrationForm form)
        {
            var result = await _httpClient.PostAsJsonAsync("registration", form);
            result.EnsureSuccessStatusCode();
        }

        public Task<User> CurrentUser()
        {
            return _httpClient.GetFromJsonAsync<User>("login");
        }
    }
}
