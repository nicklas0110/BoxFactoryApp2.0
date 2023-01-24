using Moq;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using BoxFactoryApp;
using FluentValidation;

namespace TestProject1;

public class UnitTest1
{
    
    #region CreateBoxService
    
    /// <summary>
    /// Test that box service can be create with valid repository.
    /// </summary>
    [Fact]
    public void CreateBoxService_ValidBoxRepository_Test()
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();

        // Act
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Assert
        Assert.NotNull(service);
        Assert.True(service is BoxServive);
    }

    /// <summary>
    /// Test if an exception is thrown if boxRepo is null when calling BoxService Constructor.
    /// </summary>
    [Fact]
    public void CreateBoxService_BoxRepositoryIsNull_ExpectArgumentNullException_Test() 
    {
        // Arrange
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => new BoxServive(null,
            new PostBoxValidator(), new BoxValidator(), mapper));
    }
    
    #endregion // CreateBoxService
    
    #region CreateBox

     /// <summary>
     /// Checks that CreateNewBox is called when trying to create valid boxes.
     /// </summary>
     [Theory]
     [InlineData("L", "Benny", "type")]
     [InlineData("XL", "Børge", "type")]
     public void CreateBox_ValidBox_Test(string size, string customerName, string type) 
     {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration => { configuration.CreateMap<BoxDTOs, Box>(); })
         .CreateMapper();
        var service = new BoxServive(boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        var boxDTO = new BoxDTOs { Size = size, CustomerName = customerName, Type = type};

        // Act
        service.CreateNewBox(boxDTO);

        // Assert
        boxRepoMock.Verify(r => r.CreateNewBox(It.IsAny<Box>()), Times.Once);
    }

     /// <summary>
     /// Tests that we get an exception when trying to create invalid boxes. Also tests that CreateNewBox is never called on the repository.
     /// </summary>
    [Theory]
    [InlineData(null, "name", "type", "'Size' bør ikke være tom.")]       // invalid size. size == null
    [InlineData("", "name", "type", "'Size' bør ikke være tom.")]         // invalid size. size == ""
    [InlineData("size", "", "type", "'Customer Name' bør ikke være tom.")] // invalid name. name == ""
    public void CreateBox_InvalidBox_ExpectArgumentException_Test(string size, string customerName, string type, string expected)
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        var boxDTO = new BoxDTOs { Size = size, CustomerName = customerName, Type = type};

        // Act and assert
        var ex = Assert.Throws<ValidationException>(() => service.CreateNewBox(boxDTO));

        // Assert
        Assert.Equal(expected, ex.Message);
        boxRepoMock.Verify(r => r.CreateNewBox(It.IsAny<Box>()), Times.Never);
    }

    /// <summary>
    /// Test that if we pass null instead of a box that an exception is thrown. Also tests that CreateNewBox is never called on the repository.
    /// </summary>
    [Fact]
    public void CreateBox_BoxIsNull_ExpectArgumentNullException_Test()
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act and assert
        var ex = Assert.Throws<ArgumentNullException>(() => service.CreateNewBox(null));

        // Assert
        boxRepoMock.Verify(r => r.CreateNewBox(null), Times.Never);
    }

    #endregion // CreateBox
        
    #region UpdateBox

    /// <summary>
    /// Tests that UpdateBox is called on the repository once when a valid box is provided.
    /// </summary>
    [Theory]
    [InlineData(1, "newSize", "name", "type")]
    [InlineData(1, "size", "newName", "type")]
    public void UpdateBox_ValidUpdate_Test(int id, string size, string customerName, string type)
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        var box = new Box { Id = id, customerName = customerName, size = size, type = type};
        // Act
        service.UpdateBox(1, box);

        // Assert
        boxRepoMock.Verify(r => r.UpdateBox(It.IsAny<Box>()), Times.Once);
    }

    /// <summary>
    /// Tests that an exception is thrown if a box with invalid data is provided. Also tests that UpdateBox is never called on the repository.
    /// </summary>
    [Theory]
    [InlineData(0, "size", "name", "type", "'Id' skal være større end '0'.")]    // invalid id. Id <= 0
    [InlineData(1, null, "name", "type", "'size' bør ikke være tom.")]                 // invalid size. size == null
    [InlineData(1, "", "name", "type", "'size' bør ikke være tom.")]                     // invalid size. size == ""
    [InlineData(1, "size", "", "type", "'customer Name' bør ikke være tom.")]                    // invalid name. name == ""
    public void UpdateBox_InvalidUpdate_ExpectArgumentException_Test(int id, string size, string customerName, string type, string expected)
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        var box = new Box { Id = id, customerName = customerName, size = size, type = type};

        // Act and assert
        var ex = Assert.Throws<ValidationException>(() => service.UpdateBox(id, box));

        Assert.Equal(expected, ex.Message);
        boxRepoMock.Verify(r => r.UpdateBox(box), Times.Never);
    }

    /// <summary>
    /// Tests that an exception is thrown when null is passed to UpdateBox on the servie. Also tests that UpdateBox is never called on the repository.
    /// </summary>
    [Fact]
    public void UpdateBox_BoxIsNull_ExpectArgumentNullException_Test()
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act and assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateBox(1,null));

        // Assert
        Assert.Equal("Box cannot be null", ex.Message);
        boxRepoMock.Verify(r => r.UpdateBox(null), Times.Never);
    }
        
    #endregion // UpdateBox
        
    #region DeleteBox

    /// <summary>
    /// Tests that DeleteBox is called on the repository, with the given Id. We do boundary testing (We test first two, middle and last two in dataset).
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(int.MaxValue / 2)]
    [InlineData(int.MaxValue - 1)]
    [InlineData(int.MaxValue)]
    public void DeleteBox_ExistingBox_Test(int id)
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act
        service.DeleteBox(id);

        // Assert
        boxRepoMock.Verify(r => r.DeleteBox(id), Times.Once);
    }

    /// <summary>
    /// Tests that an exception is thrown when a box with the given id does not exist. Also tests that DeleteBox is never called on the repository.
    /// </summary>
    [Fact]
    public void DeleteBox_BoxDoesNotExist_ExpectArgumentException_Test()
    {
        // Arrange
        var id = 1;
        
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        boxRepoMock.Setup(r => r.GetBoxById(id)).Throws<KeyNotFoundException>();
        
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act and assert
        var ex = Assert.Throws<ArgumentException>(() => service.DeleteBox(id));

        Assert.Equal("A box with the given Id does not exist", ex.Message);
        boxRepoMock.Verify(r => r.DeleteBox(id), Times.Never);
    }

    /// <summary>
    /// Tests that if a invalid Id is passed to DeleteBox on the service an exception is thrown. Also tests that DeleteBox is never called on the repository.
    /// </summary>
    /// <param name="id"></param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void DeleteBox_IdIsInvalid_ExpectArgumentException_Test(int id)
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act and assert
        var ex = Assert.Throws<ArgumentException>(() => service.DeleteBox(id));

        // Assert
        Assert.Equal("Id cannot be 0 or below", ex.Message);
        boxRepoMock.Verify(r => r.DeleteBox(id), Times.Never);
    }

    #endregion // DeleteBox
        
    #region GetBox

    /// <summary>
    /// Tests that GetBoxById is called once on the repository, and that we get the expected result.
    /// </summary>
    [Fact]
    public void GetBox_ExistingBox_Test()
    {
        // Arrange
        var box = new Box { Id = 1, customerName = "customerName", size = "size", type = "type"};

        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        boxRepoMock.Setup(r => r.GetBoxById(1)).Returns(box);
        
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act
        var result = service.GetBoxById(box.Id);

        // Assert
        Assert.Equal(box, result);
        boxRepoMock.Verify(r => r.GetBoxById(box.Id), Times.Once);

    }

    /// <summary>
    /// Tests that an exception is thrown if a box with the given iId does not exist. Also tests that GetBoxById is called once on the repository.
    /// </summary>
    [Fact]
    public void GetBox_NonExistingBox_Test()
    {
        // Arrange
        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        boxRepoMock.Setup(r => r.GetBoxById(1)).Returns(() => null);
        
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        var id = 1;
        
        // Act
        var result = service.GetBoxById(id);

        // Assert
        Assert.Null(result);
        boxRepoMock.Verify(r => r.GetBoxById(id), Times.Once);
    }

    #endregion // GetBox

    #region GetAllBoxes

    /// <summary>
    /// Tests that all boxes are returned, and that GetAllBoxes is called once on the repository.
    /// </summary>
    [Fact]
    public void GetAllBoxes_Test()
    {
        //Arrange
        // Existing data
        var s1 = new Box { Id = 1, customerName = "customerName", size = "size", type = "type"};
        var s2 = new Box { Id = 2, customerName = "customerName2", size = "size2", type = "type2"};
        var boxes = new List<Box>() { s1, s2 };

        Mock<IBoxRepository> boxRepoMock = new Mock<IBoxRepository>();
        boxRepoMock.Setup(r => r.GetAllBoxes()).Returns(boxes);
        
        var mapper = new MapperConfiguration(configuration =>
            {configuration.CreateMap<BoxDTOs, Box>();}).CreateMapper();
        var service = new BoxServive(
            boxRepoMock.Object, new PostBoxValidator(), new BoxValidator(), mapper);

        // Act
        var result = service.GetAllNBoxes();

        // Assert
        Assert.Equal(result.ToList().Count, boxes.Count);
        Assert.Contains(s1, result);
        Assert.Contains(s2, result);
        boxRepoMock.Verify(r => r.GetAllBoxes(), Times.Once);
    }

    #endregion // GetAllBoxes
}