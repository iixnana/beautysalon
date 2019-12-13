using Entities.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;

namespace XUnitTests
{
    public class UserControllerTests
    {
        [Theory]
        [InlineData("http://localhost:5000/api/users/1")]
        [InlineData("http://localhost:5000/api/user/-1")]
        [InlineData("http://localhost:5000/api/user/-1/reservations")]
        [InlineData("http://localhost:5000/api/user/-1/reservation")]
        [InlineData("http://localhost:5000/api/user/500/reservation")]
        [InlineData("http://localhost:5000/api/users/")]
        public async Task UserController_BadRequest(string address)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            Assert.False(apiResponse.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData("http://localhost:5000/api/user")]
        [InlineData("http://localhost:5000/api/user/1")]
        [InlineData("http://localhost:5000/api/user/1/reservations")]
        public async Task UserController_GoodRequest(string address)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            Assert.True(apiResponse.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData("http://localhost:5000/api/user/0", "{\"id\":0,\"creationDate\":\"0001-01-01T00:00:00\",\"userType\":0,\"firstName\":null,\"lastName\":null,\"password\":null,\"phone\":null,\"email\":null,\"token\":null}")]
        [InlineData("http://localhost:5000/api/user/1", "{\"id\":1,\"creationDate\":\"2019-09-27T00:00:00\",\"userType\":0,\"firstName\":\"Tina\",\"lastName\":\"Rose\",\"password\":\"123\",\"phone\":\"+37061111111\",\"email\":\"user@gmail.com\",\"token\":null}")]
        public async Task GetUserByIdAsync(string address, string expected)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            
            var stringResponse = await apiResponse.Content.ReadAsStringAsync();
            Assert.Equal(expected, stringResponse);
        }

        [Theory]
        [InlineData("http://localhost:5000/api/user/0/reservations", "{\"id\":0,\"creationDate\":\"0001-01-01T00:00:00\",\"userType\":0,\"firstName\":null,\"lastName\":null,\"password\":null,\"phone\":null,\"email\":null,\"token\":null,\"reservations\":[],\"timetables\":null,\"services\":null,\"articles\":null}")]
        [InlineData("http://localhost:5000/api/user/1/reservations", "{\"id\":1,\"creationDate\":\"2019-09-27T00:00:00\",\"userType\":0,\"firstName\":\"Tina\",\"lastName\":\"Rose\",\"password\":\"123\",\"phone\":\"+37061111111\",\"email\":\"user@gmail.com\",\"token\":null,\"reservations\":[{\"id\":1,\"userId\":1,\"masterId\":2,\"timeStart\":\"2019-09-30T08:00:00\",\"timeEnd\":\"2019-09-30T10:00:00\"},{\"id\":2,\"userId\":1,\"masterId\":2,\"timeStart\":\"2019-10-01T10:00:00\",\"timeEnd\":\"2019-10-01T11:00:00\"},{\"id\":3,\"userId\":1,\"masterId\":2,\"timeStart\":\"2019-10-10T10:00:00\",\"timeEnd\":\"2019-10-10T12:00:00\"}],\"timetables\":null,\"services\":null,\"articles\":null}")]
        public async Task GetUserWithDetailsAsync(string address, string expected)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            
            var stringResponse = await apiResponse.Content.ReadAsStringAsync();
            Assert.Equal(expected, stringResponse);
        }

        [Theory]
        [InlineData("First", "Last", "123", "+37099999999", "firstlast@gmail.com")]
        public async Task PostUser(string firstName, string lastName, string password, string phone, string email)
        {
            User userData = new User()
            {
                Id = 30,
                FirstName = firstName, 
                LastName = lastName,
                Password = password,
                Phone = phone,
                Email = email
            };
            var json = JsonConvert.SerializeObject(userData);
            var apiClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PostAsync($"http://localhost:5000/api/user", content);
            
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        
        [Theory]
        [InlineData("Testas", "Testas2", "123", "+37099999999", "testavimui@gmail.com", 30)]
        public async Task PutUser(string firstName, string lastName, string password, string phone, string email, int id)
        {
            User userData = new User()
            {
                FirstName = firstName, 
                LastName = lastName,
                Password = password,
                Phone = phone,
                Email = email
            };
            var json = JsonConvert.SerializeObject(userData);
            var apiClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PutAsync($"http://localhost:5000/api/user/"+id, content);
            
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.DeleteAsync($"http://localhost:5000/api/user/30");
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, statusCode);
        }
    }
}