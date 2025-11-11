using System;
using System.Windows;
using Bakery.DAL.Entities;
using Bakery.DAL.Repositories;
using Bakery.BLLL.Services;

namespace Bakery.WPFApplication
{
    public partial class App : Application
    {
        public AuthService AuthService { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var db = new BakeryContext();
            var userRepo = new UserRepository(db);
            AuthService = new AuthService(userRepo);

            var login = new Views.LoginWindow();
            login.Show();
        }
    }
}
