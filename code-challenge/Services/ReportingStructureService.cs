using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                ReportingStructure reportingStructure = new ReportingStructure();
                reportingStructure.employee = _employeeRepository.GetById(id);

                // Calculate the number of direct reports
                reportingStructure.numberOfReports = FindNumberOfReports(reportingStructure.employee);

                return reportingStructure;
            }

            return null;
        }

        public int FindNumberOfReports(Employee employee)
        {
            int count = (employee.DirectReports != null ? employee.DirectReports.Count : 0);

            if(employee != null && employee.DirectReports != null)
            {
                foreach(Employee report in employee.DirectReports)
                {
                    count += FindNumberOfReports(report);
                }
            }

            return count;
        }
    }
}
