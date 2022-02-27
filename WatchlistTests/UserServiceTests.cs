using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Watchlist.Data;
using Watchlist.Services;
using Xunit;

namespace WatchlistTests
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock = new Mock<RoleManager<IdentityRole>>();
        private readonly Mock<HttpContext> _httpContextMock = new Mock<HttpContext>();
        private readonly string userId = Guid.NewGuid().ToString();

        private readonly UserService _userService;

        public UserServiceTests()
        {


            
            _userManagerMock = new Mock<UserManager<ApplicationUser>>();
            //_userManagerMock.Setup(p => p.FindByIdAsync(It.IsAny<string>()))
            //    .ReturnsAsync(new ApplicationUser
            //    {
            //        Id = userId,
            //        UserName = "Koman.atse@directsoft.ci",
            //        Email = "Koman.atse@directsoft.ci",
            //        FirstName = "Koman",
            //        LastName = "Atsé"
            //    });


            _userService = new UserService(_userManagerMock.Object,_roleManagerMock.Object);
        }


        [Fact]
        public async void GetCurrentUserIdAsync_ShouldReturnId_WhenUserIsConnected()
        {
            //Arrange

            //Act
            var result = await _userService.GetCurrentUserIdAsync(_httpContextMock.Object);
            
            //Assert
            Assert.Equal(userId, result);
        }
    }
}
