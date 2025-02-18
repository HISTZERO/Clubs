using System;
using System.Threading.Tasks;
using AutoMapper;
using Clubs.DTOs;
using Clubs.Repositories;
using Clubs.Services;
using Clubs.Models;
using Moq;
using Xunit;

public class ClubServiceTests
{
    private readonly ClubService _clubService;
    private readonly Mock<IClubRepository> _clubRepositoryMock;
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ClubServiceTests()
    {
        _clubRepositoryMock = new Mock<IClubRepository>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _mapperMock = new Mock<IMapper>();
        _clubService = new ClubService(_clubRepositoryMock.Object, _playerRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetClubByIdAsync_ClubExists_ReturnsClubResponseDto()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var club = new Club { Id = clubId, Name = "Test Club" };
        var clubResponseDto = new ClubResponseDto { Id = clubId, Name = "Test Club" };

        _clubRepositoryMock.Setup(repo => repo.GetClubByIdAsync(clubId)).ReturnsAsync(club);
        _mapperMock.Setup(mapper => mapper.Map<ClubResponseDto>(club)).Returns(clubResponseDto);

        // Act
        var result = await _clubService.GetClubByIdAsync(clubId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clubId, result.Id);
        Assert.Equal("Test Club", result.Name);
    }

    [Fact]
    public async Task GetClubByIdAsync_ClubDoesNotExist_ReturnsNull()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        _clubRepositoryMock.Setup(repo => repo.GetClubByIdAsync(clubId)).ReturnsAsync((Club)null);

        // Act
        var result = await _clubService.GetClubByIdAsync(clubId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateClubAsync_ClubNameExists_ThrowsException()
    {
        // Arrange
        var clubRequestDto = new ClubRequestDto { Name = "Existing Club" };
        _clubRepositoryMock.Setup(repo => repo.ClubExistsAsync(clubRequestDto.Name)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clubService.CreateClubAsync(clubRequestDto, 1));
    }

    [Fact]
    public async Task CreateClubAsync_PlayerNotFound_ThrowsException()
    {
        // Arrange
        var clubRequestDto = new ClubRequestDto { Name = "New Club" };
        _clubRepositoryMock.Setup(repo => repo.ClubExistsAsync(clubRequestDto.Name)).ReturnsAsync(false);
        _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync((Player)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clubService.CreateClubAsync(clubRequestDto, 1));
    }

    [Fact]
    public async Task CreateClubAsync_ValidRequest_ReturnsClubResponseDto()
    {
        // Arrange
        var clubRequestDto = new ClubRequestDto { Name = "New Club" };
        var player = new Player { Id = 1, Name = "Player 1" };
        var club = new Club { Id = Guid.NewGuid(), Name = "New Club", Members = new List<Player> { player } };
        var clubResponseDto = new ClubResponseDto { Id = club.Id, Name = "New Club", Members = new List<PlayerResponseDto> { new PlayerResponseDto { Id = 1 } } };

        _clubRepositoryMock.Setup(repo => repo.ClubExistsAsync(clubRequestDto.Name)).ReturnsAsync(false);
        _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
        _mapperMock.Setup(mapper => mapper.Map<Club>(clubRequestDto)).Returns(club);
        _clubRepositoryMock.Setup(repo => repo.CreateClubAsync(club)).ReturnsAsync(club);
        _mapperMock.Setup(mapper => mapper.Map<ClubResponseDto>(club)).Returns(clubResponseDto);

        // Act
        var result = await _clubService.CreateClubAsync(clubRequestDto, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(club.Id, result.Id);
        Assert.Equal("New Club", result.Name);
        Assert.Single(result.Members);
        Assert.Equal(1, result.Members[0].Id);
    }

    [Fact]
    public async Task AddMemberAsync_ClubNotFound_ThrowsException()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        _clubRepositoryMock.Setup(repo => repo.GetClubByIdAsync(clubId)).ReturnsAsync((Club)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clubService.AddMemberAsync(clubId, 1));
    }

    [Fact]
    public async Task AddMemberAsync_PlayerAlreadyMember_ThrowsException()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var player = new Player { Id = 1, Name = "Player 1" };
        var club = new Club { Id = clubId, Name = "Test Club", Members = new List<Player> { player } };

        _clubRepositoryMock.Setup(repo => repo.GetClubByIdAsync(clubId)).ReturnsAsync(club);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clubService.AddMemberAsync(clubId, 1));
    }

    [Fact]
    public async Task AddMemberAsync_PlayerNotFound_ThrowsException()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var club = new Club { Id = clubId, Name = "Test Club", Members = new List<Player>() };

        _clubRepositoryMock.Setup(repo => repo.GetClubByIdAsync(clubId)).ReturnsAsync(club);
        _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync((Player)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clubService.AddMemberAsync(clubId, 1));
    }

    [Fact]
    public async Task AddMemberAsync_ValidRequest_AddsMember()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var player = new Player { Id = 1, Name = "Player 1" };
        var club = new Club { Id = clubId, Name = "Test Club", Members = new List<Player>() };

        _clubRepositoryMock.Setup(repo => repo.GetClubByIdAsync(clubId)).ReturnsAsync(club);
        _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);

        // Act
        await _clubService.AddMemberAsync(clubId, 1);

        // Assert
        _clubRepositoryMock.Verify(repo => repo.AddMemberAsync(clubId, player), Times.Once);
    }
}
