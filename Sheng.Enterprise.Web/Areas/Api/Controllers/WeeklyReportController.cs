using Sheng.Enterprise.Core;
using Sheng.Enterprise.Infrastructure;
using System;
using System.IO;
using System.Web.Mvc;

namespace Sheng.Enterprise.Web.Areas.Api.Controllers
{
	public class WeeklyReportController : EnterpriseController
	{
		private static readonly WeeklyReportManager _weeklyReportManager = WeeklyReportManager.Instance;

		public ActionResult Post()
		{
			WeeklyReport weeklyReport = base.RequestArgs<WeeklyReport>();
			weeklyReport.Domain = base.UserContext.Domain.Id;
			weeklyReport.User = base.UserContext.User.Id;
			DateTime monday;
			DateTime sunday;
			DateTimeHelper.GetWeek(weeklyReport.Year, weeklyReport.WeekOfYear, out monday, out sunday);
			weeklyReport.Monday = monday;
			weeklyReport.Sunday = sunday;
			if (weeklyReport.ItemList != null)
			{
				foreach (WeeklyReportItem expr_77 in weeklyReport.ItemList)
				{
					expr_77.Domain = base.UserContext.Domain.Id;
					expr_77.WeeklyReport = weeklyReport.Id;
					expr_77.User = weeklyReport.User;
					expr_77.Year = weeklyReport.Year;
					expr_77.Month = weeklyReport.Month;
					expr_77.WeekOfYear = weeklyReport.WeekOfYear;
					expr_77.Monday = weeklyReport.Monday;
					expr_77.Sunday = weeklyReport.Sunday;
				}
			}
			WeeklyReportController._weeklyReportManager.Post(weeklyReport);
			var data = new
			{
				WeekLogId = weeklyReport.Id
			};
			return this.RespondDataResult(data);
		}

		public ActionResult Check()
		{
			CheckResult checkResult = base.RequestArgs<CheckResult>();
			checkResult.Checker = base.UserContext.User.Id;
			DateTime monday;
			DateTime sunday;
			DateTimeHelper.GetWeek(checkResult.Year, checkResult.WeekOfYear, out monday, out sunday);
			checkResult.Monday = monday;
			checkResult.Sunday = sunday;
			WeeklyReportController._weeklyReportManager.Check(checkResult);
			return this.RespondResult();
		}

		public ActionResult Uncheck()
		{
			string input = base.Request.QueryString["id"];
			WeeklyReportController._weeklyReportManager.Uncheck(Guid.Parse(input));
			return this.RespondResult();
		}

		public ActionResult ExportByPersonal()
		{
			ExportByPersonalArgs args = base.RequestArgs<ExportByPersonalArgs>();
			FileInfo fileInfo = new FileInfo(ExcelHelper.ExportPersonal(base.Server.MapPath("/"), args));
			return this.RespondDataResult(fileInfo.Name);
		}

		public ActionResult ExportByWorkType()
		{
			ExportByWorkTypeArgs exportByWorkTypeArgs = base.RequestArgs<ExportByWorkTypeArgs>();
			exportByWorkTypeArgs.Domain = base.UserContext.Domain.Id;
			FileInfo fileInfo = new FileInfo(ExcelHelper.ExportWorkType(base.Server.MapPath("/"), exportByWorkTypeArgs));
			return this.RespondDataResult(fileInfo.Name);
		}

		public ActionResult ExportByOrganization()
		{
			ExportByOrganizationArgs exportByOrganizationArgs = base.RequestArgs<ExportByOrganizationArgs>();
			exportByOrganizationArgs.Domain = base.UserContext.Domain.Id;
			FileInfo fileInfo = new FileInfo(ExcelHelper.ExportOrganization(base.Server.MapPath("/"), exportByOrganizationArgs));
			return this.RespondDataResult(fileInfo.Name);
		}
	}
}
