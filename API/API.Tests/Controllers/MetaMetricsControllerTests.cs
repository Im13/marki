// using API.Controllers.Admin;
// using API.DTOs;
// using API.Tests.TestData;
// using AutoMapper;
// using Core.Interfaces;
// using Microsoft.AspNetCore.Mvc;
// using Moq;

// namespace API.Tests.Controllers
// {
//     public class MetaMetricsControllerTests
//     {
//         private readonly Mock<IFacebookMarketingService> _mockFacebookService;
//         private readonly Mock<IMapper> _mockMapper;
//         private readonly MetaMetricsController _controller;

//         public MetaMetricsControllerTests()
//         {
//             _mockFacebookService = new Mock<IFacebookMarketingService>();
//             _mockMapper = new Mock<IMapper>();
//             _controller = new MetaMetricsController(_mockFacebookService.Object, _mockMapper.Object);
//         }

//         [Fact]
//         public async Task GetCampaignsWithAdsets_ValidDates_ReturnsOkResult()
//         {
//             // Arrange
//             var since = "2025-07-01";
//             var until = "2025-07-31";
//             var mockData = MockCampaignData.GetSampleCampaigns();
//             var expectedDto = new List<CampaignWithAdsetsDTO>();

//             _mockFacebookService
//                 .Setup(x => x.GetActiveCampaignsWithAdsetsInsightsAsync(
//                     It.IsAny<DateTime>(), 
//                     It.IsAny<DateTime>()))
//                 .ReturnsAsync(mockData);

//             _mockMapper
//                 .Setup(x => x.Map<List<CampaignWithAdsetsDTO>>(mockData))
//                 .Returns(expectedDto);

//             // Act
//             var result = await _controller.GetCampaignsWithAdsets(since, until);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result.Result);
//             var returnValue = Assert.IsType<List<CampaignWithAdsetsDTO>>(okResult.Value);
//             Assert.Equal(expectedDto, returnValue);
//         }

//         [Theory]
//         [InlineData("invalid-date", "2025-07-31")]
//         [InlineData("2025-07-01", "invalid-date")]
//         public async Task GetCampaignsWithAdsets_InvalidDates_ReturnsBadRequest(string since, string until)
//         {
//             // Act
//             var result = await _controller.GetCampaignsWithAdsets(since, until);

//             // Assert
//             Assert.IsType<BadRequestObjectResult>(result.Result);
//         }
//     }
// }