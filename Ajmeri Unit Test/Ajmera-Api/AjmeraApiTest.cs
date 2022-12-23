using UnitTest.Helper;
using UnitTest.Models;
using Xunit;

namespace UnitTest.Ajmera_Api
{
    public partial class AjmeraApiTest
    {
        #region fields

        private readonly IApiHelper _apiHelper;
        private readonly string _baseUrl;

        #endregion

        #region ctor

        public AjmeraApiTest()
        {
            _apiHelper = new ApiHelper(new HttpClient());
            _baseUrl = "https://localhost:7191/api/Book/";
        }

        #endregion

        #region get all books with actual api call

        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResult_Api()
        {
            // Act
            var item = await _apiHelper.BaseApiCall<List<Book>>($"{_baseUrl}", new Book(), HttpMethod.Get);

            // Assert
            Assert.Equal<int>(item.Item2, (int)StatusCode.OK);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsAllItems_Api()
        {
            // Act
            var item = await _apiHelper.BaseApiCall<List<Book>>($"{_baseUrl}", new Book(), HttpMethod.Get);
            // Assert
            Assert.IsType<List<Book>>(item.Item1);
        }

        #endregion

        #region get books by id  with actual api call

        [Fact]
        public async void GetById_UnknownGuidPassed_ReturnsNotFoundResult_Api()
        {

            // Act
            var item = await _apiHelper.BaseApiCall<string>($"{_baseUrl}{Guid.NewGuid()}", new Book(), HttpMethod.Get);
            // Assert
            Assert.Equal<int>(item.Item2, (int)StatusCode.NotFound);
        }
        [Fact]
        public async Task GetById_ExistingGuidPassed_ReturnsOkResult_Api()
        {
            // Arrange
            var testGuid = new Guid("BFB52C5D-1D73-4BD1-C764-08DAE408FF2F");
            // Act
            var item = await _apiHelper.BaseApiCall<Book>($"{_baseUrl}{testGuid}", new Book(), HttpMethod.Get);
            // Assert
            Assert.Equal<int>(item.Item2, (int)StatusCode.OK);
        }
        [Fact]
        public async Task GetById_ExistingGuidPassed_ReturnsRightItem_Api()
        {
            // Arrange
            var testGuid = new Guid("BFB52C5D-1D73-4BD1-C764-08DAE408FF2F");
            // Act
            var item = await _apiHelper.BaseApiCall<Book>($"{_baseUrl}{testGuid}", new Book(), HttpMethod.Get);
            // Assert
            Assert.IsType<Book>(item.Item1);
        }

        #endregion

        #region add books with actual api call

        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest_Api()
        {
            // Arrange
            Book book = null;
            // Act
            var item = await _apiHelper.BaseApiCall<string>($"{_baseUrl}", book, HttpMethod.Post);
            // Assert
            Assert.Equal<int>(item.Item2, (int)StatusCode.BadRequest);

        }
        [Fact]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse_Api()
        {
            // Arrange
            var testBook = new Book()
            {
                AuthorName = "Guinness Original 6 Pack",
                Name = "Guinness",
            };

            // Act
            var item = await _apiHelper.BaseApiCall<Book>($"{_baseUrl}", testBook, HttpMethod.Post);

            // Assert
            Assert.Equal<int>(item.Item2, (int)StatusCode.OK);
        }
        [Fact]
        public async Task Add_ValidObjectPassed_ReturnedResponseHasCreatedItem_Api()
        {
            // Arrange
            var testItem = new Book()
            {
                Name = "Guinness world record Original 6 Pack",
                AuthorName = "Guinness record",
            };
            // Act
            var response = await _apiHelper.BaseApiCall<Book>($"{_baseUrl}", testItem, HttpMethod.Post);
            var item = response.Item1 as Book;
            // Assert
            Assert.IsType<Book>(item);
            Assert.Equal(testItem.Name, item.Name);
        }

        #endregion

    }
}
