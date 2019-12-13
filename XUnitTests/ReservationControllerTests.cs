using System;
using Entities.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;

namespace XUnitTests
{
    public class ReservationControllerTests
    {
        [Theory]
        [InlineData("http://localhost:5000/api/reservations/1")]
        [InlineData("http://localhost:5000/api/reservation/-1")]
        [InlineData("http://localhost:5000/api/reservation/5000")]
        [InlineData("http://localhost:5000/api/reservations/")]
        public async Task ReservationController_BadRequest(string address)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            Assert.False(apiResponse.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData("http://localhost:5000/api/reservation")]
        [InlineData("http://localhost:5000/api/reservation/1")]
        public async Task ReservationController_GoodRequest(string address)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            Assert.True(apiResponse.IsSuccessStatusCode);
        }
        
        [Theory]
        [InlineData("http://localhost:5000/api/reservation/0", "{\"id\":0,\"userId\":0,\"masterId\":0,\"timeStart\":\"0001-01-01T00:00:00\",\"timeEnd\":\"0001-01-01T00:00:00\"}")]
        [InlineData("http://localhost:5000/api/reservation/1", "{\"id\":1,\"userId\":1,\"masterId\":2,\"timeStart\":\"2019-09-30T08:00:00\",\"timeEnd\":\"2019-09-30T10:00:00\"}")]
        public async Task GetReservationByIdAsync(string address, string expected)
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.GetAsync(address);
            
            var stringResponse = await apiResponse.Content.ReadAsStringAsync();
            Assert.Equal(expected, stringResponse);
        }

        [Fact]
        public async Task PostReservation()
        {
            Reservation reservationData = new Reservation()
            {
                Id = 15,
                UserId = 1, 
                MasterId = 2,
                TimeStart = Convert.ToDateTime("2019-11-10 08:00:00"),
                TimeEnd = Convert.ToDateTime("2019-11-10 10:00:00")
            };
            var json = JsonConvert.SerializeObject(reservationData);
            var apiClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PostAsync($"http://localhost:5000/api/reservation", content);
            
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }
        
        [Fact]
        public async Task PutReservation()
        {
            Reservation reservationData = new Reservation()
            {
                Id = 15,
                UserId = 1, 
                MasterId = 2,
                TimeStart = Convert.ToDateTime("2019-11-10 08:00:00"),
                TimeEnd = Convert.ToDateTime("2019-11-10 09:00:00")
            };
            var json = JsonConvert.SerializeObject(reservationData);
            var apiClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PutAsync($"http://localhost:5000/api/reservation/15", content);
            
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, statusCode);
        }
        

        [Fact]
        public async Task DeleteReservation()
        {
            var apiClient = new HttpClient();
            var apiResponse = await apiClient.DeleteAsync($"http://localhost:5000/api/reservation/15");
            var statusCode = apiResponse.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, statusCode);
        }
    }
}