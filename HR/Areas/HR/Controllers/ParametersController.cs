using HR.Areas.HR.Service;
using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Base;
using Microsoft.AspNetCore.Mvc;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class ParametersController : AreaBaseController
    {
        private readonly IEntityService<Option> _OptService;
        private readonly IEntityService<OptionTag> _OptTagService;
        private readonly IEntityService<Message> _MsgService;
        private readonly IEntityService<MessageTag> _MsgTagService;

        public ParametersController(
            IEntityService<Option> optServ, IEntityService<OptionTag> optTagServ,
            IEntityService<Message> msgServ, IEntityService<MessageTag> msgTagServ)
        {
            _OptService = optServ;
            _OptTagService = optTagServ;
            _MsgService = msgServ;
            _MsgTagService = msgTagServ;
        }

        public IActionResult SystemOption()
        {
            return View();
        }

        public IActionResult MessageMemo()
        {
            return View();
        }

        #region 選項維護 - 取得資料
        [HttpPost]
        [Route("~/API/Params/Options")]
        public IEnumerable<Option> GetOptions()
        {
            return _OptService.GetData();
        }

        [HttpGet]
        [Route("~/API/Params/Get/Options/{tid}")]
        public IEnumerable<Option> GetCompanies(int tid)
        {
            return (_OptService as OptionService).GetOptionsByTagSn(tid);
        }

        [HttpPost]
        [Route("~/API/Params/OptionTags")]
        public IEnumerable<OptionTag> GetOptionTags()
        {
            return _OptTagService.GetData();
        }
        #endregion

        #region 選項維護 - 新增資料
        [HttpPost]
        [Route("~/API/Params/Add/Option")]
        public Option AddOption(Option opt)
        {
            return _OptService.AddData(opt);
        }
        #endregion

        #region 選項維護 - 更新資料
        [HttpPatch]
        [Route("~/API/Params/Update/Option")]
        public Option UpdateOption(Option opt)
        {
            return _OptService.UpdateData(opt);
        }
        #endregion

        #region 選項維護 - 刪除資料
        [HttpDelete]
        [Route("~/API/Params/Delete/Option/{id}")]
        public Option DeleteOption(int id)
        {
            return _OptService.DeleteData(id);
        }
        #endregion

        #region 訊息備忘錄 - 取得資料
        [HttpPost]
        [Route("~/API/Params/Messages")]
        public IEnumerable<Message> GetMessages()
        {
            return _MsgService.GetData();
        }

        [HttpPost]
        [Route("~/API/Params/MessageTags")]
        public IEnumerable<MessageTag> GetMessageTags()
        {
            return _MsgTagService.GetData();
        }
        #endregion

        #region 訊息備忘錄 - 新增資料
        [HttpPost]
        [Route("~/API/Params/Add/Message")]
        public Message AddMessage(Message msg)
        {
            return _MsgService.AddData(msg);
        }

        [HttpPost]
        [Route("~/API/Params/Add/MessageTag")]
        public MessageTag AddMessageTag(MessageTag tag)
        {
            return _MsgTagService.AddData(tag);
        }
        #endregion

        #region 訊息備忘錄 - 更新資料
        [HttpPatch]
        [Route("~/API/Params/Update/Message")]
        public Message UpdateMessage(Message msg)
        {
            return _MsgService.UpdateData(msg);
        }
        #endregion

        #region 訊息備忘錄 - 刪除資料
        [HttpDelete]
        [Route("~/API/Params/Delete/Message/{id}")]
        public Message DeleteMessage(int id)
        {
            return _MsgService.DeleteData(id);
        }
        #endregion
    }
}
