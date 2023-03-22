using HR.Models.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HR.Interface;
using HR.Database;
using HR.Models.Repository.Base;
using HR.Utils.Export;
using System.Runtime.Serialization;

namespace HR.Areas.Frontend.Service
{
    public class ExportService : IExport
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<FE_RESUME> _FeResume;
        private readonly IRepository<HR_VACMST> _VacMstRepo;

        public ExportService(HRSysContext hRContext)
        {
            _HRContext = hRContext;
            _FeResume = new HRGenericRepository<FE_RESUME>(_HRContext);
            _VacMstRepo = new HRGenericRepository<HR_VACMST>(_HRContext);
        }

        public Dictionary<string, string> GetDataDictionary(string id)
        {
            Dictionary<string, string> dict = new();

            FE_RESUME resume = _FeResume.GetDataByCondition(x => x.FR_RSM_ID == id).First();
            JToken? baseInfoToken = JsonConvert.DeserializeObject<JToken>(resume.FR_RSM_BCI);
            int SexFlag = 0;
            if (baseInfoToken != null)
            {
                string jobName = _VacMstRepo.GetDataByCondition(x => x.HVM_VAC_SN == Convert.ToInt32($"{baseInfoToken["apply_job"]}")).Select(x => x.HVM_VAC_NAM).First();
                SexFlag = Convert.ToInt32(baseInfoToken["sex"]);
                dict.Add("SystemDate", resume.FR_RSM_ADD_DAT.ToString("yyyy 年 MM 月 dd 日"));
                dict.Add("ApplyJob", jobName);
                dict.Add("Name", $"{baseInfoToken["name"]}");
                dict.Add("Sex", SexFlag == 1 ? "男" : "女");
                dict.Add("Birthday", Convert.ToDateTime(baseInfoToken["birthday"]).ToString("yyyy 年 MM 月 dd 日"));
                dict.Add("Unmarried", $"{baseInfoToken["marriage"]}" == "1" ? "■" : "□");
                dict.Add("Married", $"{baseInfoToken["marriage"]}" == "2" ? "■" : "□");
                dict.Add("HaveChildren", $"{baseInfoToken["marriage"]}" == "3" ? "■" : "□");
                dict.Add("Email", $"{baseInfoToken["email"]?["account"]}@{baseInfoToken["email"]?["host"]}");
                dict.Add("Phone", $"{baseInfoToken["phone"]}");
                dict.Add("Address", $"{baseInfoToken["address"]}");
                dict.Add("SchoolName1", $"{baseInfoToken["edu1"]?["school"]}");
                dict.Add("MajorName1", $"{baseInfoToken["edu1"]?["major"]}");
                dict.Add("DayDepartment1", $"{baseInfoToken["edu1"]?["don"]}" == "0" ? "■" : "□");
                dict.Add("NightDepartment1", $"{baseInfoToken["edu1"]?["don"]}" == "1" ? "■" : "□");
                dict.Add("Graduate1", $"{baseInfoToken["edu1"]?["goq"]}" == "1" ? "■" : "□");
                dict.Add("Quit1", $"{baseInfoToken["edu1"]?["goq"]}" == "0" ? "■" : "□");
                dict.Add("EduStart1", Convert.ToDateTime(baseInfoToken["edu1"]["start"]).ToString("yyyy 年 MM 月"));
                dict.Add("EduEnd1", Convert.ToDateTime(baseInfoToken["edu1"]["end"]).ToString("yyyy 年 MM 月"));
                dict.Add("SchoolName2", $"{baseInfoToken["edu2"]?["school"]}");
                dict.Add("MajorName2", $"{baseInfoToken["edu2"]?["major"]}");
                dict.Add("DayDepartment2", $"{baseInfoToken["edu2"]?["don"]}" == "0" ? "■" : "□");
                dict.Add("NightDepartment2", $"{baseInfoToken["edu2"]?["don"]}" == "1" ? "■" : "□");
                dict.Add("Graduate2", $"{baseInfoToken["edu2"]?["goq"]}" == "1" ? "■" : "□");
                dict.Add("Quit2", $"{baseInfoToken["edu2"]?["goq"]}" == "0" ? "■" : "□");
                dict.Add("EduStart2", Convert.ToDateTime(baseInfoToken["edu2"]["start"]).ToString("yyyy 年 MM 月"));
                dict.Add("EduEnd2", Convert.ToDateTime(baseInfoToken["edu2"]["end"]).ToString("yyyy 年 MM 月"));
            }

            JToken? statusToken = JsonConvert.DeserializeObject<JToken>(resume.FR_RSM_STATUS);
            if (statusToken != null)
            {
                dict.Add("NoneSpecialID", statusToken["special"]["id"].Values<string>().ToArray().Contains("0") ? "■" : "□");
                dict.Add("Aboriginal", statusToken["special"]["id"].Values<string>().ToArray().Contains("1") ? "■" : "□");
                dict.Add("PhyAndMentalDisabled", statusToken["special"]["id"].Values<string>().ToArray().Contains("2") ? "■" : "□");
                dict.Add("Foreigner", statusToken["special"]["id"].Values<string>().ToArray().Contains("3") ? "■" : "□");
                dict.Add("Other", statusToken["special"]["id"].Values<string>().ToArray().Contains("4") ? "■" : "□");
                dict.Add("OtherIDContent", statusToken["special"]["id"].Values<string>().ToArray().Contains("4") ? $"{statusToken["special"]?["other"]}" : "");
                dict.Add("EconomicAtatus1", statusToken["eco"].Values<string>().ToArray().Contains("1") ? "■" : "□");
                dict.Add("EconomicAtatus2", statusToken["eco"].Values<string>().ToArray().Contains("2") ? "■" : "□");
                dict.Add("EconomicAtatus3", statusToken["eco"].Values<string>().ToArray().Contains("3") ? "■" : "□");
                dict.Add("EconomicAtatus4", statusToken["eco"].Values<string>().ToArray().Contains("4") ? "■" : "□");
                dict.Add("MilitaryService1", SexFlag == 1 && $"{statusToken["ms"]?["state"]}" == "1" ? "■" : "□");
                dict.Add("MilitaryService2", SexFlag == 1 && $"{statusToken["ms"]?["state"]}" == "2" ? "■" : "□");
                dict.Add("MilitaryService3", SexFlag == 1 && $"{statusToken["ms"]?["state"]}" == "3" ? "■" : "□");
                dict.Add("MSReason", SexFlag == 1 && $"{statusToken["ms"]?["state"]}" == "3" ? $"{statusToken["ms"]?["reason"]}" : "");
                dict.Add("FocusItem1", statusToken["focus"]["items"].Values<string>().ToArray().Contains("1") ? "■" : "□");
                dict.Add("FocusItem2", statusToken["focus"]["items"].Values<string>().ToArray().Contains("2") ? "■" : "□");
                dict.Add("FocusItem3", statusToken["focus"]["items"].Values<string>().ToArray().Contains("3") ? "■" : "□");
                dict.Add("FocusItem4", statusToken["focus"]["items"].Values<string>().ToArray().Contains("4") ? "■" : "□");
                dict.Add("FocusItem5", statusToken["focus"]["items"].Values<string>().ToArray().Contains("5") ? "■" : "□");
                dict.Add("FocusItem6", statusToken["focus"]["items"].Values<string>().ToArray().Contains("6") ? "■" : "□");
                dict.Add("FocusItem7", statusToken["focus"]["items"].Values<string>().ToArray().Contains("7") ? "■" : "□");
                dict.Add("OtherFocusContent", statusToken["focus"]["items"].Values<string>().ToArray().Contains("7") ? $"{statusToken["focus"]?["other"]}" : "");

                string[] FinancialLicenses = statusToken["license"]["financial"].Values<string>().ToArray();
                dict.Add("FinancialLicense", FinancialLicenses.Length > 0 ? string.Join("、", FinancialLicenses) : "無");
                string[] RealEstateLicense = statusToken["license"]["realEstate"].Values<string>().ToArray();
                dict.Add("RealEstateLicense", RealEstateLicense.Length > 0 ? string.Join("、", RealEstateLicense) : "無");
                string[] ITLicense = statusToken["license"]["IT"].Values<string>().ToArray();
                dict.Add("ITLicense", ITLicense.Length > 0 ? string.Join("、", ITLicense) : "無");
                string[] LegalLicense = statusToken["license"]["legal"].Values<string>().ToArray();
                dict.Add("LegalLicense", LegalLicense.Length > 0 ? string.Join("、", LegalLicense) : "無");
                string[] OtherLicense = statusToken["license"]["other"].Values<string>().ToArray();
                dict.Add("OtherLicense", OtherLicense.Length > 0 ? string.Join("、", OtherLicense) : "無");
            }

            JToken? QAToken = JsonConvert.DeserializeObject<JToken>(resume.FR_RSM_QA);
            if (QAToken != null)
            {
                bool SalaryNegoFlag = Convert.ToBoolean(QAToken["expected_salary"]["negotiable"]);
                dict.Add("FixedCharges", $"{QAToken["charges"]}");
                dict.Add("RegistrationDay", Convert.ToDateTime(QAToken["registration_day"]).ToString("yyyy 年 MM 月 dd 日"));
                dict.Add("ExpectedSalary", SalaryNegoFlag ? "面議" : $"{QAToken["expected_salary"]["lowest"]} ~ {QAToken["expected_salary"]["highest"]}");
                dict.Add("CriminalRecord", $"{QAToken["criminal_record"]["exists"]}" == "0" ? "否" : $"是，原因：{QAToken["criminal_record"]["reason"]}");
                dict.Add("Notification", $"{QAToken["notify"]["agree"]}" == "0" ? "否" : $"是，照會公司/主管名稱：{QAToken["notify"]["name"]}");
                string[] AdvancedStudies = QAToken["advanced_study"].Values<string>().ToArray();
                dict.Add("AdvancedStudy", AdvancedStudies.Length > 0 ? string.Join("、", AdvancedStudies) : "無");
                string[] PersonalURLs = QAToken["personal_urls"].Values<string>().ToArray();
                dict.Add("PersonalURL", PersonalURLs.Length > 0 ? string.Join("、", PersonalURLs) : "無");
                dict.Add("Motivation", $"{QAToken["motivation"]}");
            }

            return dict;
        }

        public Dictionary<string, string> GetExperienceDictionary(string id)
        {
            Dictionary<string, string> dict = new();

            FE_RESUME resume = _FeResume.GetDataByCondition(x => x.FR_RSM_ID == id).First();

            JArray? ExperienceArray = JsonConvert.DeserializeObject<JArray>(resume.FR_RSM_EXP);
            if (ExperienceArray != null && ExperienceArray.Count > 0)
            {
                dict.Add("ExperienceCount", $"{ExperienceArray.Count}");
                foreach (JToken exp in ExperienceArray)
                {
                    dict.Add($"Experience{exp["sn"]}-Agency", $"{exp["agency"]}");
                    dict.Add($"Experience{exp["sn"]}-JobTitle", $"{exp["jobTitle"]}");
                    dict.Add($"Experience{exp["sn"]}-ServeDateStart", $"{exp["period_start"]}");
                    dict.Add($"Experience{exp["sn"]}-ServeDateEnd", $"{exp["period_end"]}");
                    dict.Add($"Experience{exp["sn"]}-SalaryStandar", $"{exp["salary"]?["standard"]}" == "0" ? "月薪" : "年薪");
                    dict.Add($"Experience{exp["sn"]}-Salary", $"{exp["salary"]?["amount"]}");
                    dict.Add($"Experience{exp["sn"]}-LeavingReason", $"{exp["leaving_reason"]}");
                }
            }

            return dict;
        }
    }
}
