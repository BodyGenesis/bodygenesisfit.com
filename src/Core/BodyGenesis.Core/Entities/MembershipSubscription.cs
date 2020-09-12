using System;

namespace BodyGenesis.Core.Entities
{
    public class MembershipSubscription
    {
        public MembershipPlan Plan { get; set; } = new MembershipPlan();
        public bool Active { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool CancellationRequested => CancellationRequestedDate.HasValue;
        public DateTime? CancellationRequestedDate { get; set; }
        public string AgreementData { get; set; } = string.Empty;
        public bool AgreementSigned { get; set; }
        public DateTime? DateAgreementSigned { get; set; }
        public bool CancellationCompleted { get; set; }

        public bool CanCancel => !CancellationRequested && Active;
        public bool CanCancelImmediately => CanCancel && (DateTime.Now.Day <= 15);

        public DateTime CancellationEffectiveDate
        {
            get
            {
                var now = DateTime.Now;

                if (CancellationRequested)
                {
                    return EndDate.Value;
                }

                else if (CanCancelImmediately)
                {
                    return new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 0, 0, 0);
                }

                var next = now.AddMonths(1);

                return new DateTime(next.Year, next.Month, DateTime.DaysInMonth(next.Year, next.Month), 0, 0, 0);
            }
        }

        public void Cancel()
        {
            Active = false;
            EndDate = DateTime.Now;
            CancellationCompleted = true;
        }
        
        public void RequestCancellation()
        {
            if (!CanCancel)
            {
                return;
            }

            EndDate = CancellationEffectiveDate;
            CancellationRequestedDate = DateTime.Now;
        }

        public void SignAgreement(string agreementData)
        {
            AgreementData = agreementData;
            AgreementSigned = true;
            DateAgreementSigned = DateTime.Now;
        }
        
    }
}
