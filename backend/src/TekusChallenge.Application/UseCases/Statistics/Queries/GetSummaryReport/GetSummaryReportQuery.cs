using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Statistics.Queries.GetSummaryReport;

/// <summary>
/// Query to get a comprehensive summary report with all key indicators
/// </summary>
public sealed record GetSummaryReportQuery : IRequest<Response<SummaryReportDto>>
{
}

