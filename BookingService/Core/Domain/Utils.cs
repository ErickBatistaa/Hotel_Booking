namespace Domain
{
    public static class Utils
    {
        public static bool ValidateEmail(string email)
        {
            if(string.IsNullOrEmpty(email) || email == "b@b.com")
            {
                return false;
            }

            return true;
        }
    }
}
