using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Sections.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class SectionDataHandler : AsyncDataHandler
{
    protected override string Endpoint => $"projects/{_request.GetProjectId()}/sections";

    private readonly SectionRequest _request;

    public SectionDataHandler(InvocationContext invocationContext,
        [ActionParameter] SectionRequest request) : base(invocationContext, request)
    {
        _request = request;
    }
}