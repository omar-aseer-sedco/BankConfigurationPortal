using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BankConfigurationPortal.Web.Controllers {
    public class ScreensController : ApiController {
        private readonly IScreenData db;

        public ScreensController(IScreenData db) {
            this.db = db;
        }

        public IHttpActionResult Get(int branchId = 0, int counterId = 0) {
            try {
                var screen = db.GetActiveScreen(GetBankName());
                if (screen.ScreenId == 0) {
                    return NotFound();
                }

                IEnumerable<TicketingButton> buttons;

                if (branchId == 0 && counterId == 0) {
                    buttons = db.GetButtons(GetBankName(), screen.ScreenId);
                }
                else if (branchId != 0 && counterId == 0) {
                    buttons = db.GetButtons(GetBankName(), screen.ScreenId, branchId);
                }
                else if (branchId != 0 && counterId != 0) {
                    buttons = db.GetButtons(GetBankName(), screen.ScreenId, branchId, counterId);
                }
                else {
                    return BadRequest("You must supply the branch ID to filter the buttons.");
                }

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
                    string messageEn = string.Empty, messageAr = string.Empty;
                    if (button is IssueTicketButton issueTicketButton) {
                        serviceId = issueTicketButton.ServiceId;
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
                        MessageEn = messageEn,
                        MessageAr = messageAr
                    };
                    ++i;
                }

                return Ok(ret);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return InternalServerError();
            }
        }

        private string GetBankName() {
            return "bank1"; // TODO: fix this
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
            public string MessageEn { get; set; }
            public string MessageAr { get; set; }
        }
    }
}
