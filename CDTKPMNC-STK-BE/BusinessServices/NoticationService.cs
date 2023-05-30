using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository;
using CDTKPMNC_STK_BE.Models;
using FluentValidation.Validators;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class NoticationService : CommonService
    {
        private readonly INoticationRepository _noticationRepo;
        public NoticationService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _noticationRepo = _unitOfWork.NoticationRepo;
        }

        public NoticaionsReturn GetNoticationByUser(Guid userId)
        {
            var notications = _noticationRepo.GetByUser(userId).ToArray();
            bool haveUnread = false;
            int numberUnread = 0;
            foreach (var notic in notications)
            {
                if (notic.IsRead == false)
                {
                    numberUnread += 1;
                }
            }
            if (numberUnread > 0) haveUnread = true;
            return new NoticaionsReturn
            {
                HaveUnread = haveUnread,
                NumberUnread = numberUnread,
                Notications = notications,
            };
        }

        public NoticaionsReturn GetNoticationByUser(Account user)
        {
             return GetNoticationByUser(user.Id);
        }

        public Notication? GetById(Guid noticationId)
        {
            return _noticationRepo.GetById(noticationId);
        }

        public void MarkAsRead(Notication notication)
        {
            notication!.IsRead = true;
            _noticationRepo.Update(notication);
        }

        public void Send(Account recipientUser, string title, string message)
        {
            var notication = new Notication
            {
                CreateAt = DateTime.Now,
                Title = title,
                Message = message,
                Account = recipientUser,
                AccountId = recipientUser.Id,
                IsRead = false
            };
            _noticationRepo.Add(notication);
        }
    }
}
