namespace MHN.Sync.Entity.Enum
{
    public enum OperationType
    {
        Insert,
        Update,
        Delete,
        UpInsert
    }

    public enum TextCenvertionType
    {
        ENCRYPT,
        DECRYPT,
        NORMAL
    }

    public enum SyncType
    {
        NONE,

        IMPORT,
        EXPORT,
        EMAIL_SYNC_REPORT,

        DISENROLLMENT_IMPORT,
        DISENROLLMENT_EMAIL_SYNC_REPORT,

        LOM_IMPORT,
        LOM_EXPORT,
        LOM_EMAIL_SYNC_REPORT,

        CS_IMPORT,
        CS_EXPORT,
        CS_EMAIL_SYNC_REPORT,
        CS_Mailback_Backlog,


        LITERATURE_IMPORT,
        UPLOAD_LITERATURE,
        WEB_PROSPECT_EXPORT,
        //LITERATURE_IMPORT_NOON,
        //UPLOAD_LITERATURE_NOON,
        MAILBACK_IMPORT,
        MAILBACK_EXPORT_FOR_CAV,
        FULLFILLMENT_UPDATE,

        EMAIL_DATASYNC_REPORT,
        EMAIL_DATASYNC_REPORT_TO_CLIENT,

        EFFORTFILE,
        ENROLLMENT_IMPORT,
        KITS,
        NON_MAILABLE,
        CAREPLAN_IMPORT,
        CAREPLAN_EXPORT_PDF,
        CLEAN_UP_DATA,
        MonthlyAudit,
        GenerateScreenShot,

        MedicaidExport,

        AEP,
        SEP,
        RETENTION,
        DISENROLLMENTSUMMARY,
        MemberProfileFeedUpdate,
        PREDICTIVE_Model,

        INTAKE_WORKFLOW,

        FTPACKNOWLEDGEMENT,
        EMAILACKNOWLEDGEMENT,

        DataSyncJob,

        AGENT_CROSSWALK,
        DATABASEMIGRATION,


        MHN_SSO_USER_SYNC
    }

    public enum PaySpanSyncType
    {
        HUMANA_IMPORT,
        PAYSPAN_EXPORT,
        PAYSPAN_IMPORT
    }

    public enum SyncStatus
    {
        PENDING,
        INPROGRESS,
        FAILED,
        SUCCESS,
        NOTHING_TO_PROCESS
    }

    public enum ResponseType
    {
        DM, //direct mail
        CC, //call center
        MS,  //micro site
        CRM
    }

    public enum SyncExecutionProcess
    {
        AUTO,
        MANUAL
    }

    public enum LogType
    {
        DEBUG,
        ERROR,
        FATAL,
        INFO,
        WARN
    }

    public enum CareplanType
    {
        HEDIS,
        Initial,
        Transition
    }

    public enum PasswordExpiredSyncType
    {
        Check_Pass_Expired_Users,
        Email_To_Reset_Pass,
        Email_Sync_Report
    }

    public enum APICredentialType
    {
        OnDemand,
        Calculated
    }
}
