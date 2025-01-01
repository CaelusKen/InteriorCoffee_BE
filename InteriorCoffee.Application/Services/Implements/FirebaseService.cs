using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class FirebaseService
    {
        private readonly IConfiguration _configuration;

        public FirebaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(_configuration["Firebase:ServiceAccountKey"])
                });
            }
        }

        // Validate a user email against Firebase
        public async Task<UserRecord> GetUserByEmailAsync(string email)
        {
            try
            {
                return await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception("User not found in Firebase", ex);
            }
        }

        // Example method to get user by UID
        public async Task<UserRecord> GetUserByUidAsync(string uid)
        {
            try
            {
                return await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception("User not found in Firebase", ex);
            }
        }
    }
}
