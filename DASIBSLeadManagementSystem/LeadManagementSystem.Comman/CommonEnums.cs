
namespace LeadManagementSystem.Comman
{
   public enum LesDecision
    {
        Yes,
        MayBe,
        Decline
    }

    public enum LeadStatus
    {
        NewApplication,
        Qualification,
        DebtorLookup,
        AppCapture,
        Validation,
        CreditAssessment,
        Contract,
        Fraud,
        DeclineNotification,
        Declined,
        Cancelled,
        ChangeBankAcc,
        Banking,
        NotTakenUp,
        IODLetterSent,
        RiskPending,
        ManageResponse,
        Upsold,
        PreQualification,
        CONTRACTManualExposureCheck
    }

    public enum SystemType
    {
        EzflowAPI,
        Dalas
    }

    public enum DalasStatus
    {
        NewApplication = 1,
        Qualification = 2,
        //QualificationIDP = 2,
        DebtorLookup = 3,
        AppCapture = 4,
        Validation = 5,
        CreditAssessment = 6,
        Contract = 7,
        Fraud = 8,
        DeclineNotification = 9,
        Declined = 10,
        Cancelled = 11,
        ChangeBankAcc = 12,
        Banking = 13,
        NotTakenUp = 14,
        IODLetterSent = 43,
        RiskingPending = 44,
        ManageResponse = 45,
        Upsold = 46,
        PreQualification = 47,
        LESDeclined = 48,
        LESApproved = 49,
        LESMayBe = 50,
        OutBoundLead = 51,
        ApplicationCreated = 52,
        CONTRACTManualExposureCheck = 76,
        NoApp = 999
    }


    public enum DalasSubStatus
    {
        RingingOrEngaged = 1,
        LeftMessage = 2,
        ClientToCall = 3,
        CSRToCall = 4,
        IncorrectNumber = 5,
        CSRCancelled = 6,
        ClientCancelled = 7,
        LetterPending = 8,
        TLEscalate = 9,
        ManagerEscalate = 10,
        NotTakenUpPending = 11,
        FaxPending = 12,
        Undecided = 13,
        KYC_Pending = 14,
        KYC_Confirmed = 15,
        DOAuthPending = 16,
        DOAuthGranted = 17,
        TAndCSent = 18,
        TAndCAccepted = 19,
        EFTPending = 35,
        DebtorPending = 36,
        EFTPreparation = 37,
        EFTReceipt = 38,
        EFTError = 39,
        DebtorReceipt = 40,
        DebtorError = 41,
        IODLetterPending = 42,
        CrossSold = 43,
        FraudCheckOK = 44,
        CommunicationsError = 45,
        ProcessingError = 46,
        PersistentCommunicationsErrors = 47,
        InternetApplication = 48,
        SLOffer = 49,
        SLAllocateAttorneys = 52,
        CompleteContract = 53,
        SLAttorneyComplete = 54,
        SLAttorneysSent = 55,
        SLAttorneyAllocated = 56,
        LoanAgreementSentYes = 57,
        DocumentsPending = 58,
        DocumentsConfirmed = 59,
        SpousalConsentRequired = 60,
        SpousalConsentDenied = 61,
        ReloadError = 62,
        ReloadLetterPending = 63,
        DebtorReloadPending = 64,
        DebtorResultPending = 65,
        TeamLeaderNTUQueue = 66,
        EFTInvestigation = 67,
        AVSPending = 68,
        AVSResult = 69,
        SettlementPending = 70,
        SettlementReceipt = 71,

        AVSNegative = 72,
        AVSFailure = 73,
        CreditorsEFTPending = 74,
        ExposureFailure = 75,
        ManualExposureCheck = 76,
        ReContact = 78,
        CoBrandUploadPending = 79,
        RTSPending = 80,
        RTSFailure = 81
    }

}
