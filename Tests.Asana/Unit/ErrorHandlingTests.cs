using System.Net;
using Apps.Asana.Extensions;
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
}