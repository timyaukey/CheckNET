﻿using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{

    [Cmdlet(VerbsCommon.Add, "CheckbookNormalTrx")]
    public class AddNormalTrx : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Register Register { get; set; }

        [Parameter(Mandatory = true)]
        public DateTime Date { get; set; }

        [Parameter(Mandatory = true)]
        public string Number { get; set; }

        [Parameter(Mandatory = true)]
        public string Description { get; set; }

        [Parameter]
        public string Memo { get; set; }

        [Parameter(Mandatory = true)]
        public Trx.TrxStatus Status { get; set; }

        [Parameter]
        public SwitchParameter Fake { get; set; }

        [Parameter]
        public SwitchParameter AwaitingReview { get; set; }

        [Parameter]
        public decimal NormalMatchRange { get; set; }

        [Parameter]
        public string ImportKey { get; set; }

        [Parameter]
        public SplitContent OneSplit { get; set; }

        [Parameter]
        public SplitContent[] Splits { get; set; }

        public AddNormalTrx()
        {
            Status = Trx.TrxStatus.Unreconciled;
        }

        protected override void BeginProcessing()
        {
            NormalTrx normalTrx = new NormalTrx(Register);
            normalTrx.NewStartNormal(blnWillAddToRegister: true,
                strNumber_: Number,
                datDate_: Date,
                strDescription_: Description,
                strMemo_: Memo == null ? "" : Memo,
                lngStatus_: Status,
                blnFake_: Fake.IsPresent,
                curNormalMatchRange_: NormalMatchRange,
                blnAwaitingReview_: AwaitingReview.IsPresent,
                blnAutoGenerated_: false,
                intRepeatSeq_: 0,
                strImportKey_: ImportKey == null ? "" : ImportKey,
                strRepeatKey_: "");
            if (OneSplit != null && Splits != null)
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("-OneSplit and -Splits may not both be specified", "DupeSplits"));
            else if (OneSplit != null)
                OneSplit.AddToNormalTrx(normalTrx);
            else if (Splits != null)
            {
                foreach (SplitContent split in Splits)
                {
                    split.AddToNormalTrx(normalTrx);
                }
            }
            else
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Either -OneSplit or -Splits must be specified", "NoSplits"));
            Register.ValidationError += Register_ValidationError;
            try
            {
                normalTrx.Validate();
                if (TrxValidationError != null)
                    ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation(TrxValidationError, "InvalidTrx"));
                Register.NewAddEnd(normalTrx, new LogAdd(), "PowershellAddNormalTrx");
            }
            finally
            {
                Register.ValidationError -= Register_ValidationError;
            }
        }

        private string TrxValidationError = null;

        private void Register_ValidationError(Trx objTrx, string strMsg)
        {
            TrxValidationError = strMsg;
        }
    }
}