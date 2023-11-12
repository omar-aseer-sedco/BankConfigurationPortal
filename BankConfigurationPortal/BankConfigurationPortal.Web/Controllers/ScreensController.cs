using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BankConfigurationPortal.Web.Controllers {
    [ApiAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ScreensController : ApiController {
        private readonly IScreenData db;

        public ScreensController(IScreenData db) {
            this.db = db;
        }

        public IHttpActionResult Get() {
            try {
                string bankName = GetBankNameFromRequest();

                var screen = db.GetActiveScreen(bankName);
                if (screen.ScreenId == 0) return NotFound();

                IEnumerable<TicketingButton> buttons = db.GetButtons(bankName, screen.ScreenId);
                return Ok(GetScreenModel(screen, buttons));
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return InternalServerError(new Exception("An unexpected error has occurred."));
            }
        }

        public IHttpActionResult Get(int? branchId) {
            try {
                string bankName = GetBankNameFromRequest();

                if (branchId is null) return BadRequest();
                if (!db.CheckIfBranchExists(bankName, branchId.Value)) return NotFound();

                var screen = db.GetActiveScreen(bankName);
                if (screen.ScreenId == 0) return NotFound();

                IEnumerable<TicketingButton> buttons = db.GetButtons(bankName, screen.ScreenId, branchId.Value);
                return Ok(GetScreenModel(screen, buttons));
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return InternalServerError(new Exception("An unexpected error has occurred."));
            }
        }

        public IHttpActionResult Get(int? branchId, int? counterId) {
            try {
                string bankName = GetBankNameFromRequest();

                if (branchId is null || counterId is null) return BadRequest();
                if (!db.CheckIfCounterExists(bankName, branchId.Value, counterId.Value)) return NotFound();

                var screen = db.GetActiveScreen(bankName);
                if (screen.ScreenId == 0) return NotFound();

                IEnumerable<TicketingButton> buttons = db.GetButtons(bankName, screen.ScreenId, branchId.Value, counterId.Value);
                return Ok(GetScreenModel(screen, buttons));
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return InternalServerError(new Exception("An unexpected error has occurred."));
            }
        }

        private ScreenModel GetScreenModel(TicketingScreen screen, IEnumerable<TicketingButton> buttons) {
            ScreenModel ret = new ScreenModel() {
                BankName = screen.BankName,
                ScreenId = screen.ScreenId,
                ScreenTitle = screen.ScreenTitle,
                ButtonCount = buttons.Count(),
                Buttons = new ButtonModel[buttons.Count()],
            };

            int i = 0;
            foreach (var button in buttons) {
                int serviceId = 0;
                string messageEn = string.Empty, messageAr = string.Empty, serviceNameEn = string.Empty, serviceNameAr = string.Empty;
                if (button is IssueTicketButton issueTicketButton) {
                    serviceId = issueTicketButton.ServiceId;
                    serviceNameEn = issueTicketButton.ServiceNameEn;
                    serviceNameAr = issueTicketButton.ServiceNameAr;
                }
                if (button is ShowMessageButton showMessageButton) {
                    messageEn = showMessageButton.MessageEn;
                    messageAr = showMessageButton.MessageAr;
                }

                ret.Buttons[i] = new ButtonModel {
                    ButtonId = button.ButtonId,
                    Type = Enum.GetName(typeof(ButtonsConstants.Types), button.Type),
                    NameEn = button.NameEn,
                    NameAr = button.NameAr,
                    ServiceId = serviceId,
                    ServiceNameEn = serviceNameEn,
                    ServiceNameAr = serviceNameAr,
                    MessageEn = messageEn,
                    MessageAr = messageAr
                };
                ++i;
            }

            return ret;
        }

        private string GetBankNameFromRequest() {
            return Encoding.UTF8.GetString(Convert.FromBase64String(Request.Headers.FirstOrDefault(h => h.Key.Equals(AuthenticationConstants.AUTHORIZATION_HEADER_NAME)).Value.First())).Split(':')[0];
        }

        private class ScreenModel {
            public string BankName { get; set; }
            public int ScreenId { get; set; }
            public string ScreenTitle { get; set; }
            public int ButtonCount { get; set; }
            public ButtonModel[] Buttons { get; set; }
        }

        private class ButtonModel {
            public int ButtonId { get; set; }
            public string Type { get; set; }
            public string NameEn { get; set; }
            public string NameAr { get; set; }
            public int ServiceId { get; set; }
            public string ServiceNameEn { get; set; }
            public string ServiceNameAr { get; set; }
            public string MessageEn { get; set; }
            public string MessageAr { get; set; }
        }
    }
}
