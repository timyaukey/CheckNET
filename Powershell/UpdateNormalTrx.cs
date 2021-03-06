﻿using System;
using System.Collections.Generic;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{

    [Cmdlet(VerbsData.Update, "CheckbookNormalTrx")]
    public class UpdateNormalTrx : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public NormalTrx NormalTrx { get; set; }

        [Parameter]
        public DateTime? Date { get; set; }

        [Parameter]
        public string Number { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public string Memo { get; set; }

        [Parameter]
        public Trx.TrxStatus? Status { get; set; }

        [Parameter]
        public bool? Fake { get; set; }

        [Parameter]
        public bool? AwaitingReview { get; set; }

        [Parameter]
        public decimal? NormalMatchRange { get; set; }

        [Parameter]
        public string ImportKey { get; set; }

        [Parameter]
        public SplitContent OneSplit { get; set; }

        [Parameter]
        public SplitContent[] Splits { get; set; }

        public UpdateNormalTrx()
        {
        }

        protected override void BeginProcessing()
        {
            if (OneSplit == null && Splits == null)
            {
                List<SplitContent> newSplits = new List<SplitContent>();
                foreach(TrxSplit oldSplit in NormalTrx.colSplits)
                {
                    newSplits.Add(new SplitContent
                    {
                        strCatKey = oldSplit.strCategoryKey,
                        strPONumber = oldSplit.strPONumber,
                        strInvoiceNum = oldSplit.strInvoiceNum,
                        datInvoice = oldSplit.datInvoiceDate,
                        datDue = oldSplit.datDueDate,
                        curAmount = oldSplit.curAmount,
                        strBudgetKey = oldSplit.strBudgetKey,
                        strMemo = oldSplit.strMemo,
                        strTerms = oldSplit.strTerms
                    });
                }
                Splits = newSplits.ToArray();
            }
            else if (OneSplit != null && Splits != null)
            {
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("-OneSplit and -Splits may not both be specified", "SplitError"));
            }
            NormalTrxManager mgr = new NormalTrxManager(NormalTrx);
            mgr.Update(delegate (NormalTrx objTrx)
            {
                objTrx.UpdateStartNormal(
                    strNumber_: !string.IsNullOrEmpty(Number) ? Number : objTrx.strNumber,
                    datDate_: Date.HasValue ? Date.Value : objTrx.datDate,
                    strDescription_: !string.IsNullOrEmpty(Description) ? Description : objTrx.strDescription,
                    strMemo_: Memo != null ? Memo : objTrx.strMemo,
                    lngStatus_: Status.HasValue ? Status.Value : objTrx.lngStatus,
                    blnFake_: Fake.HasValue ? Fake.Value : objTrx.blnFake,
                    curNormalMatchRange_: NormalMatchRange.HasValue ? NormalMatchRange.Value : objTrx.curNormalMatchRange,
                    blnAwaitingReview_: AwaitingReview.HasValue ? AwaitingReview.Value : objTrx.blnAwaitingReview,
                    blnAutoGenerated_: false,
                    intRepeatSeq_: objTrx.intRepeatSeq,
                    strImportKey_: !string.IsNullOrEmpty(ImportKey) ? ImportKey : objTrx.strImportKey,
                    strRepeatKey_: objTrx.strRepeatKey);
                if (OneSplit != null)
                    OneSplit.AddToNormalTrx(objTrx);
                else
                {
                    foreach (SplitContent split in Splits)
                    {
                        split.AddToNormalTrx(objTrx);
                    }
                }
                objTrx.objReg.ValidationError += Register_ValidationError;
                try
                {
                    objTrx.Validate();
                    if (TrxValidationError != null)
                        ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation(TrxValidationError, "InvalidTrx"));
                }
                finally
                {
                    objTrx.objReg.ValidationError -= Register_ValidationError;
                }
            }, new LogChange(), "PowershellAddNormalTrx");
        }

        private string TrxValidationError = null;

        private void Register_ValidationError(Trx objTrx, string strMsg)
        {
            TrxValidationError = strMsg;
        }
    }
}
