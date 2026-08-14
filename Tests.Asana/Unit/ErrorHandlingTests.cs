using System.Net;
using Apps.Asana.Api;
using Apps.Asana.Api.Exceptions;
using Apps.Asana.Auth;
using Apps.Asana.Extensions;
using Apps.Asana.Models.Entities;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;

namespace Tests.Asana.Unit;

[TestClass]
public class ErrorHandlingTests
{
    [DataTestMethod]
    [DataRow("undefined")]
    public void EnsureValidJsonContent_UndefinedResponse_ThrowsApplicationException(string content)
    {
        // Arrange
        var response = new RestResponse { Content = content, StatusCode = HttpStatusCode.OK };

        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(response.EnsureValidJsonContent);

        // Assert
        StringAssert.Contains(ex.Message, "it was undefined");
    }
    
    [DataTestMethod]
    [DataRow(" ")]
    public void EnsureValidJsonContent_EmptyResponse_ThrowsApplicationException(string content)
    {
        // Arrange
        var response = new RestResponse { Content = content, StatusCode = HttpStatusCode.OK };

        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(response.EnsureValidJsonContent);

        // Assert
        StringAssert.Contains(ex.Message, "did not return");
    }

    [TestMethod]
    public void ConfigureErrorException_NotFoundWithAsanaError_ThrowsResourceNotFoundException()
    {
        // Arrange
        var response = new RestResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            ContentType = "application/json; charset=UTF-8",
            Content = """{"errors":[{"message":"task: Not a recognized ID: 1234567890"}]}"""
        };

        // Act
        var ex = Assert.ThrowsException<AsanaResourceNotFoundException>(
            () => new TestableAsanaClient().CaptureErrorException(response));

        // Assert
        StringAssert.Contains(ex.Message, "Not a recognized ID");
    }

    [TestMethod]
    public void ConfigureErrorException_NotFoundWithoutContent_ThrowsResourceNotFoundException()
    {
        // Arrange
        var response = new RestResponse { StatusCode = HttpStatusCode.NotFound, Content = string.Empty };

        // Act
        var ex = Assert.ThrowsException<AsanaResourceNotFoundException>(
            () => new TestableAsanaClient().CaptureErrorException(response));

        // Assert
        StringAssert.Contains(ex.Message, "404");
    }

    [DataTestMethod]
    [DataRow(HttpStatusCode.Unauthorized)]
    [DataRow(HttpStatusCode.Forbidden)]
    [DataRow(HttpStatusCode.TooManyRequests)]
    [DataRow(HttpStatusCode.RequestTimeout)]
    [DataRow(HttpStatusCode.InternalServerError)]
    [DataRow(HttpStatusCode.ServiceUnavailable)]
    public void ConfigureErrorException_OtherFailures_DoNotThrowResourceNotFoundException(HttpStatusCode statusCode)
    {
        // Arrange
        var response = new RestResponse
        {
            StatusCode = statusCode,
            ContentType = "application/json; charset=UTF-8",
            Content = """{"errors":[{"message":"Something went wrong"}]}"""
        };

        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => new TestableAsanaClient().CaptureErrorException(response));

        // Assert
        Assert.IsNotInstanceOfType(ex, typeof(AsanaResourceNotFoundException));
        StringAssert.Contains(ex.Message, "Something went wrong");
    }

    [TestMethod]
    public void DeserializeTokenResponse_HtmlBody_ThrowsApplicationException()
    {
        // Arrange
        var html = "<html><head><title>502 Bad Gateway</title></head><body>Asana is down</body></html>";

        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.BadGateway, "text/html", html)));

        // Assert
        StringAssert.Contains(ex.Message, "HTML response");
    }

    [TestMethod]
    public void DeserializeTokenResponse_HtmlBodyWithoutContentType_ThrowsApplicationException()
    {
        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.OK, null, "<!DOCTYPE html><html></html>")));

        // Assert
        StringAssert.Contains(ex.Message, "HTML response");
    }

    [TestMethod]
    public void DeserializeTokenResponse_EmptyBody_ThrowsApplicationException()
    {
        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.NoContent, "application/json", " ")));

        // Assert
        StringAssert.Contains(ex.Message, "without any content");
    }

    [TestMethod]
    public void DeserializeTokenResponse_OAuthError_ThrowsApplicationExceptionWithDescription()
    {
        // Arrange
        var body = """{"error":"invalid_grant","error_description":"The refresh token is invalid"}""";

        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.BadRequest, "application/json", body)));

        // Assert
        StringAssert.Contains(ex.Message, "invalid_grant");
        StringAssert.Contains(ex.Message, "The refresh token is invalid");
    }

    [TestMethod]
    public void DeserializeTokenResponse_FailureStatusWithoutOAuthError_ThrowsApplicationExceptionWithStatus()
    {
        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.InternalServerError, "application/json", """{"message":"boom"}""")));

        // Assert
        StringAssert.Contains(ex.Message, "500");
        StringAssert.Contains(ex.Message, "boom");
    }

    [TestMethod]
    public void DeserializeTokenResponse_NonJsonBody_ThrowsApplicationException()
    {
        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.OK, "text/plain", "Service Unavailable")));

        // Assert
        StringAssert.Contains(ex.Message, "unexpected response");
    }

    [TestMethod]
    public void DeserializeTokenResponse_MissingAccessToken_ThrowsApplicationException()
    {
        // Act
        var ex = Assert.ThrowsException<PluginApplicationException>(
            () => OAuth2TokenService.DeserializeTokenResponse(
                new TokenHttpEntity(HttpStatusCode.OK, "application/json", """{"expires_in":3600}""")));

        // Assert
        StringAssert.Contains(ex.Message, "did not return an access token");
    }

    [TestMethod]
    public void DeserializeTokenResponse_ValidBody_ReturnsAuthData()
    {
        // Arrange
        var body = """{"access_token":"abc","expires_in":"3600","refresh_token":"def"}""";

        // Act
        var authData = OAuth2TokenService.DeserializeTokenResponse(
            new TokenHttpEntity(HttpStatusCode.OK, "application/json", body));

        // Assert
        Assert.AreEqual("abc", authData.AccessToken);
        Assert.AreEqual("3600", authData.ExpiresIn);
        Assert.AreEqual("def", authData.RefreshToken);
    }

    private sealed class TestableAsanaClient : AsanaClient
    {
        public Exception CaptureErrorException(RestResponse response) => ConfigureErrorException(response);
    }
}