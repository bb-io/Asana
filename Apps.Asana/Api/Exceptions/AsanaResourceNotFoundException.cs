using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Asana.Api.Exceptions;

public class AsanaResourceNotFoundException(string message) : PluginApplicationException(message);
