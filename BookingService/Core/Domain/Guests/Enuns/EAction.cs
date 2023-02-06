namespace Domain.Guests.Enuns
{
    public enum EAction
    {
        Pay = 0,
        Finish = 1, // after paid and used
        Cancel = 2, // can never be paid
        Refound = 3, // paid then refound
        Reopen = 4 // canceled
    }
}
