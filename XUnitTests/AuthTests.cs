using System;
using Entities.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Moq;
using Xunit;
using Newtonsoft.Json;

namespace XUnitTests
{
    public class AuthTests
    {
        private AuthData _good;
        private AuthData _bad;
        
        public AuthTests()
        {
            _good = new AuthData()
            {
                Email =  "user@gmail.com",
                Password = "123"
            };
            _bad = new AuthData()
            {
                Email =  "donotexist@gmail.com",
                Password = "0"
            };
        }
        
        [Fact]
        public async Task PostAuth()
        {
            var json = JsonConvert.SerializeObject(_good);
            var apiClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PostAsync($"http://localhost:5000/api/auth/request", content);
            
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        
        [Fact]
        public void GetToken()
        {
            var mockAuth = new Mock<IAuthService>();
            string token;
            mockAuth.Setup(x => x.IsAuthenticated(It.IsAny<AuthData>(), out token)).Returns(true);
            var value = mockAuth.Object.IsAuthenticated(_good, out token);
            Assert.True(value);
        }
        
        [Fact]
        public void FailGetToken()
        {
            var mockAuth = new Mock<IAuthService>();
            string token;
            mockAuth.Setup(x => x.IsAuthenticated(It.IsAny<AuthData>(), out token));
            var value = mockAuth.Object.IsAuthenticated(_bad, out token);
            Assert.False(value);
        }
    }
}